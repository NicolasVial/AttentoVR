using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TaskLogic : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI trialNbTxt;
    [SerializeField] private TaskLogger taskLogger;
    [SerializeField] private InputActionReference leftHandTrigger;
    [SerializeField] private IncongruencyController incongruencyController;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private GameObject rightHand;

    private int nbTrials = 1;
    private bool taskStarted = false;
    private bool triggerPressed = false;
    private int triggerCounter = 0;
    private bool isTaskFinished = false;
    private int trialCounter = 1;
    private float firstAngle = 0f;
    private float secondAngle = 0f;
    private bool pauseRec = false;
    private Vector3 previousPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerPressed)
        {
            if(triggerCounter == 1)
            {
                   firstAngle = incongruencyController.GetAngle();
            }
            if(triggerCounter == 2)
            {
                pauseRec = true;
                secondAngle = incongruencyController.GetAngle();
                taskLogger.WriteToFile("-------------------------------------------------------------");
                taskLogger.WriteToFile("First angle = " + firstAngle + ".");
                taskLogger.WriteToFile("Second angle = " + secondAngle + ".");
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
        }

        if(triggerCounter == 2)
        {
            taskLogger.WriteToFile("Starting trial " + trialCounter + "...");
            pauseRec = false;
            triggerCounter = 0;
            trialNbTxt.text = trialCounter.ToString() + "/" + nbTrials.ToString();
        }
    }

    public void SetNbTrials(int newNbTrials)
    {
        nbTrials = newNbTrials;
    }

    public void StartTask()
    {
        taskStarted = true;
        trialCounter = 1;
        trialNbTxt.text = trialCounter.ToString() + "/" + nbTrials.ToString();
        StartCoroutine(StartLogging());
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