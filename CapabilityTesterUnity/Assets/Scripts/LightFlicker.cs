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
