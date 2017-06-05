/////////////////////////////////////////////////////////////////////////////////////////////
// Copyright 2017 Intel Corporation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or imlied.
// See the License for the specific language governing permissions and
// limitations under the License.
/////////////////////////////////////////////////////////////////////////////////////////////

#ifdef CAPABILITYTESTER_EXPORTS
#define CAPABILITYTESTER_API __declspec(dllexport)
#else
#define CAPABILITYTESTER_API __declspec(dllimport)
#endif

#include "StatsCollector.h"

extern "C" {
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

	// Not used in the internal calculation, but are exposed to help
	// create more elaborate bucketing systems
	extern CAPABILITYTESTER_API int GetNumPhysicalCores(void);
	extern CAPABILITYTESTER_API double GetComittedMemoryMB(void);
	extern CAPABILITYTESTER_API double GetAvailableMemoryMB(void);

	// These are used in GetMaxFrequency() calculation.
	// GetCoreFreq returns the frequency at collection time, while PercMaxFrequency returns the percentage of the maximum frequency.
	extern CAPABILITYTESTER_API double GetCoreFreq();
	extern CAPABILITYTESTER_API double GetCorePercMaxFrequency();
	extern CAPABILITYTESTER_API void GetFullProcessorNameString(char* buffer, int* bufferSize);
	extern CAPABILITYTESTER_API void GetProcessorName(char* buffer, int* bufferSize);

}