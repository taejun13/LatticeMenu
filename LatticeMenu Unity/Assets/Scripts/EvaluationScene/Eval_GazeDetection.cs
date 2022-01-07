using System;
using UnityEngine;
using UnityEngine.UI;

public class Eval_GazeDetection : FOVEBehavior
{
    [SerializeField]
    Transform eyeCursorTransform;
    [SerializeField]
    public GameObject answerFeedbackPanel, taskPanel, startButton;
    [SerializeField]
    Image startButtonGauge;

    /* For gaze-based menu selection */
    [HideInInspector] public bool menuSelectionMade = false;
    [HideInInspector] public string menuLevel1SelectedItem = "";    // 0: North, 1: East, 2: South, 3: West
    [HideInInspector] public string menuLevel2SelectedItem = "";    // 0: North, 1: East, 2: South, 3: West
    [HideInInspector] public string menuLevel3SelectedItem = "";    // 0: North, 1: East, 2: South, 3: West
    [HideInInspector] public GameObject currentlyGazedAnchor;
    [HideInInspector] public int currentlyGazedAnchor_dir;  // 0: North, 1: East, 2: South, 3: West

    /* For computing gaze ray hit  */
    Ray combinedGazeRay;
    RaycastHit hit_General, hit_LoggingPlate;
    int LM_LoggingPlate, LM_General;

    /* For start button */
    [HideInInspector] public bool startButtonSelected = false;
    float dwellBeginTime_StartButton = -1.0f;
    float dwellThreshold_StartButton = 1.0f;

    /* For delayed menu closing */
    bool menuClosing = false;
    float menuClosingDelay = 0.2f;  // close the menu 0.2s after the final menu selection made (For consistant visual feedback: "blue-colored" anchor when selected).  
    float menuClosingTimer = 0.0f;

    Eval_MenuControl _menuControl;
    Eval_Manager _manager;
    void Start()
    {
        _menuControl = this.GetComponent<Eval_MenuControl>();
        _manager = this.GetComponent<Eval_Manager>();

        // Hide start button before pressing enter
        startButton.SetActive(false);
        answerFeedbackPanel.SetActive(true);
        taskPanel.SetActive(true);
        taskPanel.transform.GetChild(1).GetComponent<TextMesh>().text = "Press space bar to start the task";

        startButtonGauge.fillAmount = 0.0f;
        LM_LoggingPlate = 1 << LayerMask.NameToLayer("LoggingPlate");
        LM_General = 1 << LayerMask.NameToLayer("General");

    }

    void Update()
    {
        /* Compute combined gaze ray */
        var rays = FoveInterface.GetGazeRays().value;
        combinedGazeRay = new Ray((rays.left.origin + rays.right.origin) / 2.0f, ((rays.left.GetPoint(10.0f) + rays.right.GetPoint(10.0f)) / 2.0f - (rays.left.origin + rays.right.origin) / 2.0f));
        eyeCursorTransform.position = combinedGazeRay.GetPoint(10.0f);
        Physics.Raycast(combinedGazeRay, out hit_General, Mathf.Infinity, LM_General);
        Physics.Raycast(combinedGazeRay, out hit_LoggingPlate, Mathf.Infinity, LM_LoggingPlate);

        /* Start button - Dwell for certain amount of time to start the task trial */
        if (!startButtonSelected)
        {
            if (hit_General.point != Vector3.zero && hit_General.collider.gameObject.name == "StartButton")
            {
                if (dwellBeginTime_StartButton == -1.0f)
                    dwellBeginTime_StartButton = Time.time;
                else if (Time.time - dwellBeginTime_StartButton > dwellThreshold_StartButton)
                {
                    startButtonSelected = true;
                    dwellBeginTime_StartButton = -1.0f;
                    startButtonGauge.fillAmount = 0.0f;
                }
                else
                    startButtonGauge.fillAmount = (Time.time - dwellBeginTime_StartButton) / dwellThreshold_StartButton;
            }
            else
            {
                dwellBeginTime_StartButton = -1.0f;
                startButtonGauge.fillAmount = 0.0f;
            }
        }

        /* Delayed menu closing - close the menu 0.2s after the final menu selection made 
         * (For consistant visual feedback: "blue-colored" anchor when selected)         
         */
        if (menuClosing)
        {
            menuClosingTimer += Time.deltaTime;
            if (menuClosingTimer > menuClosingDelay)
            {
                menuSelectionMade = true;
                menuClosingTimer = 0.0f;
                menuClosing = false;
            }
        }

        /* Task managing */
        if (_menuControl.taskStarted)
        {
            // When users' eye gaze entered ItemSelectionZone
            if (hit_General.point != Vector3.zero && hit_General.collider.gameObject.name == "ItemSelectionZone")   
            {
                if (_manager._menuStructure == Eval_Manager.MenuStructure.S_4x4x4)
                {
                    GameObject[] currentlyUsedVisualAnchors = Eval_HelperMethods.GetFourSurroundingAnchors(ref _menuControl.latticeVisualAnchor, menuLevel1SelectedItem + menuLevel2SelectedItem);
                    currentlyGazedAnchor_dir = -1;
                    for (int i = 0; i < currentlyUsedVisualAnchors.Length; i++)
                    {
                        if (hit_General.collider.transform.parent.name == currentlyUsedVisualAnchors[i].name)
                        {
                            currentlyGazedAnchor = currentlyUsedVisualAnchors[i];
                            currentlyGazedAnchor_dir = i;
                            break;
                        }
                    }
                }
                else if (_manager._menuStructure == Eval_Manager.MenuStructure.S_6x6x6)
                {
                    GameObject[] currentlyUsedVisualAnchors = Eval_HelperMethods.GetSixSurroundingAnchors(ref _menuControl.latticeVisualAnchor, menuLevel1SelectedItem + menuLevel2SelectedItem);
                    currentlyGazedAnchor_dir = -1;
                    for (int i = 0; i < currentlyUsedVisualAnchors.Length; i++)
                    {
                        if (hit_General.collider.transform.parent.name == currentlyUsedVisualAnchors[i].name)
                        {
                            currentlyGazedAnchor = currentlyUsedVisualAnchors[i];
                            currentlyGazedAnchor_dir = i;
                            break;
                        }
                    }
                }

                if (currentlyGazedAnchor_dir != -1)
                {                   
                    if (_menuControl.currentMenuLevel == 1)
                        menuLevel1SelectedItem = currentlyGazedAnchor_dir.ToString();
                    else if (_menuControl.currentMenuLevel == 2)
                        menuLevel2SelectedItem = currentlyGazedAnchor_dir.ToString();
                    else if (_menuControl.currentMenuLevel == 3)
                    {
                        menuLevel3SelectedItem = currentlyGazedAnchor_dir.ToString();
                        _menuControl.endStamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                        if (!menuClosing)
                            menuClosing = true;
                    }

                    // Progressive Lattice Menu - showing four visual anchors around the currently selected item
                    if (_manager._menuStructure == Eval_Manager.MenuStructure.S_4x4x4)
                    {
                        for (int i = 0; i < 4; i++)
                            Eval_HelperMethods.GetFourSurroundingAnchors(ref _menuControl.latticeVisualAnchor, menuLevel1SelectedItem + menuLevel2SelectedItem)[i].transform.GetChild(0).GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0xFF, 0xFF, 0xFF, 1f));
                    }
                    else if (_manager._menuStructure == Eval_Manager.MenuStructure.S_6x6x6)
                    {
                        for (int i = 0; i < 6; i++)
                            Eval_HelperMethods.GetSixSurroundingAnchors(ref _menuControl.latticeVisualAnchor, menuLevel1SelectedItem + menuLevel2SelectedItem)[i].transform.GetChild(0).GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0xFF, 0xFF, 0xFF, 1f));
                    }

                    // Visual Feedback - color the selected visual anchor with blue
                    for (int i = 0; i < _menuControl.latticeVisualAnchor.transform.childCount; i++)
                    {
                        float originalAlpha = _menuControl.latticeVisualAnchor.transform.GetChild(i).GetChild(0).GetComponent<Renderer>().material.GetVector("_Color").w;
                        _menuControl.latticeVisualAnchor.transform.GetChild(i).GetChild(0).GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0xFF, 0xFF, 0xFF, originalAlpha));
                    }
                    currentlyGazedAnchor.transform.GetChild(0).GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0.22f, 0.54f, 1f, 1f));
                }
            }
        }
    }
}
