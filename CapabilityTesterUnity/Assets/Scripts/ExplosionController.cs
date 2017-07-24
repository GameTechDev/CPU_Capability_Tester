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

public class ExplosionController : MonoBehaviour {

    // Explosion arguments
    public float ExplosiveForce;
    public float ExplosiveRadius;
    public Transform ExplosiveTransform;    // Centerpoint of explosion

    public Rigidbody BaseRigidBody;
    public GameObject[] PotentiallyDetachableCubes;
    List<Rigidbody> ObjRigidbodies = new List<Rigidbody>();
    bool IsCPUCapable = false;
    bool HasExploded = false;
    
	void Start ()
    {
        SetCPULevel(CPUCapabilityManager.Singleton.CPUCapabilityLevel);
    }

    public void SetCPULevel(CPUCapabilityManager.SYSTEM_LEVELS sysLevel)
    {
        // Only use if CPU deemed medium or high capability
        if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.HIGH
            || sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.MEDIUM)
        {
            IsCPUCapable = true;

            // add rigidbodies to all little cubes
            for (int i = 0; i < PotentiallyDetachableCubes.Length; i++)
            {
                Rigidbody CurrRigidbody = PotentiallyDetachableCubes[i].AddComponent<Rigidbody>();
                CurrRigidbody.isKinematic = true;
                CurrRigidbody.useGravity = false;
                ObjRigidbodies.Add(CurrRigidbody);
            }
            Debug.Log("(ExplosionController) System capability set to: " + CPUCapabilityManager.Singleton.CPUCapabilityLevel + ", so object (" + gameObject.name + ") is destructible");
        }
        else
        {

            Debug.Log("(ExplosionController) System capability set to: " + CPUCapabilityManager.Singleton.CPUCapabilityLevel + ", so object (" + gameObject.name + ") not destructible");
        }
    }

    public void ExplodeObject()
    {
        HasExploded = true;
        if (IsCPUCapable)
        {
            BaseRigidBody.useGravity = false;
            BaseRigidBody.isKinematic = true;
            BoxCollider[] BaseColliders = GetComponents<BoxCollider>();
            for(int i = 0; i < BaseColliders.Length; i++)
            {
                BaseColliders[i].enabled = false;
            }
            for (int i = 0; i < ObjRigidbodies.Count; i++)
            {
                Rigidbody CurrRigidbody = ObjRigidbodies[i];
                CurrRigidbody.isKinematic = false;
                CurrRigidbody.useGravity = true;
                CurrRigidbody.AddExplosionForce(ExplosiveForce, ExplosiveTransform.position, ExplosiveRadius);
                ObjRigidbodies[i].gameObject.AddComponent<BoxCollider>();
            }
        }
        else
        {
            // Boring destruction implementation
            BaseRigidBody.AddExplosionForce(ExplosiveForce, ExplosiveTransform.position, ExplosiveRadius);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(!HasExploded)
        {
            ExplosiveTransform.position = collision.contacts[0].point;
            ExplodeObject();
        }
    }
}

