///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2017, Intel Corporation
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of 
// the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
// SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.Assertions;
using System.Runtime.InteropServices;
using System;
using System.Text;

public class CPUCapabilityManager : MonoBehaviour
{
    #region Essentials

    // These functions are used to allocate and initialize resources and to deallocate, respectively
    [DllImport("CapabilityTester")]
    private static extern void InitializeResources();
    [DllImport("CapabilityTester")]
    private static extern void FreeResources();
    // This is used to determine if running on an Intel CPU
    [DllImport("CapabilityTester")]
    private static extern bool IsIntelCPU();

    // These are the getters used in the bucketing decision
    // They can be used to create your own bucketing solution
    [DllImport("CapabilityTester")]
    private static extern int GetNumLogicalCores();
    [DllImport("CapabilityTester")]
    private static extern double GetUsablePhysMemoryGB();
    [DllImport("CapabilityTester")]
    private static extern double GetMaxBaseFrequency();
    [DllImport("CapabilityTester")]
    private static extern double GetCacheSizeMB();
    [DllImport("CapabilityTester")]
    private static extern SYSTEM_LEVELS CategorizeSystemCPU();
    #endregion

    #region Threshold Getters and Setters
    // These are the setters for the thresholds used to compare against in the bucketing decision
    // Default values reflect stats of the i7-6700k CPU as shown @ ark.intel.com
    // int LogicalCoreThreshold = 8;			// # of Threads @ ark.intel.com
    // double MemoryThresholdGB = 7.7;
    // double MaxFrequencyThresholdMhz = 4000;	// Processor Base Frequency @ ark.intel.com
    // double FrequencyMarginOfError = 50.0;
    // double CacheSizeThresholdMB = 8;			// Cache @ ark.intel.com
    // double FrequencyBiasMhz = 50;			// Bias used in bucketing calculation
    [DllImport("CapabilityTester")]
    private static extern void SetThresholdLogicalCoreCount(int count);
    [DllImport("CapabilityTester")]
    private static extern void SetThresholdMemoryGB(double memoryThreshold);
    [DllImport("CapabilityTester")]
    private static extern void SetThresholdMaxBaseFrequencyMhz(double freqMaxBaseMhz);
    [DllImport("CapabilityTester")]
    private static extern void SetThresholdCacheSizeMB(double cacheSize);


    // Getters to check current values of the thresholds
    [DllImport("CapabilityTester")]
    private static extern int GetThresholdLogicalCoreCount();
    [DllImport("CapabilityTester")]
    private static extern double GetThresholdMemoryGB();
    [DllImport("CapabilityTester")]
    private static extern double GetThresholdMaxBaseFrequencyMhz();
    [DllImport("CapabilityTester")]
    private static extern double GetThresholdCacheSizeMB();
    [DllImport("CapabilityTester")]
    private static extern double GetMaxBaseFrequencyBias();
    #endregion

    #region Extras

    // Not used in the internal calculation, but are exposed to help
    // create more elaborate bucketing systems
    [DllImport("CapabilityTester")]
    private static extern int GetNumPhysicalCores();
    [DllImport("CapabilityTester")]
    private static extern double GetComittedMemoryMB();
    [DllImport("CapabilityTester")]
    private static extern double GetAvailableMemoryMB();

    // These are used in GetMaxFrequency() calculation.
    // GetCoreFreq returns the frequency at collection time, while PercMaxFrequency returns the percentage of the maximum frequency.
    [DllImport("CapabilityTester")]
    private static extern double GetCoreFreq();

    // These are used to get the CPU name to be used for direct comparison in enabling features for specific models
    [DllImport("CapabilityTester")]
    private static extern void GetFullProcessorNameString(StringBuilder buffer, ref int bufferSize);
    [DllImport("CapabilityTester")]
    private static extern void GetProcessorName(StringBuilder buffer, ref int bufferSize);

    #endregion

    public enum SYSTEM_LEVELS
    {
        OFF,
        LOW,
        MEDIUM,
        HIGH,
        NUM_SYSTEMS
    };

    struct SystemThreshold
    {
        public int NumLogicalCores;
        public double UsablePhysMemoryGB;
        public double MaxBaseFrequency;
        public double CacheSizeMB;

        // Add your own metrics!
    }

    int SysLogicalCores;
    double SysUsablePhysMemoryGB;
    double SysMaxBaseFrequency;
    double SysCacheSizeMB;

    int BufferSize = 512;
    string CPUName = null;

    SystemThreshold LowSettings;
    SystemThreshold MedSettings;
    SystemThreshold HighSettings;

    public static CPUCapabilityManager Singleton = null;

    SYSTEM_LEVELS MySystemLevel = SYSTEM_LEVELS.OFF;
    public SYSTEM_LEVELS CPUCapabilityLevel
    {
        get
        {
            return MySystemLevel;
        }
        set
        {
            MySystemLevel = value;
        }
    }

    public string CPUNameString
    {
        get
        {
            return CPUName;
        }
    }

    public int LogicalCoreCount
    {
        get
        {
            return SysLogicalCores;
        }
    }

    public double PhysicalMemoryGB
    {
        get
        {
            return SysUsablePhysMemoryGB;
        }
    }

    public double MaxBaseFrequency
    {
        get
        {
            return SysMaxBaseFrequency;
        }
    }

    public double CacheSizeMB
    {
        get
        {
            return SysCacheSizeMB;
        }
    }

    bool IsSystemHigherThanThreshold(SystemThreshold threshold)
    {
        if (threshold.NumLogicalCores < SysLogicalCores && threshold.MaxBaseFrequency < SysMaxBaseFrequency
            && threshold.UsablePhysMemoryGB < SysUsablePhysMemoryGB && threshold.CacheSizeMB < SysCacheSizeMB)
        {
            return true;
        }
        return false;
    }

    void Awake()
    {
        if(!Singleton)
        {
            Singleton = this;
        }
        else
        {
            Assert.IsNotNull(Singleton, "(Obj:" + gameObject.name + ") Only 1 instance of CPUCapabilityManager needed at once");
            DestroyImmediate(this);
        }
    }

    public void Init()
    {
        QueryCPU();
    }

    void QueryCPU()
    {
        InitializeResources();
        if (IsIntelCPU())
        {
            StringBuilder cpuNameBuffer = new StringBuilder(BufferSize);
            GetProcessorName(cpuNameBuffer, ref BufferSize);
            SysLogicalCores = GetNumLogicalCores();
            SysUsablePhysMemoryGB = GetUsablePhysMemoryGB();
            SysMaxBaseFrequency = GetMaxBaseFrequency();
            SysCacheSizeMB = GetCacheSizeMB();

            CPUName = cpuNameBuffer.ToString();

            Debug.Log("You are running on an Intel CPU - " + CPUName);
            Debug.Log("The following are values queried from the system:");
            Debug.Log("Number of logical cores = " + SysLogicalCores);
            Debug.Log("Total physical memory = " + SysUsablePhysMemoryGB);
            Debug.Log("Maximum base frequency per core = " + SysMaxBaseFrequency);
            Debug.Log("Cache size = " + SysCacheSizeMB);

            SelfCheckDemo();
        }
        else
        {
            Debug.Log("You are not running on an Intel CPU");
        }
    }

    void SelfCheckDemo()
    {
        // i5-4590
        LowSettings.NumLogicalCores = 4;
        LowSettings.UsablePhysMemoryGB = 8;
        LowSettings.MaxBaseFrequency = 3.3;
        LowSettings.CacheSizeMB = 6;

        // i7 - 7820HK - Set to turbo mode
        MedSettings.NumLogicalCores = 8;
        MedSettings.UsablePhysMemoryGB = 8;
        MedSettings.MaxBaseFrequency = 3.9;
        MedSettings.CacheSizeMB = 8;

        // i7-6700k
        HighSettings.NumLogicalCores = 8;
        HighSettings.UsablePhysMemoryGB = 8;
        HighSettings.MaxBaseFrequency = 4.0;
        HighSettings.CacheSizeMB = 8;

        if (IsSystemHigherThanThreshold(HighSettings) || IsWhitelistedCPU(SYSTEM_LEVELS.HIGH))
        {
            MySystemLevel = SYSTEM_LEVELS.HIGH;
        }
        else if (IsSystemHigherThanThreshold(MedSettings) || IsWhitelistedCPU(SYSTEM_LEVELS.MEDIUM))
        {
            MySystemLevel = SYSTEM_LEVELS.MEDIUM;
        }
        else if (IsSystemHigherThanThreshold(LowSettings) || IsWhitelistedCPU(SYSTEM_LEVELS.OFF))
        {
            MySystemLevel = SYSTEM_LEVELS.LOW;
        }
        else
        {
            MySystemLevel = SYSTEM_LEVELS.OFF;
        }

        Debug.Log("Your system level has been categorized as: " + MySystemLevel);
    }

    // Allows you to specify specific CPU models that can be whitelisted
    private bool IsWhitelistedCPU(SYSTEM_LEVELS sysLevelToCheck)
    {
        if (sysLevelToCheck == SYSTEM_LEVELS.HIGH)
        {
            return (
                (CPUName == "i7-6700K"));
        }
        else if (sysLevelToCheck == SYSTEM_LEVELS.MEDIUM)
        {
            return (
                (CPUName == "i7-7820HK"));
        }
        else if (sysLevelToCheck == SYSTEM_LEVELS.LOW)
        {
            return (
                (CPUName == "i5-4590"));
        }
        else if (sysLevelToCheck == SYSTEM_LEVELS.OFF)
        {
            return (
                (CPUName == "i3-6100"));
        }
        else
        {
            return false;
        }
    }
}