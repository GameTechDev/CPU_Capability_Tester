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
