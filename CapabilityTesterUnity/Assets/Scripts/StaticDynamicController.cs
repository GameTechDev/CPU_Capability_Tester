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
            Debug.Log("Creating StaticDynamicController");
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
