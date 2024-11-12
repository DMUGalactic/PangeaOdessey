using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public Button stageButton1;
    public Button stageButton2;
    public Button stageButton3;
    public Button stageButton4;
    public Button stageButton5;
    public Button stageButton6;
    public Button stageButton7;
    public Button stageButton8;

    public StageData stageData;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(stageData.grassStage_2 && stageButton2 != null)
        {
            stageButton2.interactable = true;
        }

        if(stageData.iceStage_1 && stageButton3 != null)
        {
            stageButton3.interactable = true;
        }

        if(stageData.iceStage_2 && stageButton4 != null)
        {
            stageButton4.interactable = true;
        }

        if(stageData.fireStage_1 && stageButton5 != null)
        {
            stageButton5.interactable = true;
        }

        if(stageData.fireStage_2 && stageButton6 != null)
        {
            stageButton6.interactable = true;
        }

        if(stageData.darkStage_2 && stageButton7 != null)
        {
            stageButton7.interactable = true;
        }

        if(stageData.darkStage_2 && stageButton8 != null)
        {
            stageButton8.interactable = true;
        }
    }
}
