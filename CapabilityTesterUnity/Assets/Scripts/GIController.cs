using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GIController : MonoBehaviour {

    public static GIController Singleton = null;
    
    void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
        }
        else
        {
            Assert.IsNotNull(Singleton, "(Obj:" + gameObject.name + ") Only 1 instance of GIController needed at once");
            DestroyImmediate(this);
        }
    }

    public void Init()
    {
        Debug.Log("Initializing GIController");
    }

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


