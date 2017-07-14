using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotator : MonoBehaviour {

    public Transform SunTransform;
    public Transform LightTransform;
    public Transform DarkTransform;
    
    public float LightAndDarkTransitionTime = 0.01f;
    bool IsDay = true;
	
	void Update () {
        if (Equals(SunTransform.rotation, LightTransform.rotation))
            IsDay = true;
        if (Equals(SunTransform.rotation, DarkTransform.rotation))
            IsDay = false;

        if (IsDay)
            SunTransform.rotation = Quaternion.Lerp(SunTransform.rotation, DarkTransform.rotation, Time.deltaTime * LightAndDarkTransitionTime);
        else
            SunTransform.rotation = Quaternion.Lerp(SunTransform.rotation, LightTransform.rotation, Time.deltaTime * LightAndDarkTransitionTime);
    }
}
