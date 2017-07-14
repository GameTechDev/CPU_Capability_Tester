using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class UIController : MonoBehaviour {

    public Button ToggleSimulationButton = null;
    public Text SimulationButtonText = null;
    public Text CurrentCapabilityText = null;
    public Text CPUNameText = null;
    public Text NumLogicalCoresText = null;
    public Text PhysMemGBText = null;
    public Text MaxBaseFreqText = null;
    public Text CacheSizeText = null;

    public static UIController Singleton = null;

    void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
        }
        else
        {
            Assert.IsNotNull(Singleton, "(Obj:" + gameObject.name + ") Only 1 instance of CPUSystemManager needed at once");
            DestroyImmediate(this);
        }
    }
    
    public void Init()
    {
        ToggleSimulationButton.onClick.AddListener(() => ToggleButton());
    }

    public void ToggleButton()
    {
        StartCoroutine(CPUSystemManager.Singleton.SwitchSetting());
    }

    void Start()
    {
        SimulationButtonText.text = "Press to Force Setting: " + CPUSystemManager.Singleton.GetNextCPUSetting();
        CurrentCapabilityText.text = "Current CPU Capability Level: " + CPUCapabilityManager.Singleton.CPUCapabilityLevel;
        CPUNameText.text = "CPU Name: " + CPUCapabilityManager.Singleton.CPUNameString;
        NumLogicalCoresText.text = "Number of Logical Cores: " + CPUCapabilityManager.Singleton.LogicalCoreCount;
        PhysMemGBText.text = "Physical Memory (GB): " + CPUCapabilityManager.Singleton.PhysicalMemoryGB;
        MaxBaseFreqText.text = "Maxmimum Base Frequency (MHz): " + CPUCapabilityManager.Singleton.MaxBaseFrequency;
        CacheSizeText.text = "Cache Size (MB): " + CPUCapabilityManager.Singleton.CacheSizeMB;
    }
}
