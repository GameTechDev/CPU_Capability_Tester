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
        // Only use if CPU deemed capable to handle it
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
        }
    }

    public void ExplodeObject()
    {
        Debug.Log("Exploded");
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
            ExplodeObject();
    }
}

