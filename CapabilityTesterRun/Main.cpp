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
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
/////////////////////////////////////////////////////////////////////////////////////////////

#include <iostream>
#include <Windows.h>
#include "../CapabilityTester/CapabilityTester.h"
#include <stdlib.h>

enum SYSTEM_LEVELS
{
	OFF,
	LOW,
	MEDIUM,
	HIGH,
	NUM_SYSTEMS
};

struct SystemThreshold
{
public:
	int NumLogicalCores;
	double UsablePhysMemoryGB;
	double MaxBaseFrequency;
	double CacheSizeMB;

	// Add your own metrics!
};

std::string CPUName;
const int BufferSize = 512;
int SysLogicalCores;
double SysUsablePhysMemoryGB;
double SysMaxBaseFrequency;
double SysCacheSizeMB;

SystemThreshold LowSettings;
SystemThreshold MedSettings;
SystemThreshold HighSettings;

bool IsSystemHigherThanThreshold(SystemThreshold threshold)
{
	if (threshold.NumLogicalCores < SysLogicalCores && threshold.MaxBaseFrequency < SysMaxBaseFrequency
		&& threshold.UsablePhysMemoryGB < SysUsablePhysMemoryGB && threshold.CacheSizeMB < SysCacheSizeMB)
	{
		return true;
	}
	return false;
}


// Allows you to specify specific CPU models that can be whitelisted
bool IsWhitelistedCPU(SYSTEM_LEVELS sysLevelToCheck)
{
	if (sysLevelToCheck == HIGH)
	{
		return (
			(CPUName == "i7-6700K"));
	}
	else if (sysLevelToCheck == MEDIUM)
	{
		return (
			(CPUName == "i7-7820HK"));
	}
	else if (sysLevelToCheck == LOW)
	{
		return (
			(CPUName == "i5-4590"));
	}
	else if (sysLevelToCheck == OFF)
	{
		return (
			(CPUName == "i3-6100"));
	}
	else
	{
		return false;
	}
}

std::string SysLevelToString(SYSTEM_LEVELS level)
{
	if (level == HIGH)
	{
		return std::string("HIGH");
	}
	else if (level == MEDIUM)
	{
		return std::string("MEDIUM");
	}
	else if (level == LOW)
	{
		return std::string("LOW");
	}
	else
	{
		return std::string("OFF");
	}
}

void SelfCheckDemo()
{
	SYSTEM_LEVELS SystemLevel = OFF;

	// i5-4590
	LowSettings.NumLogicalCores = 4;
	LowSettings.UsablePhysMemoryGB = 8;
	LowSettings.MaxBaseFrequency = 3.3;
	LowSettings.CacheSizeMB = 6;

	// i7 - 7820HK
	MedSettings.NumLogicalCores = 8;
	MedSettings.UsablePhysMemoryGB = 8;
	MedSettings.MaxBaseFrequency = 3.9;
	MedSettings.CacheSizeMB = 8;

	// i7-6700k
	HighSettings.NumLogicalCores = 8;
	HighSettings.UsablePhysMemoryGB = 8;
	HighSettings.MaxBaseFrequency = 4.0;
	HighSettings.CacheSizeMB = 8;

	if (IsSystemHigherThanThreshold(HighSettings) || IsWhitelistedCPU(HIGH))
	{
		SystemLevel = HIGH;
	}
	else if (IsSystemHigherThanThreshold(MedSettings) || IsWhitelistedCPU(MEDIUM))
	{
		SystemLevel = MEDIUM;
	}
	else if (IsSystemHigherThanThreshold(LowSettings) || IsWhitelistedCPU(OFF))
	{
		SystemLevel = LOW;
	}
	else
	{
		SystemLevel = OFF;
	}

	std::cout << "Your system level has been categorized as: " << SysLevelToString(SystemLevel) << std::endl;
}

int main()
{
	InitializeResources();

	if (IsIntelCPU())
	{
		char cpuName[BufferSize];
		GetProcessorName(cpuName, (int*)&BufferSize);
		SysLogicalCores = GetNumLogicalCores();
		SysUsablePhysMemoryGB = GetUsablePhysMemoryGB();
		SysMaxBaseFrequency = GetMaxBaseFrequency();
		SysCacheSizeMB = GetCacheSizeMB();

		CPUName = std::string(cpuName);

		std::cout << "You are running on an Intel CPU - " << CPUName << std::endl;
		std::cout << "The following are values queried from the system :" << std::endl;
		std::cout << "Number of logical cores = " << GetNumLogicalCores() << std::endl;
		std::cout << "Total physical memory = " << GetUsablePhysMemoryGB() << std::endl;
		std::cout << "Maximum base frequency = " << GetMaxBaseFrequency() << std::endl;
		std::cout << "Cache size = " << GetCacheSizeMB() << std::endl;

		SelfCheckDemo();
	}
	else
	{
		std::cout << "You are not running on an Intel CPU" << std::endl;
	}

	FreeResources();
	Sleep(25000);
}