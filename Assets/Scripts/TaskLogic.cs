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
                taskLogger.WriteToFile("-------------------------------------------------------------");
                taskLogger.WriteToFile("First angle = " + firstAngle + ". With incongruency = " + firstAngleIncongruency + ".");
                taskLogger.WriteToFile("Second angle = " + secondAngle + ". With incongruency = " + secondAngleIncongruency + ".");
                taskLogger.WriteToFile("Finished trial " + trialCounter + ".");
                taskLogger.WriteToFile("-------------------------------------------------------------");
                trialCounter++;
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
            taskLogger.WriteToFile("Task finished.");
            taskLogger.CloseFile();
            menuManager.ResetMenu();
            blurValue = 0f;
            blurImg.color = new Color(blurImg.color.r, blurImg.color.g, blurImg.color.b, blurValue);
        }

        if(triggerCounter == 4)
        {
            pauseRec = false;
            triggerCounter = 0;
            SetupTrial();
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
        
        taskLogger.WriteToFile("Number of trials: " + nbTrials.ToString());
        taskLogger.WriteToFile("-------------------------------------------------------------");
        
        
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
        taskLogger.WriteToFile("Starting trial " + trialCounter.ToString() + "...");
        StartCoroutine(FirstIncongrencyChange());
    }

    private void SetParameters(int trialNb)
    {
        seeArm = bool.Parse(parameters[trialCounter - 1][0]);
        firstAngleIncongruency = float.Parse(parameters[trialCounter - 1][1]);
        secondAngleIncongruency = float.Parse(parameters[trialCounter - 1][2]);
        blurValue = float.Parse(parameters[trialCounter - 1][3]);
    }

    public IEnumerator StartLogging()
    {
        bool firstTime = true;
        float keepTime = 0;
        while(!isTaskFinished)
        {
            if(!pauseRec)
            {
                float velocity = (rightHand.transform.position - previousPos).magnitude / (Time.time - keepTime);
                keepTime = Time.time;
                if (firstTime)
                {
                    velocity = 0;
                    firstTime = false;
                }
                taskLogger.WriteToFile("real Angle: " + incongruencyController.GetAngle() + " || " + "Angle with incongruency: " + incongruencyController.GetIncongruencyAngle() + " || " + "Velocity: " + velocity);
                previousPos = rightHand.transform.position;
                yield return new WaitForSeconds(0.025f);
            }
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
        if (taskStarted)
        {
            taskLogger.WriteToFile("Trigger pressed to record an angle.");
            triggerPressed = true;
            triggerCounter++;
        }
    }
        
}
