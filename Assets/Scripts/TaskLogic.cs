using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TaskLogic : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI trialNbTxt;
    [SerializeField] private TaskLogger taskLogger;
    [SerializeField] private TaskLogger handPosLogger;
    [SerializeField] private InputActionReference leftHandTrigger;
    [SerializeField] private IncongruencyController incongruencyController;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private ParametersReader parametersReader;
    [SerializeField] private GameObject grayPanel;
    [SerializeField] private Image blurImg;

    private int nbTrials = 1;
    private bool taskStarted = false;
    private bool triggerPressed = false;
    private int triggerCounter = 0;
    private bool isTaskFinished = false;
    private int trialCounter = 1;
    private float firstAngle = 0f;
    private float secondAngle = 0f;
    private float blurValue = 0f;
    private bool pauseRec = false;
    private Vector3 previousPos;
    private float firstAngleIncongruency = 0f;
    private float secondAngleIncongruency = 0f;
    private bool seeArm = true;
    private List<List<string>> parameters = new List<List<string>>();
    private float previousAngle = 0f;

    [Header("Real time angle values.")]
    public float actualAngle;
    public float actualIncongruencyAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        actualAngle = incongruencyController.GetAngle();
        actualIncongruencyAngle = incongruencyController.GetDiffAngle();

        if(triggerPressed)
        {
            if(triggerCounter == 1)
            {
                previousAngle = incongruencyController.GetAngle();
            }
            if(triggerCounter == 2)
            {
                firstAngle = previousAngle - incongruencyController.GetAngle();
                StartCoroutine(SecondIncongrencyChange());
            }
            if(triggerCounter == 3)
            {
                previousAngle = incongruencyController.GetAngle();
            }
            if(triggerCounter == 4)
            {
                pauseRec = true;
                secondAngle = previousAngle - incongruencyController.GetAngle();
                menuManager.ShowChooseAngle();
                blurValue = 0f;
                blurImg.color = new Color(blurImg.color.r, blurImg.color.g, blurImg.color.b, blurValue);
            }

            triggerPressed = false;
        }

        // End of task condition
        if(trialCounter > nbTrials)
        {
            isTaskFinished = true;
            taskStarted = false;
            trialCounter = 1;
            triggerPressed = false;
            firstAngle = 0f;
            secondAngle = 0f;
            triggerCounter = 0;
            pauseRec = false;
            taskLogger.CloseFile();
            handPosLogger.CloseFile();
            menuManager.ResetMenu();
            blurValue = 0f;
            blurImg.color = new Color(blurImg.color.r, blurImg.color.g, blurImg.color.b, blurValue);
        }

        if(triggerCounter == 5)
        {
            trialCounter++;
            triggerPressed = false;
            triggerCounter = 0;
            if(trialCounter <= nbTrials)
            {
                pauseRec = false;
                SetupTrial();
            }
        }
    }

    private IEnumerator FirstIncongrencyChange()
    {
        grayPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        incongruencyController.SetIncongruencyAngle(firstAngleIncongruency);
        yield return new WaitForSeconds(0.5f);
        grayPanel.SetActive(false);
    }

    private IEnumerator SecondIncongrencyChange()
    {
        grayPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        incongruencyController.SetIncongruencyAngle(secondAngleIncongruency);
        yield return new WaitForSeconds(0.5f);
        grayPanel.SetActive(false);
    }

    public void StartTask()
    {
        parameters = parametersReader.ReadParameters();
        nbTrials = parameters.Count;
        trialCounter = 1;
        SetParameters(trialCounter);
        
        taskLogger.WriteToFile(parametersReader.GetFirstLine());
        
        
        SetupTrial();
        taskStarted = true;
        StartCoroutine(StartLogging());
    }

    private void SetupTrial()
    {
        trialNbTxt.text = trialCounter.ToString() + "/" + nbTrials.ToString();
        SetParameters(trialCounter);
        if (seeArm)
        {
            menuManager.ShowAvatars();
        }
        else
        {
            menuManager.HideAvatars();
        }
        blurImg.color = new Color(blurImg.color.r, blurImg.color.g, blurImg.color.b, blurValue);
        StartCoroutine(FirstIncongrencyChange());
    }

    /*
     * Parameters:
     * 0: subj NOT USED
     * 1: mod NOT USED
     * 2: co USED
     * 3: standard NOT USED
     * 4: stim NOT USED
     * 5: block NOT USED
     * 6: mod2 NOT USED
     * 7: stim_letter NOT USED
     * 8: standard_letter NOT USED
     * 9: ord.st USED
     * 10: trial USED
     * 11: stim1 USED  
     * 12: stim2 USED
     * 13: stim1_let NOT USED
     * 14: stim2_let NOT USED
     * 15: incongruency USED
     * 16: visual_real_standard NOT USED
     * 17: blur USED
     * 18: resp USED
     * */

    private void SetParameters(int trialNb)
    {
        if(parameters[trialCounter - 1][2] == "\"internal\"")
        {
            seeArm = true;
        }
        else
        {
            seeArm = false;
        }

        if(parameters[trialCounter - 1][9] == "1")
        {
            firstAngleIncongruency = float.Parse(parameters[trialCounter - 1][15]);
            secondAngleIncongruency = 0f;
        }
        else
        {
            firstAngleIncongruency = 0f;
            secondAngleIncongruency = float.Parse(parameters[trialCounter - 1][15]);
        }

        blurValue = float.Parse(parameters[trialCounter - 1][17]);
    }

    private void WriteTrialData(int answerAngleLarger)
    {
        string line = parameters[trialCounter - 1][0] + ",";
        line += parameters[trialCounter - 1][1] + ",";
        line += parameters[trialCounter - 1][2] + ",";
        line += parameters[trialCounter - 1][3] + ",";
        line += parameters[trialCounter - 1][4] + ",";
        line += parameters[trialCounter - 1][5] + ",";
        line += parameters[trialCounter - 1][6] + ",";
        line += parameters[trialCounter - 1][7] + ",";
        line += parameters[trialCounter - 1][8] + ",";
        line += parameters[trialCounter - 1][9] + ",";
        line += parameters[trialCounter - 1][10] + ",";
        line += parameters[trialCounter - 1][11] + ",";
        line += parameters[trialCounter - 1][12] + ",";
        line += parameters[trialCounter - 1][13] + ",";
        line += parameters[trialCounter - 1][14] + ",";
        line += parameters[trialCounter - 1][15] + ",";
        line += parameters[trialCounter - 1][16] + ",";
        line += parameters[trialCounter - 1][17] + ",";
        line += answerAngleLarger.ToString();
        taskLogger.WriteToFile(line);
    }

    public IEnumerator StartLogging()
    {
        bool firstTime = true;
        while(!isTaskFinished)
        {
            if(!pauseRec)
            {
                if(firstTime)
                {
                    handPosLogger.StartTaskLogging();
                    handPosLogger.WriteToFile("\"trial Nb\",\"x pos\",\"y pos\",\"z pos\"");
                    firstTime = false;
                }
                else
                {
                    string line = trialCounter.ToString() + ",";
                    line += rightHand.transform.position.x.ToString() + ",";
                    line += rightHand.transform.position.y.ToString() + ",";
                    line += rightHand.transform.position.z.ToString();
                    handPosLogger.WriteToFile(line);
                }
            }
            yield return new WaitForSeconds(0.025f);
        }
        isTaskFinished = false;
        yield return null;
    }

    private void Awake()
    {
        leftHandTrigger.action.performed += PressLeftHandTrigger;
    }

    private void OnDestroy()
    {
        leftHandTrigger.action.performed -= PressLeftHandTrigger;
    }

    private void PressLeftHandTrigger(InputAction.CallbackContext context)
    {
        if (taskStarted && triggerCounter!=4)
        {
            triggerPressed = true;
            triggerCounter++;
        }
    }

    public void PressFirstAngleBigger()
    {
        if (taskStarted)
        {
            triggerCounter++;
            WriteTrialData(1);
            menuManager.HideChooseAngle();
        }
    }

    public void PressSecondAngleBigger()
    {
        if (taskStarted)
        {
            triggerCounter++;
            WriteTrialData(2);
            menuManager.HideChooseAngle();
        }
    }
        
}
