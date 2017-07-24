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

public class ParticleSystemController : MonoBehaviour
{
    public static ParticleSystemController Singleton = null;

    ParticleSystem[] ParticleSystems;
    public Transform[] CollisionPlanes;

    void Awake()
    {
        if(!Singleton)
        {
            Singleton = this;
            Debug.Log("Creating ParticleSystemController");
        }
        else
        {
            Assert.IsNotNull(Singleton, "(Obj:" + gameObject.name + ") Only 1 instance of ParticleSystemController needed at once");
            DestroyImmediate(this);
        }
    }

    public void Init()
    {
        ParticleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        Debug.Log("Initializing ParticleSystemController");
    }

    void Start()
    {
        SetCPULevel(CPUCapabilityManager.Singleton.CPUCapabilityLevel);
    }


    public void SetCPULevel(CPUCapabilityManager.SYSTEM_LEVELS sysLevel)
    {
        if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.HIGH)
        {
            for (int i = 0; i < ParticleSystems.Length; i++)
            {
                var particleSysMain = ParticleSystems[i].main;
                var particleSysCollision = ParticleSystems[i].collision;
                var particleSysEmission = ParticleSystems[i].emission;
                particleSysEmission.rateOverTime = 400.0f;
                particleSysMain.maxParticles = 20000;
                particleSysCollision.enabled = true;
                particleSysCollision.type = ParticleSystemCollisionType.World;
            }
        }
        else if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.MEDIUM)
        {
            for (int i = 0; i < ParticleSystems.Length; i++)
            {
                var particleSysMain = ParticleSystems[i].main;
                var particleSysCollision = ParticleSystems[i].collision;
                var particleSysEmission = ParticleSystems[i].emission;
                particleSysEmission.rateOverTime = 300.0f;
                particleSysMain.maxParticles = 10000;
                particleSysCollision.enabled = true;
                particleSysCollision.type = ParticleSystemCollisionType.World;
            }
        }
        else if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.LOW)
        {
            for (int i = 0; i < ParticleSystems.Length; i++)
            {
                var particleSysMain = ParticleSystems[i].main;
                var particleSysCollision = ParticleSystems[i].collision;
                var particleSysEmission = ParticleSystems[i].emission;
                particleSysEmission.rateOverTime = 200.0f;
                particleSysMain.maxParticles = 5000;
                particleSysCollision.enabled = true;
                particleSysCollision.type = ParticleSystemCollisionType.Planes;
                for (int j = 0; j < CollisionPlanes.Length; j++)
                {
                    particleSysCollision.SetPlane(j, CollisionPlanes[j]);
                }
            }
        }
        else if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.OFF)
        {
            for (int i = 0; i < ParticleSystems.Length; i++)
            {
                var particleSysMain = ParticleSystems[i].main;
                var particleSysCollision = ParticleSystems[i].collision;
                var particleSysEmission = ParticleSystems[i].emission;
                particleSysEmission.rateOverTime = 100.0f;
                particleSysMain.maxParticles = 3000;
                particleSysCollision.enabled = false;
            }
        }
    }
}