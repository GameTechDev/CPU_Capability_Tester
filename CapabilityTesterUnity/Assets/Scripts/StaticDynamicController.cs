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
