using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GIController : MonoBehaviour {

	void Start () {
        SetCPULevel(CPUCapabilityManager.Singleton.CPUCapabilityLevel);
    }

    public void SetCPULevel(CPUCapabilityManager.SYSTEM_LEVELS sysLevel)
    {
        if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.HIGH)
        {
            DynamicGI.updateThreshold = 0;
        }
        else if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.MEDIUM)
        {
            DynamicGI.updateThreshold = 25;
        }
        else if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.LOW)
        {
            DynamicGI.updateThreshold = 50;
        }
        else if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.OFF)
        {
            DynamicGI.updateThreshold = 100;
        }
        Debug.Log("(" + gameObject.name + ") System capability set to: " + CPUCapabilityManager.Singleton.CPUCapabilityLevel + ", so setting GI update threshold to: " + DynamicGI.updateThreshold);
    }
}


