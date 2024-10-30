using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private LineRenderer rightHand;
    [SerializeField] private LineRenderer leftHand;
    [SerializeField] private GameObject body;

    [SerializeField] private GameObject StartTaskPanel;
    [SerializeField] private GameObject TaskPanel;
    [SerializeField] private GameObject chooseBiggerAnglePanel;

    [SerializeField] private TaskLogic taskLogic;
    [SerializeField] private TaskLogger taskLogger;
    [SerializeField] private IncongruencyController incongruencyController;
    [SerializeField] private GameObject[] blackLines;
    [SerializeField] private GameObject fakeArmGO;
    [SerializeField] private GameObject handOnlyGO;
    [SerializeField] private GameObject armVisualGO;

    [SerializeField] private TextMeshProUGUI actualAngleTxt;
    [SerializeField] private TextMeshProUGUI incongruencyAngleTxt;
    [SerializeField] private TextMeshProUGUI trialNbTxt;
    [SerializeField] private TextMeshProUGUI modTxt;
    [SerializeField] private TextMeshProUGUI stimulus1Txt;
    [SerializeField] private TextMeshProUGUI stimulus2Txt;
    [SerializeField] private TextMeshProUGUI conditionTxt;
    [SerializeField] private TextMeshProUGUI ordSt;

    [SerializeField] private Image firstClickImg;
    [SerializeField] private Image secondClickImg;
    [SerializeField] private Image thirdClickImg;
    [SerializeField] private Image fourthClickImg;


    // Start is called before the first frame update
    void Start()
    {
        InitPanels();
    }

    // Update is called once per frame
    void Update()
    {
        // update actual angle text, with only 1 decimal
        actualAngleTxt.text = "Arm angle: " + incongruencyController.GetAngle().ToString("F1") + "°";
        // update incongruency angle text, with only 1 decimal
        incongruencyAngleTxt.text = "Incongruency angle: " + incongruencyController.GetDiffAngle().ToString("F1") + "°";
    }

    private void InitPanels()
    {
        StartTaskPanel.SetActive(true);
        TaskPanel.SetActive(false);
        chooseBiggerAnglePanel.SetActive(false);
        rightHand.enabled = false;
        leftHand.enabled = true;
        body.SetActive(true);
        for(int i = 0; i < blackLines.Length; i++)
        {
            blackLines[i].SetActive(true);
        }
        fakeArmGO.SetActive(false);
        handOnlyGO.SetActive(false);
        armVisualGO.SetActive(true);
        ResetclickColors();
    }

    public void ResetMenu()
    {
        InitPanels();
    }

    public void ShowBody()
    {
        armVisualGO.SetActive(true);
        body.SetActive(true);
        HideFakeArm();
    }

    public void HideBody()
    {
        body.SetActive(false);
    }

    public void ShowFakeArm()
    {
        fakeArmGO.SetActive(true);
        HideBody();
    }

    public void HideFakeArm()
    {
        fakeArmGO.SetActive(false);
    }

    public void ShowHandOnlyGO()
    {
        handOnlyGO.SetActive(true);
    }

    public void HideHandOnlyGO()
    {
        handOnlyGO.SetActive(false);
    }

    public void ShowGlassOnly()
    {
        ShowBody();
        armVisualGO.SetActive(false);
    }


    public void PressStartTaskButton()
    {
        for (int i = 0; i < blackLines.Length; i++)
        {
            blackLines[i].SetActive(false);
        }
        taskLogger.StartTaskLogging();
        StartTaskPanel.SetActive(false);
        TaskPanel.SetActive(true);
        rightHand.enabled = false;
        leftHand.enabled = false;
        taskLogic.StartTask();
    }

    public void ShowChooseAngle()
    {
        chooseBiggerAnglePanel.SetActive(true);
    }

    public void HideChooseAngle() {
        chooseBiggerAnglePanel.SetActive(false);
    }

    public void SetTrialNbTxt(string s)
    {
        trialNbTxt.text = s;
    }

    public void SetModTxt(string s)
    {
        modTxt.text = s;
    }

    public void SetStimulus1Txt(string s)
    {
        stimulus1Txt.text = s;
    }

    public void SetStimulus2Txt(string s)
    {
        stimulus2Txt.text = s;
    }

    public void SetConditionTxt(string s)
    {
        conditionTxt.text = s;
    }

    public void SetOrdStTxt(string s)
    {
        ordSt.text = s;
    }

    public void SetFirstClickImgColor(Color c)
    {
        firstClickImg.color = c;
    }

    public void SetSecondClickImgColor(Color c)
    {
        secondClickImg.color = c;
    }

    public void SetThirdClickImgColor(Color c)
    {
        thirdClickImg.color = c;
    }

    public void SetFourthClickImgColor(Color c)
    {
        fourthClickImg.color = c;
    }

    public void ResetclickColors()
    {

        SetFirstClickImgColor(Color.white);
        SetSecondClickImgColor(Color.white);
        SetThirdClickImgColor(Color.white);
        SetFourthClickImgColor(Color.white);
    }
}
