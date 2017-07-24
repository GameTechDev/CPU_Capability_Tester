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
