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

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class CPUSystemManager : MonoBehaviour {

    private static CPUSystemManager Singleton = null;

    CPUCapabilityManager.SYSTEM_LEVEL CurrentLevel = CPUCapabilityManager.SYSTEM_LEVEL.NUM_SYSTEMS;

    public static CPUSystemManager Instance
    {
        get
        {
            if (!Singleton)
            {
                Singleton = FindObjectOfType(typeof(CPUSystemManager)) as CPUSystemManager;

                if (!Singleton)
                {
                    Debug.LogError("There needs to be one active CPUSystemManager script on a gameobject in your scene");
                }
            }
            return Singleton;
        }
    }

    void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Assert.IsNotNull(Singleton, "(Obj:" + gameObject.name + ") Only 1 instance of CPUSystemManager needed at once");
            DestroyImmediate(this);
        }
    }

    void Start()
    {
        // First run - query hardware and choose system level based on CPUCapabilityManager thresholds
        CurrentLevel = CPUCapabilityManager.Instance.CPUCapabilityLevel;
        SceneManager.LoadScene("CPU_Capability_Demo");
        UIController.Instance.RefreshText();
    }

    public void SwitchSetting()
    {
        CurrentLevel = GetNextCPUSetting();
        CPUCapabilityManager.Instance.CPUCapabilityLevel = CurrentLevel;
        SceneManager.LoadScene("CPU_Capability_Demo");
        UIController.Instance.RefreshText();
    }

    public CPUCapabilityManager.SYSTEM_LEVEL GetNextCPUSetting()
    {
        CPUCapabilityManager.SYSTEM_LEVEL nextLevel = (CPUCapabilityManager.SYSTEM_LEVEL)(((((int)CurrentLevel) + 1)) % ((int)CPUCapabilityManager.SYSTEM_LEVEL.NUM_SYSTEMS));
        return nextLevel;
    }
}
