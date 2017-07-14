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
