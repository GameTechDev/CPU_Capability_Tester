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
