#ifdef _WIN32
#include "StatsCollector.h"
#include <algorithm>
#include <iterator>
#include <iostream>
#include <string>
#include <assert.h>
#include <Psapi.h>
#include <D3D11.h>
#include <dxdiag.h>
#include <Setupapi.h>

// PDH counter tokens - indexes token string vector
enum PDH_TOKENS
{
	PDHT_PROCESSOR_INFORMATION,
	PDHT_PROCESSOR_FREQUENCY,
	PDHT_MEMORY,
	PDHT_PERCENT_MAX_FREQ,

	PDHT_COUNT
};

static const wchar_t *vinit[PDHT_COUNT] = { L"Processor Information", L"Processor Frequency", L"Memory", L"% of Maximum Frequency" };

std::vector<std::wstring> sPDHTokens(vinit, std::end(vinit));

size_t cache_l3_size() {
	size_t size = 0;
	DWORD buffer_size = 0;
	DWORD i = 0;
	SYSTEM_LOGICAL_PROCESSOR_INFORMATION * buffer = 0;

	GetLogicalProcessorInformation(0, &buffer_size);
	buffer = (SYSTEM_LOGICAL_PROCESSOR_INFORMATION *)malloc(buffer_size);
	GetLogicalProcessorInformation(&buffer[0], &buffer_size);
	for (i = 0; i != buffer_size / sizeof(SYSTEM_LOGICAL_PROCESSOR_INFORMATION); ++i) {
		if (buffer[i].Relationship == RelationCache && buffer[i].Cache.Level == 3) {
			size = buffer[i].Cache.Size;
			break;
		}
	}

	free(buffer);
	return size;
}

void InfoCollector::InitializeData()
{

	mQueryHandle = NULL;

	// Get cache size
	size_t cacheSize = cache_l3_size();
	mCacheSize = static_cast<double>(cacheSize) / 1024 / 1024;

	// Get total  physical memory
	MEMORYSTATUSEX statex;
	statex.dwLength = sizeof(statex); // I misunderstand that
	GlobalMemoryStatusEx(&statex);
	mUsablePhysicalMemoryGB = (float)statex.ullTotalPhys / (1024 * 1024 * 1024);

	ConstructMetricDataStructure();
	GetCoreCounts();

	mMetricsVec[AVAILABLE_MEMORY]->mPDHPath = std::wstring(L"\\" + sPDHTokens[PDHT_MEMORY] + L"\\Available MBytes");
	mMetricsVec[RSS_MEMORY_SIZE]->mPDHPath = std::wstring(L"\\" + sPDHTokens[PDHT_MEMORY] + L"\\Committed Bytes");
	mMetricsVec[CPU_FREQ]->mPDHPath = std::wstring(L"\\" + sPDHTokens[PDHT_PROCESSOR_INFORMATION] + L"(0," + std::to_wstring(0) + L")\\" + sPDHTokens[PDHT_PROCESSOR_FREQUENCY]);
	mMetricsVec[CPU_PERCENT_MAX_FREQ_PER_CORE]->mPDHPath = std::wstring(L"\\" + sPDHTokens[PDHT_PROCESSOR_INFORMATION] + L"(0," + std::to_wstring(0) + L")\\" + sPDHTokens[PDHT_PERCENT_MAX_FREQ]);

	PDH_STATUS Status = PdhOpenQuery(NULL, NULL, &mQueryHandle);

	if (Status != ERROR_SUCCESS) {
		std::cout << "nPdhOpenQuery failed with status " << Status << std::endl;
		if (mQueryHandle) {
			PdhCloseQuery(mQueryHandle);
		}
	}

	for (auto& it : mMetricsVec) {
		PDH_STATUS status = PdhAddCounter(mQueryHandle, (WCHAR*)(it)->mPDHPath.c_str(), 0, &((it)->mPDHCounter));

		if (status != ERROR_SUCCESS)
		{
			std::cout << "nPdhAddCounter " << (it)->mMetricName << " failed with status " << Status << std::endl;
		}
	}

	PDH_STATUS status = PdhCollectQueryData(mQueryHandle);

	if (status != ERROR_SUCCESS) {
		std::cout << "nPdhOpenQuery failed with status " << Status << std::endl;
		PdhCloseQuery(mQueryHandle);
	}

	CollectPDHData();
}

InfoCollector::InfoCollector()
	: mCPUCoresNumber(0)
	, mCPUPhysicalCoresNumber(0)
{
	InitializeData();
}

InfoCollector::~InfoCollector() {}

void cpuID(unsigned i, unsigned regs[4]) {
#ifdef _WIN32
	__cpuid((int *)regs, (int)i);

#else
	asm volatile
		("cpuid" : "=a" (regs[0]), "=b" (regs[1]), "=c" (regs[2]), "=d" (regs[3])
			: "a" (i), "c" (0));
	// ECX is set to zero for CPUID function 4
#endif
}


bool InfoCollector::IsIntelCPU()
{
	SYSTEM_INFO lpSysInfo;
	GetNativeSystemInfo(&lpSysInfo);
	char vendor[12];
	unsigned regs[4];
	cpuID(0, regs);
	((unsigned *)vendor)[0] = regs[1];
	((unsigned *)vendor)[1] = regs[3];
	((unsigned *)vendor)[2] = regs[2];
	std::string cpuVendor = std::string(vendor, 12);
	const std::string test("GenuineIntel");
	if (cpuVendor.compare(test) != 0)
	{
		return false;
	}
	else
	{
		return true;
	}
}

double InfoCollector::GetCacheSize()
{
	return mCacheSize;
}


float InfoCollector::GetUsablePhysMemoryGB()
{
	return mUsablePhysicalMemoryGB;
}

void InfoCollector::AddCounters() {
	
}

void InfoCollector::BuildMetricURIs() {

}

void InfoCollector::CollectPDHData() {
	PDH_FMT_COUNTERVALUE DisplayValue;
	PDH_STATUS status = PdhCollectQueryData(mQueryHandle);

	if (status != ERROR_SUCCESS) {
		std::cerr << "PdhCollectQueryData failed with " << status << std::endl;

		PdhCloseQuery(mQueryHandle);
	}
	else
	{
		for (auto& it : mMetricsVec) {
			status = PdhGetFormattedCounterValue((it)->mPDHCounter, PDH_FMT_DOUBLE, &((it)->mCounterType), &DisplayValue);
			if (status != ERROR_SUCCESS) {
				std::cerr << "PdhGetFormattedCounterValue failed with status " << status << " on " << (it)->mMetricName << std::endl;
			}
			else {
				if ((it)->mMetricName.compare(mMetricsVec[RSS_MEMORY_SIZE]->mMetricName) == 0)
					(it)->SetCurrentValue(DisplayValue.doubleValue / (1024 * 1024));
				else
					(it)->SetCurrentValue(DisplayValue.doubleValue);
			}
		}
	}
}

typedef BOOL(WINAPI *GetLogicalProcessorInformation_TYPE)(
	PSYSTEM_LOGICAL_PROCESSOR_INFORMATION,
	PDWORD);

void InfoCollector::GetCoreCounts()
{
	SYSTEM_INFO systemInfo;
	::GetSystemInfo(&systemInfo);
	mCPUCoresNumber = systemInfo.dwNumberOfProcessors;
	PSYSTEM_LOGICAL_PROCESSOR_INFORMATION info = NULL;
	DWORD length = 0;
	GetLogicalProcessorInformation_TYPE pGetProcessorInfo = (GetLogicalProcessorInformation_TYPE)GetProcAddress(GetModuleHandle(L"kernel32"), "GetLogicalProcessorInformation");

	if (pGetProcessorInfo)
	{
		BOOL res = pGetProcessorInfo(info, &length);
		if (!res)
		{
			info = (PSYSTEM_LOGICAL_PROCESSOR_INFORMATION)malloc(length);
			if (info)
			{
				pGetProcessorInfo(info, &length);
				DWORD count = length / sizeof(SYSTEM_LOGICAL_PROCESSOR_INFORMATION);
				for (DWORD i = 0; i < count; ++i)
				{
					switch (info[i].Relationship)
					{
					case RelationProcessorCore:
						++mCPUPhysicalCoresNumber;
						break;
					}
				}
				free((void*)info);
			}
		}
	}
	else
	{
		mCPUPhysicalCoresNumber = mCPUCoresNumber;
	}
	assert(0 != mCPUCoresNumber);
}
#endif