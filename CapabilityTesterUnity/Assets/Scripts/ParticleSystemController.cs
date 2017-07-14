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
            Singleton.Init();
        }
        else
        {
            Assert.IsNotNull(Singleton, "(Obj:" + gameObject.name + ") Only 1 instance of ParticleSystemController needed at once");
            DestroyImmediate(this);
        }
    }

    void Start()
    {
        SetCPULevel(CPUCapabilityManager.Singleton.CPUCapabilityLevel);
    }

    public void Init()
    {
        ParticleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        Debug.Log("Initializing ParticleSystemController");
    }

    public void SetCPULevel(CPUCapabilityManager.SYSTEM_LEVELS sysLevel)
    {
        if (sysLevel == CPUCapabilityManager.SYSTEM_LEVELS.HIGH)
        {
            for (int i = 0; i < ParticleSystems.Length; i++)
            {
                var particleSysMain = ParticleSystems[i].main;
                var particleSysCollision = ParticleSystems[i].collision;
                particleSysMain.maxParticles = 10000;
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
                particleSysMain.maxParticles = 5000;
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
                particleSysMain.maxParticles = 3000;
                particleSysCollision.enabled = false;
            }
        }
    }
}