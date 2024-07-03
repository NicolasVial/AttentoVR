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

    [SerializeField] private GameObject avatarOrGlassPanel;
    [SerializeField] private GameObject avatarSelectionPanel;
    [SerializeField] private GameObject ParametersPanel;
    [SerializeField] private GameObject StartTaskPanel;
    [SerializeField] private GameObject TaskPanel;

    [Header("Parameters variables")]
    [SerializeField] private TextMeshProUGUI incongruencyTxt;
    [SerializeField] private TextMeshProUGUI nbOfTrialsTxt;

    [SerializeField] private IncongruencyController incongruencyController;
    [SerializeField] private TaskLogic taskLogic;
    [SerializeField] private TaskLogger taskLogger;


    private float incongruencyAngleDegree = 0f;
    private int nbOfTrials = 1;
    private AvatarGender avatarGender;
    private bool seeArm = true;

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
        avatarOrGlassPanel.SetActive(true);
        avatarSelectionPanel.SetActive(false);
        ParametersPanel.SetActive(false);
        StartTaskPanel.SetActive(false);
        TaskPanel.SetActive(false);
        incongruencyAngleDegree = 0f;
        incongruencyController.SetIncongruencyAngle(incongruencyAngleDegree);
        incongruencyTxt.text = "+" + incongruencyAngleDegree.ToString() + " deg";
        nbOfTrials = 1;
        nbOfTrialsTxt.text = nbOfTrials.ToString();
        taskLogic.SetNbTrials(nbOfTrials);
        rightHand.enabled = false;
        leftHand.enabled = true;
        maleAvatar.SetActive(false);
        femaleAvatar.SetActive(false);
        maleBody.SetActive(true);
        femaleBody.SetActive(true);
    }

    public void ResetMenu()
    {
        InitPanels();
    }

    public void PressAvatarButton()
    {
        avatarOrGlassPanel.SetActive(false);
        avatarSelectionPanel.SetActive(true);
        seeArm = true;
    }

    public void PressGlassButton()
    {
        avatarOrGlassPanel.SetActive(false);
        avatarSelectionPanel.SetActive(true);
        seeArm = false;
    }

    public void PressMaleAvatarButton()
    {
        avatarSelectionPanel.SetActive(false);
        ParametersPanel.SetActive(true);
        avatarGender = AvatarGender.Male;
    }

    public void PressFemaleAvatarButton()
    {
        avatarSelectionPanel.SetActive(false);
        ParametersPanel.SetActive(true);
        avatarGender = AvatarGender.Female;
    }

    public void PressEndParametersButton()
    {
        ParametersPanel.SetActive(false);
        StartTaskPanel.SetActive(true);
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

    public void PressStartTaskButton()
    {
        if (seeArm)
        {
            femaleBody.SetActive(true);
            maleBody.SetActive(true);
        }
        else
        {
            femaleBody.SetActive(false);
            maleBody.SetActive(false);
        }
        taskLogger.StartTaskLogging();
        StartTaskPanel.SetActive(false);
        TaskPanel.SetActive(true);
        rightHand.enabled = false;
        leftHand.enabled = false;
        taskLogger.WriteToFile("Number of trials: " + nbOfTrials.ToString());
        taskLogger.WriteToFile("Incongruency angle: " + incongruencyAngleDegree.ToString());
        taskLogger.WriteToFile("-------------------------------------------------------------");
        taskLogger.WriteToFile("Starting trial 1...");
        taskLogic.StartTask();
    }

    public void PressIncreaseIncAngle()
    {
        incongruencyAngleDegree += 1f;
        if(incongruencyAngleDegree >= 0)
        {
            incongruencyTxt.text = "+" + incongruencyAngleDegree.ToString() + " deg";
        }
        else
        {
            incongruencyTxt.text = incongruencyAngleDegree.ToString() + " deg";
        }
        incongruencyController.SetIncongruencyAngle(incongruencyAngleDegree);
    }

    public void PressDecreaseIncAngle()
    {
        incongruencyAngleDegree -= 1f;
        if (incongruencyAngleDegree >= 0)
        {
            incongruencyTxt.text = "+" + incongruencyAngleDegree.ToString() + " deg";
        }
        else
        {
            incongruencyTxt.text = incongruencyAngleDegree.ToString() + " deg";
        }
        incongruencyController.SetIncongruencyAngle(incongruencyAngleDegree);
    }

    public void PressIncreaseNbTrials()
    {
        nbOfTrials += 1;
        nbOfTrialsTxt.text = nbOfTrials.ToString();
        taskLogic.SetNbTrials(nbOfTrials);
    }

    public void PressDecreaseNbTrials()
    {
        if (nbOfTrials > 1)
        {
            nbOfTrials -= 1;
            nbOfTrialsTxt.text = nbOfTrials.ToString();
            taskLogic.SetNbTrials(nbOfTrials);
        }
    }
}
