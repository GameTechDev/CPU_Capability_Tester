#include <iostream>
#include <Windows.h>
#include "../CapabilityTester/CapabilityTester.h"
#include <stdlib.h>

int main()
{
	InitializeResources();

	if (IsIntelCPU())
	{
		std::cout << "Default CPU threshold values based on i7-6700k specs @ ark.intel.com" << std::endl;
		std::cout << "Default core count threshold: " << GetThresholdLogicalCoreCount() << std::endl;
		std::cout << "Default maximum base frequency threshold: " << GetThresholdMaxBaseFrequencyMhz() << std::endl;
		std::cout << "Default cache size threshold: " << GetThresholdCacheSizeMB() << std::endl;
		std::cout << "Default memory threshold: " << GetThresholdMemoryGB() << std::endl;
		std::cout << std::endl;

		std::cout << "The following are values queried from the system:" << std::endl;
		std::cout << "Number of logical cores = " << GetNumLogicalCores() << std::endl;
		std::cout << "Total physical memory = " << GetUsablePhysMemoryGB() << std::endl;
		std::cout << "Maximum base frequency = " << GetMaxBaseFrequency() << std::endl;
		std::cout << "Cache size = " << GetCacheSizeMB() << std::endl;
		SYSTEM_LEVELS systemLevel = CategorizeSystemCPU();
		if (systemLevel == HIGH_END_SYSTEM)
		{
			std::cout << "This system has been categorized as high end.  System values exceeded threshold." << std::endl;
		}
		else
		{
			std::cout << "This system has been categorized as low end.  System values didn't exceed threshold." << std::endl;
		}

		std::cout << std::endl;
		int newCoreCountThreshold = 4;
		double newMemoryThresholdGB = 3.7;
		double newMaxBaseFrequencyThresholdMhz = 2000;
		double newCacheSizeThreshold = 4;
		SetThresholdLogicalCoreCount(newCoreCountThreshold);
		SetThresholdMemoryGB(newMemoryThresholdGB);
		SetThresholdMaxBaseFrequencyMhz(newMaxBaseFrequencyThresholdMhz);
		SetThresholdCacheSizeMB(newCacheSizeThreshold);
		std::cout << "Setting new core count threshold to: " << GetThresholdLogicalCoreCount() << std::endl;
		std::cout << "Setting new memory threshold to: " << GetThresholdMemoryGB() << std::endl;
		std::cout << "Setting new maximum base frequency threshold to: " << GetThresholdMaxBaseFrequencyMhz() << std::endl;
		std::cout << "Setting new cache size threshold to: " << GetThresholdCacheSizeMB() << std::endl;

		systemLevel = CategorizeSystemCPU();
		if (systemLevel == HIGH_END_SYSTEM)
		{
			std::cout << "With the new thresholds, this system has been categorized as high end" << std::endl;
		}
		else
		{
			std::cout << "With the new thresholds, this system has been categorized as low end" << std::endl;
		}

		std::cout << std::endl;
		std::cout << "Extras:" << std::endl;
		std::cout << "Comitted Memory (MB) = " << GetComittedMemoryMB() << std::endl;
		std::cout << "Available Memory (MB) = " << GetAvailableMemoryMB() << std::endl;
		std::cout << "Num physical cores = " << GetNumPhysicalCores() << std::endl;

		const int size = 128;
		char fullCPUNameString[size];
		char cpuUNameString[size];

		GetFullProcessorNameString(fullCPUNameString, (int*)&size);
		std::cout << "Full CPU Name String = " << std::string(fullCPUNameString) << std::endl;

		GetProcessorName(cpuUNameString, (int*)&size);
		std::cout << "CPU Name = " << std::string(cpuUNameString) << std::endl;
	}
	else
	{
		std::cout << "Not an Intel CPU" << std::endl;
	}

	FreeResources();
	Sleep(25000);
}