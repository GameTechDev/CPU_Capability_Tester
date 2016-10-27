#ifdef CAPABILITYTESTER_EXPORTS
#define CAPABILITYTESTER_API __declspec(dllexport)
#else
#define CAPABILITYTESTER_API __declspec(dllimport)
#endif

#include "StatsCollector.h"

extern "C" {
	enum SYSTEM_LEVELS {
		LOW_END_SYSTEM,
		HIGH_END_SYSTEM,
		NUM_SYSTEM_LEVELS
	};

	// This needs to be done first to query and fill values
	extern CAPABILITYTESTER_API void InitializeResources(void);

	// This is used to determine if running on an Intel CPU
	extern CAPABILITYTESTER_API bool IsIntelCPU(void);

	// Cleans up resources.
	extern CAPABILITYTESTER_API void FreeResources(void);

	// These are the getters used in the bucketing decision
	// They can be used to create your own bucketing solution
	extern CAPABILITYTESTER_API int GetNumLogicalCores(void);
	extern CAPABILITYTESTER_API double GetUsablePhysMemoryGB();
	extern CAPABILITYTESTER_API double GetMaxBaseFrequency();
	extern CAPABILITYTESTER_API double GetCacheSizeMB(void);

	// Compares the values returned above to set thresholds to bucket the system
	extern CAPABILITYTESTER_API SYSTEM_LEVELS CategorizeSystemCPU(void);

	// These are the setters for the thresholds used to compare against in the bucketing decision
	// Default values reflect stats of the i7-6700k CPU as shown @ ark.intel.com
	// int LogicalCoreThreshold = 8;			// # of Threads @ ark.intel.com
	// double MemoryThresholdGB = 7.7;
	// double MaxFrequencyThresholdMhz = 4000;	// Processor Base Frequency @ ark.intel.com
	// double FrequencyMarginOfError = 50.0;
	// double CacheSizeThresholdMB = 8;			// Cache @ ark.intel.com
	// double FrequencyBiasMhz = 50;			// Bias used in bucketing calculation

	extern CAPABILITYTESTER_API void SetThresholdLogicalCoreCount(int count);
	extern CAPABILITYTESTER_API void SetThresholdMemoryGB(double memoryThreshold);
	extern CAPABILITYTESTER_API void SetThresholdMaxBaseFrequencyMhz(double freqMaxBaseMhz);
	extern CAPABILITYTESTER_API void SetThresholdCacheSizeMB(double cacheSize);
	extern CAPABILITYTESTER_API void SetMaxBaseFrequencyBias(double bias);

	// Getters to check current values of the thresholds
	extern CAPABILITYTESTER_API int GetThresholdLogicalCoreCount();
	extern CAPABILITYTESTER_API double GetThresholdMemoryGB();
	extern CAPABILITYTESTER_API double GetThresholdMaxBaseFrequencyMhz();
	extern CAPABILITYTESTER_API double GetThresholdCacheSizeMB();
	extern CAPABILITYTESTER_API double GetMaxBaseFrequencyBias();

	// Not used in the internal calculation, but are exposed to help
	// create more elaborate bucketing systems
	extern CAPABILITYTESTER_API int GetNumPhysicalCores(void);
	extern CAPABILITYTESTER_API double GetComittedMemoryMB(void);
	extern CAPABILITYTESTER_API double GetAvailableMemoryMB(void);

	// These are used in GetMaxFrequency() calculation.
	// GetCoreFreq returns the frequency at collection time, while PercMaxFrequency returns the percentage of the maximum frequency.
	extern CAPABILITYTESTER_API double GetCoreFreq();
	extern CAPABILITYTESTER_API double GetCorePercMaxFrequency();
}