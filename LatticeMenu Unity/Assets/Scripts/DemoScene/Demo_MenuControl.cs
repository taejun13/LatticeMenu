using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Demo_MenuControl : MonoBehaviour
{
    [SerializeField]
    GameObject latticeMenu;
    [HideInInspector] public GameObject latticeVisualAnchor;
    [HideInInspector] public int currentMenuLevel = -1;   // 1st, 2nd, 3rd-level menu
    GameObject menuLevel1, menuLevel2, menuLevel3;
    [SerializeField]
    GameObject Lamp, Cactus, Door;
    /* For Menu Control */
    [HideInInspector] public GameObject currentlyShowingMenu;
    [HideInInspector] public bool latticeMenuOpened = false;
    bool doorOpenStarted = false;
    bool lampOnStarted = false;
    bool cactusGrowingStarted = false;

    /* For detailed menu UI arrangement */
    [HideInInspector] public float labelToAnchorDistance = 3;
    [HideInInspector] public float visualAnchorDiameter = 1.5f;
    [HideInInspector] public float itemSelectionZoneDiameter = 4.0f;

    string userAnswerPath;

    public enum MenuStructure
    { 
        S_4x4x4 = 4,
        S_6x6x6 = 6,
    }
    public enum MenuSize
    { 
        Deg_8 = 8,
        Deg_10 = 10,
        Deg_12 = 12,
    }
    public enum ProgressiveUnfoldingEffect
    { 
        ProgressiveLatticeMenu,
        FullLatticeMenu
    }
    [HideInInspector] public MenuStructure _menuStructure = MenuStructure.S_4x4x4;
    [HideInInspector] public MenuSize _menuSize = MenuSize.Deg_10;
    [HideInInspector] public ProgressiveUnfoldingEffect _progressiveUnfoldingEffect = ProgressiveUnfoldingEffect.ProgressiveLatticeMenu;

    Demo_GazeDetection _gazeDetection;
    void Start()
    {
        _gazeDetection = this.GetComponent<Demo_GazeDetection>();

        // Initialize menu structure
        menuLevel1 = latticeMenu.transform.Find("4x4x4/MenuLevel1").gameObject;
        menuLevel2 = latticeMenu.transform.Find("4x4x4/MenuLevel2").gameObject;
        menuLevel3 = latticeMenu.transform.Find("4x4x4/MenuLevel3").gameObject;
        latticeVisualAnchor = latticeMenu.transform.Find("4x4x4/LatticeVisualAnchor").gameObject;

        /* Initialize layout of Lattice Menu */
        Demo_HelperMethods.menuRadiusInit(ref menuLevel1, ref menuLevel2, ref menuLevel3, _menuStructure, _menuSize);
        Demo_HelperMethods.menuItemLabelDistanceInit(ref menuLevel1, ref menuLevel2, ref menuLevel3, _menuStructure, _menuSize, labelToAnchorDistance);
        Demo_HelperMethods.visualAnchorPositionInit(ref latticeVisualAnchor, _menuStructure, _menuSize);
        Demo_HelperMethods.visualAnchorSizeInit(ref latticeVisualAnchor, visualAnchorDiameter);
        Demo_HelperMethods.itemSelectionZoneSizeInit(ref latticeVisualAnchor, itemSelectionZoneDiameter);
        Demo_HelperMethods.progressiveUnfoldingEffectApply(ref latticeVisualAnchor, _progressiveUnfoldingEffect);

        /* Hide Lattice Menu before invoked */
        latticeMenu.SetActive(false);
    }

    void Update()
    {
        if (lampOnStarted)
        {
            if (Lamp.transform.GetChild(0).GetComponent<Light>().intensity <= 2.0f)
                Lamp.transform.GetChild(0).GetComponent<Light>().intensity = Lamp.transform.GetChild(0).GetComponent<Light>().intensity + 0.05f;
            else
                lampOnStarted = false;
        }
        if (doorOpenStarted)
        {
            Door.transform.localPosition = Door.transform.localPosition + new Vector3(0.04f, 0f, 0f);
        }
        if (cactusGrowingStarted)
        {
            if (Cactus.transform.localScale.x < 1)
                Cactus.transform.localScale = Cactus.transform.localScale + new Vector3(0.02f, 0.02f, 0.02f);
            else
                cactusGrowingStarted = false;
        }

        /* Start button selected - initialize & invoke Lattice Menu */
        if (_gazeDetection.dwellDone)
        {
            _gazeDetection.dwellDone = false;

            latticeMenu.SetActive(true);
            menuLevel1.SetActive(true);
            menuLevel2.SetActive(false);
            menuLevel3.SetActive(false);
            _gazeDetection.menuSelectionMade = false;
            currentlyShowingMenu = menuLevel1;
            currentMenuLevel = 1;
            latticeMenu.transform.position = _gazeDetection.eyeCursorTransform.position + new Vector3(0f, 0f, -2f);
            for (int i = 0; i < 4; i++)
                Demo_HelperMethods.GetFourSurroundingAnchors(ref latticeVisualAnchor, "")[i].transform.GetChild(0).GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0xFF, 0xFF, 0xFF, 1f));
            latticeMenuOpened = true;
        }

        /* Task Trial Management */
        if (latticeMenuOpened)
        {
            if (currentMenuLevel == 1 && _gazeDetection.menuLevel1SelectedItem != "")     // When first-level menu item is picked
            {
                menuLevel2.transform.position = Demo_HelperMethods.GetFourSurroundingAnchors(ref latticeVisualAnchor, "")[Convert.ToInt32(_gazeDetection.menuLevel1SelectedItem)].transform.position;
                menuLevel2.SetActive(true);
                menuLevel1.SetActive(false);
                currentlyShowingMenu = menuLevel2;
                currentMenuLevel = 2;
            }
            else if (currentMenuLevel == 2 && _gazeDetection.menuLevel2SelectedItem != "")    // When second-level menu item is picked
            {
                menuLevel3.transform.position = Demo_HelperMethods.GetFourSurroundingAnchors(ref latticeVisualAnchor, _gazeDetection.menuLevel1SelectedItem)[Convert.ToInt32(_gazeDetection.menuLevel2SelectedItem)].transform.position;
                menuLevel2.SetActive(false);
                menuLevel1.SetActive(false);
                currentlyShowingMenu = menuLevel3;
                currentMenuLevel = 3;

                if (_gazeDetection.menuLevel1SelectedItem == "1" && _gazeDetection.menuLevel2SelectedItem == "3")
                {
                    latticeMenu.transform.Find("4x4x4/MenuLevel3/ItemNorth/ItemText").GetComponent<TextMesh>().text = "SizeUp";
                    latticeMenu.transform.Find("4x4x4/MenuLevel3/ItemEast/ItemText").GetComponent<TextMesh>().text = "-";
                    latticeMenu.transform.Find("4x4x4/MenuLevel3/ItemSouth/ItemText").GetComponent<TextMesh>().text = "SizeDown";
                    latticeMenu.transform.Find("4x4x4/MenuLevel3/ItemWest/ItemText").GetComponent<TextMesh>().text = "-";
                }
                else if (_gazeDetection.menuLevel1SelectedItem == "1" && _gazeDetection.menuLevel2SelectedItem == "1")
                {
                    latticeMenu.transform.Find("4x4x4/MenuLevel3/ItemNorth/ItemText").GetComponent<TextMesh>().text = "On";
                    latticeMenu.transform.Find("4x4x4/MenuLevel3/ItemEast/ItemText").GetComponent<TextMesh>().text = "-";
                    latticeMenu.transform.Find("4x4x4/MenuLevel3/ItemSouth/ItemText").GetComponent<TextMesh>().text = "Off";
                    latticeMenu.transform.Find("4x4x4/MenuLevel3/ItemWest/ItemText").GetComponent<TextMesh>().text = "-";
                }
                else if (_gazeDetection.menuLevel1SelectedItem == "1" && _gazeDetection.menuLevel2SelectedItem == "0")
                {
                    latticeMenu.transform.Find("4x4x4/MenuLevel3/ItemNorth/ItemText").GetComponent<TextMesh>().text = "-";
                    latticeMenu.transform.Find("4x4x4/MenuLevel3/ItemEast/ItemText").GetComponent<TextMesh>().text = "Right";
                    latticeMenu.transform.Find("4x4x4/MenuLevel3/ItemSouth/ItemText").GetComponent<TextMesh>().text = "-";
                    latticeMenu.transform.Find("4x4x4/MenuLevel3/ItemWest/ItemText").GetComponent<TextMesh>().text = "Left";
                }

                if (_gazeDetection.menuLevel1SelectedItem == "1" && _gazeDetection.menuLevel2SelectedItem == "2")
                {
                    Cactus.SetActive(false);
                    menuLevel3.SetActive(false);

                    for (int i = 0; i < latticeVisualAnchor.transform.childCount; i++)
                        latticeVisualAnchor.transform.GetChild(i).GetChild(0).GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0xFF, 0xFF, 0xFF, 0x00));

                    userAnswerPath = "";
                    latticeMenu.SetActive(false);
                    _gazeDetection.currentlyGazedAnchor_dir = -1;
                    _gazeDetection.menuLevel1SelectedItem = "";
                    _gazeDetection.menuLevel2SelectedItem = "";
                    _gazeDetection.menuLevel3SelectedItem = "";
                    currentMenuLevel = -1;
                    latticeMenuOpened = false;
                }
                else
                    menuLevel3.SetActive(true);
            }
            else if (_gazeDetection.menuSelectionMade)
            {
                _gazeDetection.menuSelectionMade = false;
                userAnswerPath = _gazeDetection.menuLevel1SelectedItem + _gazeDetection.menuLevel2SelectedItem + _gazeDetection.menuLevel3SelectedItem;

                if (userAnswerPath == "110")
                {
                    Lamp.transform.GetChild(0).gameObject.SetActive(true);
                    Lamp.transform.GetChild(1).gameObject.SetActive(true);
                    Lamp.transform.GetChild(2).gameObject.SetActive(false);
                    lampOnStarted = true;
                }
                else if (userAnswerPath == "130")
                {
                    cactusGrowingStarted = true;
                    Cactus.GetComponent<MeshCollider>().enabled = false;
                }
                else if (userAnswerPath == "101")
                {
                    doorOpenStarted = true;
                    Door.GetComponent<MeshCollider>().enabled = false;
                }

                for (int i = 0; i < latticeVisualAnchor.transform.childCount; i++)
                    latticeVisualAnchor.transform.GetChild(i).GetChild(0).GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0xFF, 0xFF, 0xFF, 0x00));

                userAnswerPath = "";
                latticeMenu.SetActive(false);
                _gazeDetection.currentlyGazedAnchor_dir = -1;
                _gazeDetection.menuLevel1SelectedItem = "";
                _gazeDetection.menuLevel2SelectedItem = "";
                _gazeDetection.menuLevel3SelectedItem = "";
                currentMenuLevel = -1;
                latticeMenuOpened = false;
            }
        }
    }
}
