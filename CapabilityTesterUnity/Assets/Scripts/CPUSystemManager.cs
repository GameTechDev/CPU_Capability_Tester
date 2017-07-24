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
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class CPUSystemManager : MonoBehaviour {

    public static CPUSystemManager Singleton = null;

    static CPUCapabilityManager.SYSTEM_LEVELS CurrentLevel = CPUCapabilityManager.SYSTEM_LEVELS.NUM_SYSTEMS;
    public bool ReloadingLevel = false;

    void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
            DontDestroyOnLoad(this);
            Singleton.Init();
        }
        else
        {
            // Correct timing on scene reload
            if (Singleton.ReloadingLevel)
            {
                Init();
                Singleton.ReloadingLevel = false;
            }
            Assert.IsNotNull(Singleton, "(Obj:" + gameObject.name + ") Only 1 instance of CPUSystemManager needed at once");
            DestroyImmediate(this);
        }
    }

    private void Init()
    {
        CPUCapabilityManager.Singleton.Init();
        if(CurrentLevel == CPUCapabilityManager.SYSTEM_LEVELS.NUM_SYSTEMS)
        {
            // First run - query hardware and choose system level based on CPUCapabilityManager thresholds
            CurrentLevel = CPUCapabilityManager.Singleton.CPUCapabilityLevel;
        }
        else
        {
            CPUCapabilityManager.Singleton.CPUCapabilityLevel = CurrentLevel;
        }

        StaticDynamicController.Singleton.Init();
        ParticleSystemController.Singleton.Init();
        GIController.Singleton.Init();
        UIController.Singleton.Init();

        //StaticDynamicController.Singleton.SetCPULevel(CPUCapabilityManager.Singleton.CPUCapabilityLevel);
        //ParticleSystemController.Singleton.SetCPULevel(CPUCapabilityManager.Singleton.CPUCapabilityLevel);
        //GIController.Singleton.SetCPULevel(CPUCapabilityManager.Singleton.CPUCapabilityLevel);
    }

    public IEnumerator SwitchSetting()
    {
        CurrentLevel = GetNextCPUSetting();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);                  // Reload level with next CPU Setting
        ReloadingLevel = true;
    }

    public CPUCapabilityManager.SYSTEM_LEVELS GetNextCPUSetting()
    {
        CPUCapabilityManager.SYSTEM_LEVELS nextLevel = (CPUCapabilityManager.SYSTEM_LEVELS)(((((int)CurrentLevel) + 1)) % ((int)CPUCapabilityManager.SYSTEM_LEVELS.NUM_SYSTEMS));
        return nextLevel;
    }
}
