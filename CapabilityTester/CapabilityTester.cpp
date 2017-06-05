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

#include "stdafx.h"
#include "CapabilityTester.h"
#include "StatsCollector.h"
#include <iostream>
#include <sstream>
#include <string.h>
#include <stdio.h>
#include <stdlib.h>

InfoCollector* collector = nullptr;

bool IsIntelCPU(void)
{
	if (collector != nullptr)
	{
		return collector->IsIntelCPU();
	}
	else
		return 0;
}

void InitializeResources()
{
	collector = new InfoCollector();
}

int GetNumLogicalCores(void)
{
	if (collector != nullptr)
		return collector->GetLogicalCoreCount();
	else
		return 0;
}

int GetNumPhysicalCores(void)
{
	if (collector != nullptr)
		return collector->GetPhysicalCoreCount();
	else
		return 0;
}

double GetComittedMemoryMB(void)
{
	if (collector != nullptr)
	{
		return static_cast<double>(collector->CollectDataForMetric(RSS_MEMORY_SIZE));
	}
	else
		return 0;
}

double GetAvailableMemoryMB(void)
{
	if (collector != nullptr)
	{
		return static_cast<double>(collector->CollectDataForMetric(AVAILABLE_MEMORY));
	}
	else
		return 0;
}

double GetCacheSizeMB(void)
{
	if (collector != nullptr)
	{
		return collector->GetCacheSize();
	}
	else
		return 0;
}

double GetCoreFreq()
{
	if (collector != nullptr)
	{
		return collector->CollectDataForMetric(CPU_FREQ);
	}
	else
		return 0;
}

double GetCorePercMaxFrequency()
{
	if (collector != nullptr)
	{
		return collector->CollectDataForMetric(CPU_PERCENT_MAX_FREQ_PER_CORE);
	}
	else
		return 0;
}

double GetMaxBaseFrequency()
{
	if (collector != nullptr)
	{
		double coreFreq = GetCoreFreq();
		double percentMaxFreq = GetCorePercMaxFrequency();

		return ((coreFreq * 100) / percentMaxFreq);
	}
	else
		return 0;
}

double GetUsablePhysMemoryGB()
{
	if (collector != nullptr)
	{
		return collector->GetUsablePhysMemoryGB();
	}
	else
		return 0;
}

void FreeResources()
{
	if(collector)
		delete collector;
}

void GetSKU(char **cpuSKU, int* bufferSize)
{
	if (collector != nullptr)
	{
		std::string str = collector->GetFullProcessorNameString();
		if (*bufferSize > (int)str.size())
		{
			*cpuSKU = new char[*bufferSize];
			int r1 = strncpy_s(*cpuSKU, str.size() + 1, str.c_str(), str.size());
			(*cpuSKU)[str.size()] = '\0';
		}
	}
}

void GetFullProcessorNameString(char* cpuSKU, int* bufferSize)
{
	if (collector != nullptr)
	{
		std::string str = collector->GetFullProcessorNameString();
		if (*bufferSize > (int)str.size())
		{
			int r1 = strncpy_s(cpuSKU, str.size() + 1, str.c_str(), str.size());
			cpuSKU[str.size()] = '\0';
		}
	}
}

void GetProcessorName(char* cpuSKU, int* bufferSize)
{
	if (collector != nullptr)
	{
		std::string str = collector->GetProcessorName();
		if (*bufferSize > (int)str.size())
		{
			int r1 = strncpy_s(cpuSKU, str.size() + 1, str.c_str(), str.size());
			cpuSKU[str.size()] = '\0';
		}
	}
}

