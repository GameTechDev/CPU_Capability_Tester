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

public class ParticleSystemController : MonoBehaviour
{
    ParticleSystem[] ParticleSystems;
    public Transform[] CollisionPlanes;

    void Start()
    {
        Debug.Log("Initializing ParticleSystemController");
        ParticleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        SetCPULevel(CPUCapabilityManager.Instance.CPUCapabilityLevel);
    }

    public void SetCPULevel(CPUCapabilityManager.SYSTEM_LEVEL sysLevel)
    {
        if (sysLevel == CPUCapabilityManager.SYSTEM_LEVEL.HIGH)
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
        else if (sysLevel == CPUCapabilityManager.SYSTEM_LEVEL.MEDIUM)
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
        else if (sysLevel == CPUCapabilityManager.SYSTEM_LEVEL.LOW)
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
        else if (sysLevel == CPUCapabilityManager.SYSTEM_LEVEL.OFF)
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