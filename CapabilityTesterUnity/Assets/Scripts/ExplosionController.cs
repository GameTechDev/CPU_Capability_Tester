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

