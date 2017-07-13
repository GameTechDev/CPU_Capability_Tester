using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SphereMovement : MonoBehaviour {

    Rigidbody ObjRigidBody = null;
    Vector3 NewPosition = new Vector3();
    Vector3 InitialPosition = new Vector3();
    public float Radius = 10.0f;
    public float RotSpeed = 0.01f;

	// Use this for initialization
	void Start () {
        ObjRigidBody = gameObject.GetComponent<Rigidbody>();
        Assert.IsNotNull(ObjRigidBody, "(" + gameObject.name + ") could not find Rigidbody component");
        Radius = Random.Range(5.0f, 15.0f);
        RotSpeed = Random.Range(1.0f, 5.0f);
        InitialPosition = gameObject.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //NewPosition = gameObject.transform.position;
        NewPosition.x = InitialPosition.x + Mathf.Sin(Time.time * RotSpeed) * Radius;
        NewPosition.y = InitialPosition.y + Mathf.Sin(Time.time * RotSpeed) * Radius;
        NewPosition.z = InitialPosition.z + Mathf.Sin(Time.time * RotSpeed) * Radius;
        ObjRigidBody.MovePosition(NewPosition);
	}
}
