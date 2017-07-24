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
