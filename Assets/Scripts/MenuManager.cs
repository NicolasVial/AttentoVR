using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject maleAvatar;
    [SerializeField] private GameObject femaleAvatar;
    [SerializeField] private LineRenderer rightHand;
    [SerializeField] private LineRenderer leftHand;
    [SerializeField] private GameObject maleBody;
    [SerializeField] private GameObject femaleBody;

    [SerializeField] private GameObject avatarSelectionPanel;
    [SerializeField] private GameObject StartTaskPanel;
    [SerializeField] private GameObject TaskPanel;
    [SerializeField] private GameObject chooseBiggerAnglePanel;

    [SerializeField] private TaskLogic taskLogic;
    [SerializeField] private TaskLogger taskLogger;
    [SerializeField] private GameObject blackLine;
    [SerializeField] private GameObject armLinePanel;

    public AvatarGender avatarGender;

    public enum AvatarGender
    {
        Male,
        Female,
    }

    // Start is called before the first frame update
    void Start()
    {
        femaleAvatar.SetActive(false);
        maleAvatar.SetActive(false);
        InitPanels();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitPanels()
    {
        avatarSelectionPanel.SetActive(true);
        StartTaskPanel.SetActive(false);
        TaskPanel.SetActive(false);
        chooseBiggerAnglePanel.SetActive(false);
        rightHand.enabled = false;
        leftHand.enabled = true;
        maleAvatar.SetActive(false);
        femaleAvatar.SetActive(false);
        maleBody.SetActive(true);
        femaleBody.SetActive(true);
        blackLine.SetActive(true);
        armLinePanel.SetActive(false);
    }

    public void ResetMenu()
    {
        InitPanels();
    }

    public void PressMaleAvatarButton()
    {
        avatarSelectionPanel.SetActive(false);
        StartTaskPanel.SetActive(true);
        avatarGender = AvatarGender.Male;
        femaleBody.SetActive(true);
        maleBody.SetActive(true);
        if (avatarGender == AvatarGender.Male)
        {
            maleAvatar.SetActive(true);
            femaleAvatar.SetActive(false);
        }
        else
        {
            femaleAvatar.SetActive(true);
            maleAvatar.SetActive(false);
        }
    }

    public void PressFemaleAvatarButton()
    {
        avatarSelectionPanel.SetActive(false);
        StartTaskPanel.SetActive(true);
        avatarGender = AvatarGender.Female;
        femaleBody.SetActive(true);
        maleBody.SetActive(true);
        if (avatarGender == AvatarGender.Male)
        {
            maleAvatar.SetActive(true);
            femaleAvatar.SetActive(false);
        }
        else
        {
            femaleAvatar.SetActive(true);
            maleAvatar.SetActive(false);
        }
    }

    public void ShowAvatars()
    {
        femaleBody.SetActive(true);
        maleBody.SetActive(true);
        armLinePanel.SetActive(false);
    }

    public void HideAvatars()
    {
        femaleBody.SetActive(false);
        maleBody.SetActive(false);
        armLinePanel.SetActive(true);
    }


    public void PressStartTaskButton()
    {
        blackLine.SetActive(false);
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
}
