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

public class LightMovement : MonoBehaviour {
    
    public float TravelTime = 0.1f;
    Vector3 moveVec = new Vector3();

	void Update ()
    {
        moveVec = new Vector3(gameObject.transform.position.x + Mathf.Sin(Time.time) * TravelTime, gameObject.transform.position.y, gameObject.transform.position.z);
        gameObject.transform.position = moveVec;
    }
}
