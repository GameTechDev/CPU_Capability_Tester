#ifndef STATS_COLLECTOR_H
#define STATS_COLLECTOR_H

#include <string>
#include <vector>
#include <memory>
#include <d3d11.h>
#ifdef _WIN32
#include <Pdh.h>
#pragma comment(lib, "pdh.lib")
#endif

static const std::string METRIC_URI_PREFIX = "CapabilityTester";

enum METRIC {
	RSS_MEMORY_SIZE,				// App resident memory
	AVAILABLE_MEMORY,				// Total available memory
	CPU_FREQ,						// Current Frequency of the 0th core
	CPU_PERCENT_MAX_FREQ_PER_CORE,	// The Max Frequency Capable of the CPU - Matches ark.intel.com
	CPU_PREDEFINED_COUNT
};

class Metric
{
public:
	std::string		mMetricName;
#ifdef _WIN32
	std::wstring	mPDHPath;		// PDH Path to this metric's data source
	HCOUNTER		mPDHCounter;	// Handle for this counter
	DWORD			mCounterType;
#endif
	double mCurrentValue;			// Current value of metric
	double GetCurrentValue();
	void SetCurrentValue(double currentValue);
	Metric();
	Metric(std::string metricName);
};

class InfoCollector {
public:
	InfoCollector();
	~InfoCollector();

	void CollectPDHData();															// Refresh all metric data
	double CollectDataForMetric(METRIC metric);										// Allows metric classes to pull their own data
	bool IsIntelCPU();																// Determine if Intel CPU
	
	unsigned int GetLogicalCoreCount();
	unsigned int GetPhysicalCoreCount();
	float GetUsablePhysMemoryGB();
	double GetCacheSize();

	std::unique_ptr<Metric>* GetMetric(METRIC varMetric);

private:
	unsigned int										mCPUCoresNumber;			// Number of LOGICAL cores
	unsigned int										mCPUPhysicalCoresNumber;	// Number of PHYSICAL cores
	unsigned int										mResidentMemory;			// App resident Memory
	unsigned int										mAvailableMemory;			// App resident Memory
	double												mCacheSize;					// CPU cache size
	float												mUsablePhysicalMemoryGB = 0;
	std::vector<std::unique_ptr<Metric>>				mMetricsVec;				// stores single metrics
	void GetCoreCounts();
	void ConstructMetricDataStructure();
	void InitializeData();
#ifdef _WINDOWS
	void AddCounters();
	void BuildMetricURIs();
	HQUERY												mQueryHandle;				// Handle for metrics query to PDH
#endif

};
#endif	// STATS_COLLECTOR_H