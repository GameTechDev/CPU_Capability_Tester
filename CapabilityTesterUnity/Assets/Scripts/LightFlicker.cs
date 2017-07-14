using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

    Light LightComponent;
    public float LowIntensityRangeMin = 3.0f;
    public float LowIntensityRangeMax = 7.0f;

    public float HighIntensityRangeLowMin = 13.0f;
    public float HighIntensityRangeMax = 17.0f;
    public float TransitionTimeMin = 0.3f;
    public float TransitionTimeMax = 1.0f;
    bool TransitioningHigh = true;

    float CurrentIntensityTarget = 0.0f;
    float CurrentTransitionTime = 0.0f;

    void Start () {
        LightComponent = gameObject.GetComponent<Light>();
        LightComponent.intensity = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Approximately(LightComponent.intensity, CurrentIntensityTarget))
        {
            if(TransitioningHigh)
            {
                TransitioningHigh = false;
                CurrentIntensityTarget = Random.Range(LowIntensityRangeMin, LowIntensityRangeMax);
            }
            else
            {
                TransitioningHigh = true;
                CurrentIntensityTarget = Random.Range(HighIntensityRangeLowMin, HighIntensityRangeMax);
            }
            CurrentTransitionTime = Random.Range(TransitionTimeMin, TransitionTimeMax);
        }
        LightComponent.intensity = Mathf.Lerp(LightComponent.intensity, CurrentIntensityTarget, Time.deltaTime * CurrentTransitionTime);
    }
}
