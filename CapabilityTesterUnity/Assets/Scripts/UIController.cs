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

    private static UIController Singleton = null;

    public static UIController Instance
    {
        get
        {
            if (!Singleton)
            {
                Singleton = FindObjectOfType(typeof(UIController)) as UIController;

                if (!Singleton)
                {
                    Debug.LogError("There needs to be one active UIController script on a gameobject in your scene");
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
            Assert.IsNotNull(Singleton, "(Obj:" + gameObject.name + ") Only 1 instance of UIController needed at once");
            DestroyImmediate(this);
        }
    }
    
    public void ToggleButton()
    {
        CPUSystemManager.Instance.SwitchSetting();
    }

    void Start()
    {
        ToggleSimulationButton.onClick.AddListener(() => ToggleButton());
    }

    public void RefreshText()
    {
        SimulationButtonText.text = "Press to Force Setting: " + CPUSystemManager.Instance.GetNextCPUSetting();
        CurrentCapabilityText.text = "Current CPU Capability Level: " + CPUCapabilityManager.Instance.CPUCapabilityLevel;
        CPUNameText.text = "CPU Name: " + CPUCapabilityManager.Instance.CPUNameString;
        NumLogicalCoresText.text = "Number of Logical Cores: " + CPUCapabilityManager.Instance.LogicalCoreCount;
        PhysMemGBText.text = "Physical Memory (GB): " + CPUCapabilityManager.Instance.PhysicalMemoryGB;
        MaxBaseFreqText.text = "Maxmimum Base Frequency (MHz): " + CPUCapabilityManager.Instance.MaxBaseFrequency;
        CacheSizeText.text = "Cache Size (MB): " + CPUCapabilityManager.Instance.CacheSizeMB;
    }
}
