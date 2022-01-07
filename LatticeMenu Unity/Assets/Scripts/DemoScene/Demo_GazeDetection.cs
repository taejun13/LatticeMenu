/*
 * Writer: Taejun Kim, HCI Lab KAIST - https://taejun13.github.io/
 * Last Update: 2022. 1. 7
 * Lattice Menu: A Low-Error Gaze-Based Marking Menu Utilizing Target-Assisted Gaze Gestures on a Lattice of Visual Anchors (ACM CHI 2022)
 * ACM CHI 22': Conference on Human Factors in Computing Systems.
 * DOI: (TBU)
 */
 
using UnityEngine;
using UnityEngine.UI;

public class Demo_GazeDetection : FOVEBehavior
{
    [SerializeField]
    public Transform eyeCursorTransform;
    [SerializeField]
    Image menuGauge;

    /* For gaze-based menu selection */
    [HideInInspector] public bool menuSelectionMade = false;
    [HideInInspector] public string menuLevel1SelectedItem = "";    // 0: North, 1: East, 2: South, 3: West
    [HideInInspector] public string menuLevel2SelectedItem = "";    // 0: North, 1: East, 2: South, 3: West
    [HideInInspector] public string menuLevel3SelectedItem = "";    // 0: North, 1: East, 2: South, 3: West
    [HideInInspector] public GameObject currentlyGazedAnchor;
    [HideInInspector] public int currentlyGazedAnchor_dir;  // 0: North, 1: East, 2: South, 3: West
    [HideInInspector] public GameObject selectedGameObject;
    GameObject gazedGameObject;

    /* For computing gaze ray hit  */
    Ray combinedGazeRay;
    RaycastHit hit_General;
    int LM_General;

    /* For menu invoking feedback (Gauge) */
    [HideInInspector] public bool dwellDone = false;
    float dwellBeginTime_AnyObject = -1.0f;
    float dwellThreshold_AnyObject = 0.8f;

    /* For delayed menu closing */
    bool menuClosing = false;
    float menuClosingDelay = 0.2f;  // close the menu 0.2s after the final menu selection made (For consistant visual feedback: "blue-colored" anchor when selected).  
    float menuClosingTimer = 0.0f;
    Color cactusColor = new Color(0.090f, 0.311f, 0.010f);
    Color thornColor = new Color(0.800f, 0.727f, 0.002f);

    Demo_MenuControl _menuControl;
    void Start()
    {
        _menuControl = this.GetComponent<Demo_MenuControl>();
        menuGauge.fillAmount = 0.0f;
        LM_General = 1 << LayerMask.NameToLayer("General");
    }

    void Update()
    {
        /* Compute combined gaze ray */
        var rays = FoveInterface.GetGazeRays().value;
        combinedGazeRay = new Ray((rays.left.origin + rays.right.origin) / 2.0f, ((rays.left.GetPoint(10.0f) + rays.right.GetPoint(10.0f)) / 2.0f - (rays.left.origin + rays.right.origin) / 2.0f));
        eyeCursorTransform.position = combinedGazeRay.GetPoint(7.0f);
        Physics.Raycast(combinedGazeRay, out hit_General, Mathf.Infinity, LM_General);        

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

        /* Start button - Dwell for certain amount of time to start the task trial */
        if (!dwellDone && !_menuControl.latticeMenuOpened)
        {
            if (hit_General.point != Vector3.zero)
            {
                bool objectGazed = false;
                string[] objects = { "Cactus2", "UnactiveLamp", "Door" };
                for (int i = 0; i < objects.Length; i++)
                {
                    if (hit_General.collider.gameObject.name == objects[i])
                    {
                        objectGazed = true;
                        gazedGameObject = hit_General.collider.gameObject;
                        break;
                    }
                }
                if (objectGazed)
                {
                    if (dwellBeginTime_AnyObject == -1.0f)
                    {
                        dwellBeginTime_AnyObject = Time.time;
                        menuGauge.transform.position = eyeCursorTransform.position;
                        if (gazedGameObject.name == "Cactus2")
                        {
                            gazedGameObject.GetComponent<Renderer>().materials[0].color = new Color(0.243f, 0.725f, 0.273f);
                            gazedGameObject.GetComponent<Renderer>().materials[1].color = new Color(0.243f, 0.725f, 0.273f);
                        }
                        else if (gazedGameObject.name == "UnactiveLamp" || gazedGameObject.name == "Door")
                            gazedGameObject.GetComponent<Renderer>().material.color = new Color(0.243f, 0.725f, 0.273f);
                    }
                    else if (Time.time - dwellBeginTime_AnyObject > dwellThreshold_AnyObject)
                    {
                        dwellDone = true;
                        selectedGameObject = gazedGameObject;
                        if (gazedGameObject.name == "Cactus2")
                        {
                            gazedGameObject.GetComponent<Renderer>().materials[1].color = cactusColor;
                            gazedGameObject.GetComponent<Renderer>().materials[0].color = thornColor;
                        }
                        else if (gazedGameObject.name == "UnactiveLamp" || gazedGameObject.name == "Door")
                            gazedGameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
                        dwellBeginTime_AnyObject = -1.0f;
                        menuGauge.fillAmount = 0.0f;
                    }
                    else
                        menuGauge.fillAmount = (Time.time - dwellBeginTime_AnyObject) / dwellThreshold_AnyObject;
                }
            }
            else
            {
                dwellBeginTime_AnyObject = -1.0f;
                menuGauge.fillAmount = 0.0f;
                if (gazedGameObject)
                {
                    if (gazedGameObject.name == "Cactus2")
                    {
                        gazedGameObject.GetComponent<Renderer>().materials[1].color = cactusColor;
                        gazedGameObject.GetComponent<Renderer>().materials[0].color = thornColor;
                    }
                    else if (gazedGameObject.name == "UnactiveLamp" || gazedGameObject.name == "Door")
                        gazedGameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
                }
            }
        }

        /* Task managing */
        if (_menuControl.latticeMenuOpened)
        {
            // When users' eye gaze entered ItemSelectionZone
            if (hit_General.point != Vector3.zero && hit_General.collider.gameObject.name == "ItemSelectionZone")   
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

                if (currentlyGazedAnchor_dir != -1)
                {                   
                    if (_menuControl.currentMenuLevel == 1)
                        menuLevel1SelectedItem = currentlyGazedAnchor_dir.ToString();
                    else if (_menuControl.currentMenuLevel == 2)
                        menuLevel2SelectedItem = currentlyGazedAnchor_dir.ToString();
                    else if (_menuControl.currentMenuLevel == 3)
                    {
                        menuLevel3SelectedItem = currentlyGazedAnchor_dir.ToString();
                        if (!menuClosing)
                            menuClosing = true;
                    }

                    // Progressive Lattice Menu - showing four visual anchors around the currently selected item
                    for (int i = 0; i < 4; i++)
                        Eval_HelperMethods.GetFourSurroundingAnchors(ref _menuControl.latticeVisualAnchor, menuLevel1SelectedItem + menuLevel2SelectedItem)[i].transform.GetChild(0).GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0xFF, 0xFF, 0xFF, 1f));

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
