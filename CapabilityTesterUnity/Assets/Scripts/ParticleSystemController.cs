using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour {

    public ParticleSystem[] ParticleSystems;
    public Transform[] CollisionPlanes;

	void Start () {


        if (CPUCapabilityManager.Singleton.CPUCapabilityLevel == CPUCapabilityManager.SYSTEM_LEVELS.HIGH)
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
        else if (CPUCapabilityManager.Singleton.CPUCapabilityLevel == CPUCapabilityManager.SYSTEM_LEVELS.MEDIUM)
        {
            for (int i = 0; i < ParticleSystems.Length; i++)
            {
                var particleSysMain = ParticleSystems[i].main;
                var particleSysCollision = ParticleSystems[i].collision;
                particleSysMain.maxParticles = 3000;
                particleSysCollision.enabled = true;
                particleSysCollision.type = ParticleSystemCollisionType.World;
            }
        }
        else if (CPUCapabilityManager.Singleton.CPUCapabilityLevel == CPUCapabilityManager.SYSTEM_LEVELS.LOW)
        {
            for (int i = 0; i < ParticleSystems.Length; i++)
            {
                var particleSysMain = ParticleSystems[i].main;
                var particleSysCollision = ParticleSystems[i].collision;
                particleSysMain.maxParticles = 3000;
                particleSysCollision.enabled = true;
                particleSysCollision.type = ParticleSystemCollisionType.Planes;
                for(int j = 0; j < CollisionPlanes.Length; j++)
                {
                    particleSysCollision.SetPlane(i, CollisionPlanes[i]);
                }
            }
        }
        else if (CPUCapabilityManager.Singleton.CPUCapabilityLevel == CPUCapabilityManager.SYSTEM_LEVELS.OFF)
        {
            for (int i = 0; i < ParticleSystems.Length; i++)
            {
                var particleSysMain = ParticleSystems[i].main;
                var particleSysCollision = ParticleSystems[i].collision;
                particleSysMain.maxParticles = 1000;
                particleSysCollision.enabled = false;
            }
        }
		
	}
}

