using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StaticDynamicController : MonoBehaviour {

    public static StaticDynamicController Singleton = null;
    public GameObject[] PotentiallyDynamicObjects;
    int NumDynamicObjects = 0;
    
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
        Debug.Log("Initializing StaticDynamicController");
    }

    void Start () {
        SetCPULevel(CPUCapabilityManager.Singleton.CPUCapabilityLevel);
    }

    public void SetCPULevel(CPUCapabilityManager.SYSTEM_LEVELS sysLevel)
    {
        if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.HIGH)
        {
            NumDynamicObjects = PotentiallyDynamicObjects.Length;
        }
        else if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.MEDIUM)
        {
            NumDynamicObjects = PotentiallyDynamicObjects.Length / 2;
        }
        else if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.LOW)
        {
            NumDynamicObjects = PotentiallyDynamicObjects.Length / 3;
        }
        else if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.OFF)
        {
            NumDynamicObjects = 0;
        }

        Debug.Log("(Obj:" + gameObject.name + ") System capability set to: " + CPUCapabilityManager.Singleton.CPUCapabilityLevel + ", so setting number of dynamic objects to: " + NumDynamicObjects);

        for (int i = 0; i < NumDynamicObjects; i++)
        {

            Rigidbody objRigidBody = PotentiallyDynamicObjects[i].AddComponent<Rigidbody>();
            objRigidBody.useGravity = true;
            objRigidBody.mass = 10;
            PotentiallyDynamicObjects[i].AddComponent<SphereMovement>();
        }
    }
}
