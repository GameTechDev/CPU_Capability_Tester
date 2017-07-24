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
