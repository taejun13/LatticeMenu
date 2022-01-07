using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Eval_MenuControl : MonoBehaviour
{
    [SerializeField]
    GameObject latticeMenu;
    [HideInInspector] public GameObject latticeVisualAnchor;
    [HideInInspector] public int currentMenuLevel = -1;   // 1st, 2nd, 3rd-level menu
    GameObject menuLevel1, menuLevel2, menuLevel3;

    /* For Menu Control */
    [HideInInspector] public GameObject currentlyShowingMenu;
    [HideInInspector] public bool taskStarted = false;

    /* For detailed menu UI arrangement */
    string[] alphabets = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                           "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    string[] level1Items = { "", "", "", "", "", "" };
    string[] level2Items = { "", "", "", "", "", "" };
    string[] level3Items = { "", "", "", "", "", "" };
    [HideInInspector] public float labelToAnchorDistance = 3;
    [HideInInspector] public float visualAnchorDiameter = 1.5f;
    [HideInInspector] public float itemSelectionZoneDiameter = 4.0f;
    /* Experiment # */
    [HideInInspector] public int trial = 1;
    [HideInInspector] public int trialEnd = 10;
    [HideInInspector] public int reps = 1;
    [HideInInspector] public int repsEnd = 4;

    bool newTaskPathRequired = true;
    bool taskFinished = false;

    /* For logging the task trials */
    [HideInInspector] public string userAnswerPath = "";        //ex. 013: north - east - west
    string groundTruth = "";      // ex. CEG: C - E - G
    string groundTruthPath = "";      // ex. 123: east - south - west
    string userAnswer = "";        //ex. CEG: C - E - G
    long startStamp;
    [HideInInspector] public long endStamp;
    long closeStamp;

    /* For making randomized target task paths */
    string[] bent0Paths_4x4x4 = { "000", "111", "222", "333" };
    string[] bent1Paths_4x4x4 = { "001", "002", "003", "110", "112", "113", "220", "221", "223", "330", "331", "332",
                                    "011", "022", "033", "100", "122", "133", "200", "211", "233", "300", "311", "322" };
    string[] bent2Paths_4x4x4 = { "010", "012", "013", "020", "021", "023", "030", "031", "032", 
                                    "101", "102", "103", "120", "121", "123", "130", "131", "132", 
                                    "201", "202", "203", "210", "212", "213", "230", "231", "232", 
                                    "301", "302", "303", "310", "312", "313", "320", "321", "323" };
    string[] bent0Paths_6x6x6 = { "000", "111", "222", "333", "444", "555" };
    string[] bent1Paths_6x6x6 = { "001", "002", "003", "004", "005", "011", "022", "033", "044", "055",
                                    "110", "112", "113", "114", "115", "100", "122", "133", "144", "155",
                                    "220", "221", "223", "224", "225", "200", "211", "233", "244", "255",
                                    "330", "331", "332", "334", "335", "300", "311", "322", "344", "355",
                                    "440", "441", "442", "443", "445", "400", "411", "422", "433", "455",
                                    "550", "551", "552", "553", "554", "500", "511", "522", "533", "544" };
    string[] bent2Paths_6x6x6 = { "010", "012", "013", "014", "015", "020", "021", "023", "024", "025", "030", "031", "032", "034", "035", "040", "041", "042", "043", "045", "050", "051", "052", "053", "054",
                                    "101", "102", "103", "104", "105", "120", "121", "123", "124", "125", "130", "131", "132", "134", "135", "140", "141", "142", "143", "145", "150", "151", "152", "153", "154",
                                    "201", "202", "203", "204", "205", "210", "212", "213", "214", "215", "230", "231", "232", "234", "235", "240", "241", "242", "243", "245", "250", "251", "252", "253", "254",
                                    "301", "302", "303", "304", "305", "310", "312", "313", "314", "315", "320", "321", "323", "324", "325", "340", "341", "342", "343", "345", "350", "351", "352", "353", "354", 
                                    "401", "402", "403", "404", "405", "410", "412", "413", "414", "415", "420", "421", "423", "424", "425", "430", "431", "432", "434", "435", "450", "451", "452", "453", "454",
                                    "501", "502", "503", "504", "505", "510", "512", "513", "514", "515", "520", "521", "523", "524", "525", "530", "531", "532", "534", "535", "540", "541", "542", "543", "545" };
    string[] chosenTaskPaths_4x4x4 = { "", "", "", "", "", "", "", "", "", "" };
    string[] chosenTaskPaths_6x6x6 = { "", "", "", "", "", "", "", "", "", "" };


    /* For scoring */
    float[,] corrects = new float[10, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
    long[,] taskTimes = new long[10, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };

    Eval_GazeDetection _gazeDetection;
    Eval_Manager _manager;
    void Start()
    {
        _gazeDetection = this.GetComponent<Eval_GazeDetection>();
        _manager = this.GetComponent<Eval_Manager>();

        // Initialize menu structure
        if (_manager._menuStructure == Eval_Manager.MenuStructure.S_4x4x4)
        {
            menuLevel1 = latticeMenu.transform.Find("4x4x4/MenuLevel1").gameObject;
            menuLevel2 = latticeMenu.transform.Find("4x4x4/MenuLevel2").gameObject;
            menuLevel3 = latticeMenu.transform.Find("4x4x4/MenuLevel3").gameObject;
            latticeVisualAnchor = latticeMenu.transform.Find("4x4x4/LatticeVisualAnchor").gameObject;
        }
        else if (_manager._menuStructure == Eval_Manager.MenuStructure.S_6x6x6)
        {
            menuLevel1 = latticeMenu.transform.Find("6x6x6/MenuLevel1").gameObject;
            menuLevel2 = latticeMenu.transform.Find("6x6x6/MenuLevel2").gameObject;
            menuLevel3 = latticeMenu.transform.Find("6x6x6/MenuLevel3").gameObject;
            latticeVisualAnchor = latticeMenu.transform.Find("6x6x6/LatticeVisualAnchor").gameObject;
        }

        /* Initialize layout of Lattice Menu */
        Eval_HelperMethods.menuRadiusInit(ref menuLevel1, ref menuLevel2, ref menuLevel3, _manager._menuStructure, _manager._menuSize);
        Eval_HelperMethods.menuItemLabelDistanceInit(ref menuLevel1, ref menuLevel2, ref menuLevel3, _manager._menuStructure, _manager._menuSize, labelToAnchorDistance);
        Eval_HelperMethods.visualAnchorPositionInit(ref latticeVisualAnchor, _manager._menuStructure, _manager._menuSize);
        Eval_HelperMethods.visualAnchorSizeInit(ref latticeVisualAnchor, visualAnchorDiameter);
        Eval_HelperMethods.itemSelectionZoneSizeInit(ref latticeVisualAnchor, itemSelectionZoneDiameter);
        Eval_HelperMethods.progressiveUnfoldingEffectApply(ref latticeVisualAnchor, _manager._progressiveUnfoldingEffect);

        /* Making target task paths */
        System.Random rnd = new System.Random();
        for (int i = 0; i < 2; i++)
        {
            alphabets = alphabets.OrderBy(x => rnd.Next()).ToArray();
            bent0Paths_4x4x4 = bent0Paths_4x4x4.OrderBy(x => rnd.Next()).ToArray();
            bent1Paths_4x4x4 = bent1Paths_4x4x4.OrderBy(x => rnd.Next()).ToArray();
            bent2Paths_4x4x4 = bent2Paths_4x4x4.OrderBy(x => rnd.Next()).ToArray();
            bent0Paths_6x6x6 = bent0Paths_6x6x6.OrderBy(x => rnd.Next()).ToArray();
            bent1Paths_6x6x6 = bent1Paths_6x6x6.OrderBy(x => rnd.Next()).ToArray();
            bent2Paths_6x6x6 = bent2Paths_6x6x6.OrderBy(x => rnd.Next()).ToArray();
        }
        Array.Copy(bent0Paths_4x4x4, 0, chosenTaskPaths_4x4x4, 0, 1);
        Array.Copy(bent1Paths_4x4x4, 0, chosenTaskPaths_4x4x4, 1, 3);
        Array.Copy(bent2Paths_4x4x4, 0, chosenTaskPaths_4x4x4, 4, 6);
        Array.Copy(bent0Paths_6x6x6, 0, chosenTaskPaths_6x6x6, 0, 1);
        Array.Copy(bent1Paths_6x6x6, 0, chosenTaskPaths_6x6x6, 1, 3);
        Array.Copy(bent2Paths_6x6x6, 0, chosenTaskPaths_6x6x6, 4, 6);

        /* Hide Lattice Menu before pressing enter */
        latticeMenu.SetActive(false);
    }

    void Update()
    {
        /* Get new target task paths after space bar pressed */
        if (!taskFinished)
        {
            if (newTaskPathRequired && Input.GetKeyUp(KeyCode.Space))
            {
                taskTrialMaking();
                _gazeDetection.startButton.SetActive(true);
                _gazeDetection.taskPanel.SetActive(true);
                _gazeDetection.taskPanel.transform.GetChild(1).GetComponent<TextMesh>().text = "(#" + reps + ") " + groundTruth.Substring(0, 1) + " - " + groundTruth.Substring(1, 1) + " - " + groundTruth.Substring(2, 1);
                newTaskPathRequired = false;
            }
        }

        /* Start button selected - initialize & invoke Lattice Menu */
        if (_gazeDetection.startButtonSelected)
        {
            _gazeDetection.startButtonSelected = false;
            latticeMenu.SetActive(true);
            menuLevel1.SetActive(true);
            menuLevel2.SetActive(false);
            menuLevel3.SetActive(false);
            _gazeDetection.menuSelectionMade = false;
            currentlyShowingMenu = menuLevel1;
            currentMenuLevel = 1;
            latticeMenu.transform.position = _gazeDetection.startButton.transform.position;
            if (_manager._progressiveUnfoldingEffect == Eval_Manager.ProgressiveUnfoldingEffect.ProgressiveLatticeMenu)
            {
                if (_manager._menuStructure == Eval_Manager.MenuStructure.S_4x4x4)
                {
                    for (int i = 0; i < 4; i++)
                        Eval_HelperMethods.GetFourSurroundingAnchors(ref latticeVisualAnchor, "")[i].transform.GetChild(0).GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0xFF, 0xFF, 0xFF, 1f));
                }
                else if (_manager._menuStructure == Eval_Manager.MenuStructure.S_6x6x6)
                {
                    for (int i = 0; i < 6; i++)
                        Eval_HelperMethods.GetSixSurroundingAnchors(ref latticeVisualAnchor, "")[i].transform.GetChild(0).GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0xFF, 0xFF, 0xFF, 1f));
                }
            }

            _gazeDetection.startButton.SetActive(false);
            _gazeDetection.taskPanel.SetActive(false);
            _gazeDetection.answerFeedbackPanel.SetActive(false);
            taskStarted = true;
            startStamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
        }

        /* Task Trial Management */
        if (taskStarted)
        {
            if (currentMenuLevel == 1 && _gazeDetection.menuLevel1SelectedItem != "")     // When first-level menu item is picked
            {
                if (_manager._menuStructure == Eval_Manager.MenuStructure.S_4x4x4)
                    menuLevel2.transform.position = Eval_HelperMethods.GetFourSurroundingAnchors(ref latticeVisualAnchor, "")[Convert.ToInt32(_gazeDetection.menuLevel1SelectedItem)].transform.position;
                else if (_manager._menuStructure == Eval_Manager.MenuStructure.S_6x6x6)
                    menuLevel2.transform.position = Eval_HelperMethods.GetSixSurroundingAnchors(ref latticeVisualAnchor, "")[Convert.ToInt32(_gazeDetection.menuLevel1SelectedItem)].transform.position;
                menuLevel2.SetActive(true);
                menuLevel1.SetActive(false);
                currentlyShowingMenu = menuLevel2;
                currentMenuLevel = 2;
            }
            else if (currentMenuLevel == 2 && _gazeDetection.menuLevel2SelectedItem != "")    // When second-level menu item is picked
            {
                if (_manager._menuStructure == Eval_Manager.MenuStructure.S_4x4x4)
                    menuLevel3.transform.position = Eval_HelperMethods.GetFourSurroundingAnchors(ref latticeVisualAnchor, _gazeDetection.menuLevel1SelectedItem)[Convert.ToInt32(_gazeDetection.menuLevel2SelectedItem)].transform.position;
                else if (_manager._menuStructure == Eval_Manager.MenuStructure.S_6x6x6)
                    menuLevel3.transform.position = Eval_HelperMethods.GetSixSurroundingAnchors(ref latticeVisualAnchor, _gazeDetection.menuLevel1SelectedItem)[Convert.ToInt32(_gazeDetection.menuLevel2SelectedItem)].transform.position;
                menuLevel3.SetActive(true);
                menuLevel2.SetActive(false);
                menuLevel1.SetActive(false);
                currentlyShowingMenu = menuLevel3;
                currentMenuLevel = 3;
            }
            else if (_gazeDetection.menuSelectionMade)
            {
                _gazeDetection.menuSelectionMade = false;
                closeStamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                userAnswerPath = _gazeDetection.menuLevel1SelectedItem + _gazeDetection.menuLevel2SelectedItem + _gazeDetection.menuLevel3SelectedItem;
                userAnswer = userAnswerNumToAlphabet(userAnswerPath);

                // Visual Feedback for the answer (i.e., Correct or Wrong)
                int correct;
                if (groundTruth == userAnswer)
                {
                    correct = 1;
                    _gazeDetection.answerFeedbackPanel.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(0x9A, 0xFF, 0x92, 0xFF);
                    _gazeDetection.answerFeedbackPanel.transform.GetChild(1).GetComponent<TextMesh>().text = "Correct";
                }
                else
                {
                    correct = 0;
                    _gazeDetection.answerFeedbackPanel.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(0xFF, 0xB0, 0xA7, 0xFF);
                    _gazeDetection.answerFeedbackPanel.transform.GetChild(1).GetComponent<TextMesh>().text = "Wrong";
                }

                corrects[trial-1, reps-1] = correct;
                taskTimes[trial-1, reps-1] = closeStamp - startStamp;

                // Counting Reps & Trials
                if (reps == repsEnd)
                {
                    if (trial == trialEnd)
                    {
                        float noviceMenuSelectionErrorRate = 0;
                        float noviceMenuSelectionTime = 0;
                        float expertMenuSelectionErrorRate = 0;
                        float expertMenuSelectionTime = 0;
                        for (int i = 0; i < trialEnd; i++)
                        {
                            for (int j = 0; j < repsEnd; j++)
                            {
                                if (j == 0)
                                {
                                    noviceMenuSelectionErrorRate = noviceMenuSelectionErrorRate + corrects[i, j];
                                    noviceMenuSelectionTime = noviceMenuSelectionTime + taskTimes[i, j];
                                }
                                else if (j == 2 || j == 3)
                                {
                                    expertMenuSelectionErrorRate = expertMenuSelectionErrorRate + corrects[i, j];
                                    expertMenuSelectionTime = expertMenuSelectionTime + taskTimes[i, j];
                                }
                            }
                        }

                        noviceMenuSelectionErrorRate = (10 - noviceMenuSelectionErrorRate) * 100 / 10.0f;
                        noviceMenuSelectionTime = noviceMenuSelectionTime / 10.0f;
                        noviceMenuSelectionTime = noviceMenuSelectionTime / 1000.0f;
                        expertMenuSelectionErrorRate = (20 - expertMenuSelectionErrorRate) * 100 / 20.0f;
                        expertMenuSelectionTime = expertMenuSelectionTime / 20.0f;
                        expertMenuSelectionTime = expertMenuSelectionTime / 1000.0f;

                        string resultStr = "*** Evaluation Task Done *** \n\n";
                        resultStr = resultStr + "Novice Trials (#1)\n";
                        resultStr = resultStr + "- ErrorRate: " + noviceMenuSelectionErrorRate +  "%\n";
                        resultStr = resultStr + "- Selection Time: " + noviceMenuSelectionTime + "s\n\n";
                        resultStr = resultStr + "Expert Trials (#3 & #4)\n";
                        resultStr = resultStr + "- ErrorRate: " + expertMenuSelectionErrorRate + "%\n";
                        resultStr = resultStr + "- Selection Time: " + expertMenuSelectionTime + "s\n";

                        _gazeDetection.answerFeedbackPanel.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color32(0xFF, 0xB0, 0xA7, 0xFF);
                        _gazeDetection.startButton.SetActive(false);
                        _gazeDetection.answerFeedbackPanel.SetActive(false);
                        _gazeDetection.taskPanel.SetActive(true);
                        _gazeDetection.taskPanel.transform.GetChild(0).gameObject.SetActive(false);
                        _gazeDetection.taskPanel.transform.GetChild(1).GetComponent<TextMesh>().text = resultStr;
                        taskFinished = true;
                    }
                    else
                    {
                        _gazeDetection.answerFeedbackPanel.SetActive(true);
                        _gazeDetection.startButton.SetActive(false);
                        _gazeDetection.taskPanel.SetActive(true);
                        _gazeDetection.taskPanel.transform.GetChild(1).GetComponent<TextMesh>().text = "Press space bar to get a new target item";
                        reps = 1;
                        trial++;
                        newTaskPathRequired = true;
                    }
                }
                else
                {
                    reps++;
                    _gazeDetection.answerFeedbackPanel.SetActive(true);
                    _gazeDetection.startButton.SetActive(true);
                    _gazeDetection.taskPanel.SetActive(true);
                    _gazeDetection.taskPanel.transform.GetChild(1).GetComponent<TextMesh>().text = "(#" + reps + ") " + groundTruth.Substring(0, 1) + " - " + groundTruth.Substring(1, 1) + " - " + groundTruth.Substring(2, 1);
                }

                // Initialize for next trial
                if (_manager._progressiveUnfoldingEffect == Eval_Manager.ProgressiveUnfoldingEffect.ProgressiveLatticeMenu)
                {
                    for (int i = 0; i < latticeVisualAnchor.transform.childCount; i++)
                        latticeVisualAnchor.transform.GetChild(i).GetChild(0).GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0xFF, 0xFF, 0xFF, 0x00));
                }
                else if (_manager._progressiveUnfoldingEffect == Eval_Manager.ProgressiveUnfoldingEffect.FullLatticeMenu)
                {
                    for (int i = 0; i < latticeVisualAnchor.transform.childCount; i++)
                        latticeVisualAnchor.transform.GetChild(i).GetChild(0).GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0xFF, 0xFF, 0xFF, 0xFF));
                }
                taskStarted = false;
                userAnswer = "";
                userAnswerPath = "";
                latticeMenu.SetActive(false);
                _gazeDetection.currentlyGazedAnchor_dir = -1;
                _gazeDetection.menuLevel1SelectedItem = "";
                _gazeDetection.menuLevel2SelectedItem = "";
                _gazeDetection.menuLevel3SelectedItem = "";
                currentMenuLevel = -1;
            }
        }
    }

    string userAnswerNumToAlphabet(String userAnswer)
    {
        string retVal = "";
        int i, j;
        string[][] levelXItems = { level1Items, level2Items, level3Items };
        char[] numStr = { '0', '1', '2', '3', '4', '5' };
        for (i = 0; i < 3; i++)
        {
            if (_manager._menuStructure == Eval_Manager.MenuStructure.S_4x4x4)
            {
                for (j = 0; j < 4; j++)
                {
                    if (userAnswer[i] == numStr[j])
                    {
                        retVal = retVal + levelXItems[i][j];
                        break;
                    }
                }
            }
            else if (_manager._menuStructure == Eval_Manager.MenuStructure.S_6x6x6)
            {
                for (j = 0; j < 6; j++)
                {
                    if (userAnswer[i] == numStr[j])
                    {
                        retVal = retVal + levelXItems[i][j];
                        break;
                    }
                }
            }
        }

        return retVal;
    }

    void taskTrialMaking()
    {
        string[] orientationFourStr = { "North", "East", "South", "West" };
        string[] orientationSixStr = { "1", "2", "3", "4", "5", "6" };

        System.Random rnd = new System.Random();
        alphabets = alphabets.OrderBy(x => rnd.Next()).ToArray();
        if (_manager._menuStructure == Eval_Manager.MenuStructure.S_4x4x4)
        {
            Array.Copy(alphabets, 0, level1Items, 0, 4);
            Array.Copy(alphabets, 4, level2Items, 0, 4);
            Array.Copy(alphabets, 8, level3Items, 0, 4);

            for (int i = 0; i < orientationFourStr.Length; i++)
            {
                TextMesh menuLevel1_t = menuLevel1.transform.Find("Item" + orientationFourStr[i] + "/ItemText").GetComponent<TextMesh>();
                TextMesh menuLevel2_t = menuLevel2.transform.Find("Item" + orientationFourStr[i] + "/ItemText").GetComponent<TextMesh>();
                TextMesh menuLevel3_t = menuLevel3.transform.Find("Item" + orientationFourStr[i] + "/ItemText").GetComponent<TextMesh>();

                menuLevel1_t.text = level1Items[i];
                menuLevel2_t.text = level2Items[i];
                menuLevel3_t.text = level3Items[i];
            }

            groundTruth = level1Items[Convert.ToInt32(chosenTaskPaths_4x4x4[trial - 1].Substring(0, 1))] + level2Items[Convert.ToInt32(chosenTaskPaths_4x4x4[trial - 1].Substring(1, 1))] + level3Items[Convert.ToInt32(chosenTaskPaths_4x4x4[trial - 1].Substring(2, 1))];
            groundTruthPath = chosenTaskPaths_4x4x4[trial - 1];
        }
        else if (_manager._menuStructure == Eval_Manager.MenuStructure.S_6x6x6)

        {
            Array.Copy(alphabets, 0, level1Items, 0, 6);
            Array.Copy(alphabets, 6, level2Items, 0, 6);
            Array.Copy(alphabets, 12, level3Items, 0, 6);
            for (int i = 0; i < orientationSixStr.Length; i++)
            {
                TextMesh menuLevel1_t = menuLevel1.transform.Find("Item" + orientationSixStr[i] + "/ItemText").GetComponent<TextMesh>();
                TextMesh menuLevel2_t = menuLevel2.transform.Find("Item" + orientationSixStr[i] + "/ItemText").GetComponent<TextMesh>();
                TextMesh menuLevel3_t = menuLevel3.transform.Find("Item" + orientationSixStr[i] + "/ItemText").GetComponent<TextMesh>();

                menuLevel1_t.text = level1Items[i];
                menuLevel2_t.text = level2Items[i];
                menuLevel3_t.text = level3Items[i];
            }

            groundTruth = level1Items[Convert.ToInt32(chosenTaskPaths_6x6x6[trial - 1].Substring(0, 1))] + level2Items[Convert.ToInt32(chosenTaskPaths_6x6x6[trial - 1].Substring(1, 1))] + level3Items[Convert.ToInt32(chosenTaskPaths_6x6x6[trial - 1].Substring(2, 1))];
            groundTruthPath = chosenTaskPaths_6x6x6[trial - 1];
        }
    }
}
