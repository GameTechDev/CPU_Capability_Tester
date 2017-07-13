using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GIController : MonoBehaviour {

	void Start () {

        if (CPUCapabilityManager.Singleton.CPUCapabilityLevel == CPUCapabilityManager.SYSTEM_LEVELS.HIGH)
        {
            DynamicGI.updateThreshold = 0;
        }
        else if (CPUCapabilityManager.Singleton.CPUCapabilityLevel == CPUCapabilityManager.SYSTEM_LEVELS.MEDIUM)
        {
            DynamicGI.updateThreshold = 25;
        }
        else if (CPUCapabilityManager.Singleton.CPUCapabilityLevel == CPUCapabilityManager.SYSTEM_LEVELS.LOW)
        {
            DynamicGI.updateThreshold = 50;
        }
        else if (CPUCapabilityManager.Singleton.CPUCapabilityLevel == CPUCapabilityManager.SYSTEM_LEVELS.OFF)
        {
            DynamicGI.updateThreshold = 100;
        }
    }
}


