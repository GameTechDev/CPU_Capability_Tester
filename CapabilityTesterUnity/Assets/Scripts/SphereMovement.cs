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
