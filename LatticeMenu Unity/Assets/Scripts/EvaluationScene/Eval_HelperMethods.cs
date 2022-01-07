/*
 * Writer: Taejun Kim, HCI Lab KAIST - https://taejun13.github.io/
 * Last Update: 2022. 1. 7
 * Lattice Menu: A Low-Error Gaze-Based Marking Menu Utilizing Target-Assisted Gaze Gestures on a Lattice of Visual Anchors (ACM CHI 2022)
 * ACM CHI 22': Conference on Human Factors in Computing Systems.
 * DOI: (TBU)
 */

using UnityEngine;
using System;

public static class Eval_HelperMethods
{
    public static void menuRadiusInit(ref GameObject menuLevel1, ref GameObject menuLevel2, ref GameObject menuLevel3, Eval_Manager.MenuStructure _menuStructure, Eval_Manager.MenuSize menuSize)
    {
        float degreeToMeter = 0.174537f;
        string[] orientationFourStr = { "North", "East", "South", "West" };
        string[] orientationSixStr = { "1", "2", "3", "4", "5", "6" };
        if (_menuStructure == Eval_Manager.MenuStructure.S_4x4x4)
        {
            for (int i = 0; i < orientationFourStr.Length; i++)
            {
                menuLevel1.transform.Find("Item" + orientationFourStr[i] + "/ItemPanel").localScale = new Vector3(((int)menuSize - 2) * degreeToMeter, 1f, ((int)menuSize - 2) * degreeToMeter);
                menuLevel2.transform.Find("Item" + orientationFourStr[i] + "/ItemPanel").localScale = new Vector3(((int)menuSize - 2) * degreeToMeter, 1f, ((int)menuSize - 2) * degreeToMeter);
                menuLevel3.transform.Find("Item" + orientationFourStr[i] + "/ItemPanel").localScale = new Vector3(((int)menuSize - 2) * degreeToMeter, 1f, ((int)menuSize - 2) * degreeToMeter);
            }
        }
        else if (_menuStructure == Eval_Manager.MenuStructure.S_6x6x6)
        {
            for (int i = 0; i < orientationSixStr.Length; i++)
            {
                menuLevel1.transform.Find("Item" + orientationSixStr[i] + "/ItemPanel").localScale = new Vector3(((int)menuSize - 2) * degreeToMeter, 1f, ((int)menuSize - 2) * degreeToMeter);
                menuLevel2.transform.Find("Item" + orientationSixStr[i] + "/ItemPanel").localScale = new Vector3(((int)menuSize - 2) * degreeToMeter, 1f, ((int)menuSize - 2) * degreeToMeter);
                menuLevel3.transform.Find("Item" + orientationSixStr[i] + "/ItemPanel").localScale = new Vector3(((int)menuSize - 2) * degreeToMeter, 1f, ((int)menuSize - 2) * degreeToMeter);
            }
        }
    }

    public static void menuItemLabelDistanceInit(ref GameObject menuLevel1, ref GameObject menuLevel2, ref GameObject menuLevel3, Eval_Manager.MenuStructure _menuStructure, Eval_Manager.MenuSize menuSize, float labelToAnchorDistance)
    {
        float degreeToMeter = 0.174537f;
        float unitLabelDistance = (((int)menuSize - 2) - labelToAnchorDistance) * degreeToMeter;
        GameObject[] menus = { menuLevel1, menuLevel2, menuLevel3 };
        if (_menuStructure == Eval_Manager.MenuStructure.S_4x4x4)
        {
            for (int i = 0; i < menus.Length; i++)
            {
                menus[i].transform.Find("ItemNorth/ItemText").localPosition = new Vector3(0f, unitLabelDistance, -0.1f);
                menus[i].transform.Find("ItemEast/ItemText").localPosition = new Vector3(unitLabelDistance, 0f, -0.1f);
                menus[i].transform.Find("ItemSouth/ItemText").localPosition = new Vector3(0f, unitLabelDistance * -1, -0.1f);
                menus[i].transform.Find("ItemWest/ItemText").localPosition = new Vector3(unitLabelDistance * -1, 0f, -0.1f);
            }
        }
        else if (_menuStructure == Eval_Manager.MenuStructure.S_6x6x6)
        {
            for (int i = 0; i < menus.Length; i++)
            {
                menus[i].transform.Find("Item1/ItemText").gameObject.transform.localPosition = new Vector3(unitLabelDistance * (float)Math.Cos(Math.PI * 90 / 180.0), unitLabelDistance * (float)Math.Sin(Math.PI * 90 / 180.0), -0.1f);
                menus[i].transform.Find("Item2/ItemText").gameObject.transform.localPosition = new Vector3(unitLabelDistance * (float)Math.Cos(Math.PI * 30 / 180.0), unitLabelDistance * (float)Math.Sin(Math.PI * 30 / 180.0), -0.1f);
                menus[i].transform.Find("Item3/ItemText").gameObject.transform.localPosition = new Vector3(unitLabelDistance * (float)Math.Cos(Math.PI * -30 / 180.0), unitLabelDistance * (float)Math.Sin(Math.PI * -30 / 180.0), -0.1f);
                menus[i].transform.Find("Item4/ItemText").gameObject.transform.localPosition = new Vector3(unitLabelDistance * (float)Math.Cos(Math.PI * -90 / 180.0), unitLabelDistance * (float)Math.Sin(Math.PI * -90 / 180.0), -0.1f);
                menus[i].transform.Find("Item5/ItemText").gameObject.transform.localPosition = new Vector3(unitLabelDistance * (float)Math.Cos(Math.PI * -150 / 180.0), unitLabelDistance * (float)Math.Sin(Math.PI * -150 / 180.0), -0.1f);
                menus[i].transform.Find("Item6/ItemText").gameObject.transform.localPosition = new Vector3(unitLabelDistance * (float)Math.Cos(Math.PI * -210 / 180.0), unitLabelDistance * (float)Math.Sin(Math.PI * -210 / 180.0), -0.1f);
            }
        }
    }

    public static void visualAnchorPositionInit(ref GameObject latticeVisualAnchor, Eval_Manager.MenuStructure _menuStructure, Eval_Manager.MenuSize menuSize)
    {
        float degreeToMeter = 0.174537f;
        float unitDistance = (int)menuSize * degreeToMeter;
        if (_menuStructure == Eval_Manager.MenuStructure.S_4x4x4)
        {
            latticeVisualAnchor.transform.Find("1-1").localPosition = new Vector3(0f, unitDistance, 0f);
            latticeVisualAnchor.transform.Find("1-2").localPosition = new Vector3(unitDistance, 0f, 0f);
            latticeVisualAnchor.transform.Find("1-3").localPosition = new Vector3(0f, unitDistance * -1, 0f);
            latticeVisualAnchor.transform.Find("1-4").localPosition = new Vector3(unitDistance * -1, 0f, 0f);
            latticeVisualAnchor.transform.Find("2-1").localPosition = new Vector3(0f, unitDistance * 2, 0f);
            latticeVisualAnchor.transform.Find("2-2").localPosition = new Vector3(unitDistance, unitDistance, 0f);
            latticeVisualAnchor.transform.Find("2-3").localPosition = new Vector3(unitDistance * 2, 0f, 0f);
            latticeVisualAnchor.transform.Find("2-4").localPosition = new Vector3(unitDistance, unitDistance * -1, 0f);
            latticeVisualAnchor.transform.Find("2-5").localPosition = new Vector3(0f, unitDistance * -2, 0f);
            latticeVisualAnchor.transform.Find("2-6").localPosition = new Vector3(unitDistance * -1, unitDistance * -1, 0f);
            latticeVisualAnchor.transform.Find("2-7").localPosition = new Vector3(unitDistance * -2, 0f, 0f);
            latticeVisualAnchor.transform.Find("2-8").localPosition = new Vector3(unitDistance * -1, unitDistance, 0f);
            latticeVisualAnchor.transform.Find("3-1").localPosition = new Vector3(0f, unitDistance * 3, 0f);
            latticeVisualAnchor.transform.Find("3-2").localPosition = new Vector3(unitDistance, unitDistance * 2, 0f);
            latticeVisualAnchor.transform.Find("3-3").localPosition = new Vector3(unitDistance * 2, unitDistance, 0f);
            latticeVisualAnchor.transform.Find("3-4").localPosition = new Vector3(unitDistance * 3, 0f, 0f);
            latticeVisualAnchor.transform.Find("3-5").localPosition = new Vector3(unitDistance * 2, unitDistance * -1, 0f);
            latticeVisualAnchor.transform.Find("3-6").localPosition = new Vector3(unitDistance, unitDistance * -2, 0f);
            latticeVisualAnchor.transform.Find("3-7").localPosition = new Vector3(0f, unitDistance * -3, 0f);
            latticeVisualAnchor.transform.Find("3-8").localPosition = new Vector3(unitDistance * -1, unitDistance * -2, 0f);
            latticeVisualAnchor.transform.Find("3-9").localPosition = new Vector3(unitDistance * -2, unitDistance * -1, 0f);
            latticeVisualAnchor.transform.Find("3-10").localPosition = new Vector3(unitDistance * -3, 0f, 0f);
            latticeVisualAnchor.transform.Find("3-11").localPosition = new Vector3(unitDistance * -2, unitDistance, 0f);
            latticeVisualAnchor.transform.Find("3-12").localPosition = new Vector3(unitDistance * -1, unitDistance * 2, 0f);
        }
        else if (_menuStructure == Eval_Manager.MenuStructure.S_6x6x6)
        {
            float unitDistance0X = unitDistance * (float)Math.Cos(Math.PI * 90 / 180.0);
            float unitDistance0Y = unitDistance * (float)Math.Sin(Math.PI * 90 / 180.0);
            float unitDistance1X = unitDistance * (float)Math.Cos(Math.PI * 30 / 180.0);
            float unitDistance1Y = unitDistance * (float)Math.Sin(Math.PI * 30 / 180.0);
            float unitDistance2X = unitDistance * (float)Math.Cos(Math.PI * -30 / 180.0);
            float unitDistance2Y = unitDistance * (float)Math.Sin(Math.PI * -30 / 180.0);
            float unitDistance3X = unitDistance * (float)Math.Cos(Math.PI * -90 / 180.0);
            float unitDistance3Y = unitDistance * (float)Math.Sin(Math.PI * -90 / 180.0);
            float unitDistance4X = unitDistance * (float)Math.Cos(Math.PI * -150 / 180.0);
            float unitDistance4Y = unitDistance * (float)Math.Sin(Math.PI * -150 / 180.0);
            float unitDistance5X = unitDistance * (float)Math.Cos(Math.PI * -210 / 180.0);
            float unitDistance5Y = unitDistance * (float)Math.Sin(Math.PI * -210 / 180.0);

            latticeVisualAnchor.transform.Find("1-1").localPosition = new Vector3(unitDistance0X, unitDistance0Y, 0f);
            latticeVisualAnchor.transform.Find("1-2").localPosition = new Vector3(unitDistance1X, unitDistance1Y, 0f);
            latticeVisualAnchor.transform.Find("1-3").localPosition = new Vector3(unitDistance2X, unitDistance2Y, 0f);
            latticeVisualAnchor.transform.Find("1-4").localPosition = new Vector3(unitDistance3X, unitDistance3Y, 0f);
            latticeVisualAnchor.transform.Find("1-5").localPosition = new Vector3(unitDistance4X, unitDistance4Y, 0f);
            latticeVisualAnchor.transform.Find("1-6").localPosition = new Vector3(unitDistance5X, unitDistance5Y, 0f);

            latticeVisualAnchor.transform.Find("2-1").localPosition = new Vector3(unitDistance0X + unitDistance0X, unitDistance0Y + unitDistance0Y, 0f);
            latticeVisualAnchor.transform.Find("2-2").localPosition = new Vector3(unitDistance0X + unitDistance1X, unitDistance0Y + unitDistance1Y, 0f);
            latticeVisualAnchor.transform.Find("2-3").localPosition = new Vector3(unitDistance1X + unitDistance1X, unitDistance1Y + unitDistance1Y, 0f);
            latticeVisualAnchor.transform.Find("2-4").localPosition = new Vector3(unitDistance1X + unitDistance2X, unitDistance1Y + unitDistance2Y, 0f);
            latticeVisualAnchor.transform.Find("2-5").localPosition = new Vector3(unitDistance2X + unitDistance2X, unitDistance2Y + unitDistance2Y, 0f);
            latticeVisualAnchor.transform.Find("2-6").localPosition = new Vector3(unitDistance2X + unitDistance3X, unitDistance2Y + unitDistance3Y, 0f);
            latticeVisualAnchor.transform.Find("2-7").localPosition = new Vector3(unitDistance3X + unitDistance3X, unitDistance3Y + unitDistance3Y, 0f);
            latticeVisualAnchor.transform.Find("2-8").localPosition = new Vector3(unitDistance3X + unitDistance4X, unitDistance3Y + unitDistance4Y, 0f);
            latticeVisualAnchor.transform.Find("2-9").localPosition = new Vector3(unitDistance4X + unitDistance4X, unitDistance4Y + unitDistance4Y, 0f);
            latticeVisualAnchor.transform.Find("2-10").localPosition = new Vector3(unitDistance4X + unitDistance5X, unitDistance4Y + unitDistance5Y, 0f);
            latticeVisualAnchor.transform.Find("2-11").localPosition = new Vector3(unitDistance5X + unitDistance5X, unitDistance5Y + unitDistance5Y, 0f);
            latticeVisualAnchor.transform.Find("2-12").localPosition = new Vector3(unitDistance5X + unitDistance0X, unitDistance5Y + unitDistance0Y, 0f);

            latticeVisualAnchor.transform.Find("3-1").localPosition = new Vector3(unitDistance0X + unitDistance0X + unitDistance0X, unitDistance0Y + unitDistance0Y + unitDistance0Y, 0f);
            latticeVisualAnchor.transform.Find("3-2").localPosition = new Vector3(unitDistance0X + unitDistance0X + unitDistance1X, unitDistance0Y + unitDistance0Y + unitDistance1Y, 0f);
            latticeVisualAnchor.transform.Find("3-3").localPosition = new Vector3(unitDistance0X + unitDistance1X + unitDistance1X, unitDistance0Y + unitDistance1Y + unitDistance1Y, 0f);
            latticeVisualAnchor.transform.Find("3-4").localPosition = new Vector3(unitDistance1X + unitDistance1X + unitDistance1X, unitDistance1Y + unitDistance1Y + unitDistance1Y, 0f);
            latticeVisualAnchor.transform.Find("3-5").localPosition = new Vector3(unitDistance1X + unitDistance1X + unitDistance2X, unitDistance1Y + unitDistance1Y + unitDistance2Y, 0f);
            latticeVisualAnchor.transform.Find("3-6").localPosition = new Vector3(unitDistance1X + unitDistance2X + unitDistance2X, unitDistance1Y + unitDistance2Y + unitDistance2Y, 0f);
            latticeVisualAnchor.transform.Find("3-7").localPosition = new Vector3(unitDistance2X + unitDistance2X + unitDistance2X, unitDistance2Y + unitDistance2Y + unitDistance2Y, 0f);
            latticeVisualAnchor.transform.Find("3-8").localPosition = new Vector3(unitDistance2X + unitDistance2X + unitDistance3X, unitDistance2Y + unitDistance2Y + unitDistance3Y, 0f);
            latticeVisualAnchor.transform.Find("3-9").localPosition = new Vector3(unitDistance2X + unitDistance3X + unitDistance3X, unitDistance2Y + unitDistance3Y + unitDistance3Y, 0f);
            latticeVisualAnchor.transform.Find("3-10").localPosition = new Vector3(unitDistance3X + unitDistance3X + unitDistance3X, unitDistance3Y + unitDistance3Y + unitDistance3Y, 0f);
            latticeVisualAnchor.transform.Find("3-11").localPosition = new Vector3(unitDistance3X + unitDistance3X + unitDistance4X, unitDistance3Y + unitDistance3Y + unitDistance4Y, 0f);
            latticeVisualAnchor.transform.Find("3-12").localPosition = new Vector3(unitDistance3X + unitDistance4X + unitDistance4X, unitDistance3Y + unitDistance4Y + unitDistance4Y, 0f);
            latticeVisualAnchor.transform.Find("3-13").localPosition = new Vector3(unitDistance4X + unitDistance4X + unitDistance4X, unitDistance4Y + unitDistance4Y + unitDistance4Y, 0f);
            latticeVisualAnchor.transform.Find("3-14").localPosition = new Vector3(unitDistance4X + unitDistance4X + unitDistance5X, unitDistance4Y + unitDistance4Y + unitDistance5Y, 0f);
            latticeVisualAnchor.transform.Find("3-15").localPosition = new Vector3(unitDistance4X + unitDistance5X + unitDistance5X, unitDistance4Y + unitDistance5Y + unitDistance5Y, 0f);
            latticeVisualAnchor.transform.Find("3-16").localPosition = new Vector3(unitDistance5X + unitDistance5X + unitDistance5X, unitDistance5Y + unitDistance5Y + unitDistance5Y, 0f);
            latticeVisualAnchor.transform.Find("3-17").localPosition = new Vector3(unitDistance5X + unitDistance5X + unitDistance0X, unitDistance5Y + unitDistance5Y + unitDistance0Y, 0f);
            latticeVisualAnchor.transform.Find("3-18").localPosition = new Vector3(unitDistance5X + unitDistance0X + unitDistance0X, unitDistance5Y + unitDistance0Y + unitDistance0Y, 0f);
        }
    }

    public static void visualAnchorSizeInit(ref GameObject latticeVisualAnchor, float visualAnchorDiameter)
    {
        float degreeToMeter = 0.174537f;
        for (int i = 0; i < latticeVisualAnchor.transform.childCount; i++)
            latticeVisualAnchor.transform.GetChild(i).GetChild(0).localScale = new Vector3(visualAnchorDiameter * degreeToMeter, visualAnchorDiameter * degreeToMeter, 0.05f);
    }

    public static void itemSelectionZoneSizeInit(ref GameObject latticeVisualAnchor, float itemSelectionZoneDiameter)
    {
        float degreeToMeter = 0.174537f;
        for (int i = 0; i < latticeVisualAnchor.transform.childCount; i++)
            latticeVisualAnchor.transform.GetChild(i).GetChild(1).localScale = new Vector3(itemSelectionZoneDiameter / 2 * degreeToMeter, 1f, itemSelectionZoneDiameter / 2 * degreeToMeter);
    }
    
    // GetFourSurroundingAnchors()
    // (ex. trajectoryHistory "00" : Four surounding visual anchor around the "north - north" visual anchor)
    public static GameObject[] GetFourSurroundingAnchors(ref GameObject latticeVisualAnchor, string trajectoryHistory)
    {
        string[] latticePointStr = { "1-1", "1-2", "1-3", "1-4" };       
        string[] latticePointStr0 = { "2-1", "2-2", "0-1", "2-8" };    
        string[] latticePointStr1 = { "2-2", "2-3", "2-4", "0-1" };  
        string[] latticePointStr2 = { "0-1", "2-4", "2-5", "2-6" };   
        string[] latticePointStr3 = { "2-8", "0-1", "2-6", "2-7" };     
        string[] latticePointStr00 = { "3-1", "3-2", "1-1", "3-12" };    
        string[] latticePointStr01 = { "3-2", "3-3", "1-2", "1-1" };   
        string[] latticePointStr02 = { "1-1", "1-2", "1-3", "1-4" };    
        string[] latticePointStr03 = { "3-12", "1-1", "1-4", "3-11" }; 
        string[] latticePointStr10 = { "3-2", "3-3", "1-2", "1-1" };   
        string[] latticePointStr11 = { "3-3", "3-4", "3-5", "1-2" };    
        string[] latticePointStr12 = { "1-2", "3-5", "3-6", "1-3" };   
        string[] latticePointStr13 = { "1-1", "1-2", "1-3", "1-4" };  
        string[] latticePointStr20 = { "1-1", "1-2", "1-3", "1-4" };    
        string[] latticePointStr21 = { "1-2", "3-5", "3-6", "1-3" };   
        string[] latticePointStr22 = { "1-3", "3-6", "3-7", "3-8" };   
        string[] latticePointStr23 = { "1-4", "1-3", "3-8", "3-9" };   
        string[] latticePointStr30 = { "3-12", "1-1", "1-4", "3-11" };   
        string[] latticePointStr31 = { "1-1", "1-2", "1-3", "1-4" };  
        string[] latticePointStr32 = { "1-4", "1-3", "3-8", "3-9" };     
        string[] latticePointStr33 = { "3-11", "1-4", "3-9", "3-10" };  

        GameObject[] latticeFound = new GameObject[4];
        if (trajectoryHistory == "")
        {
            for (int i = 0; i < 4; i++)
                latticeFound[i] = latticeVisualAnchor.transform.Find(latticePointStr[i]).gameObject;
        }
        else if (trajectoryHistory.Length == 1)
        {
            string[][] latticeStrFirst = { latticePointStr0, latticePointStr1, latticePointStr2, latticePointStr3 };
            for (int i = 0; i < 4; i++)
                latticeFound[i] = latticeVisualAnchor.transform.Find(latticeStrFirst[Convert.ToInt32(trajectoryHistory)][i]).gameObject;
        }
        else if (trajectoryHistory.Length == 2)
        {
            string[][] latticeStrSecond0 = { latticePointStr00, latticePointStr01, latticePointStr02, latticePointStr03 };
            string[][] latticeStrSecond1 = { latticePointStr10, latticePointStr11, latticePointStr12, latticePointStr13 };
            string[][] latticeStrSecond2 = { latticePointStr20, latticePointStr21, latticePointStr22, latticePointStr23 };
            string[][] latticeStrSecond3 = { latticePointStr30, latticePointStr31, latticePointStr32, latticePointStr33 };
            string[][][] latticeStrSeconds = { latticeStrSecond0, latticeStrSecond1, latticeStrSecond2, latticeStrSecond3 };
            for (int i = 0; i < 4; i++)
                latticeFound[i] = latticeVisualAnchor.transform.Find(latticeStrSeconds[Convert.ToInt32(trajectoryHistory.Substring(0, 1))][Convert.ToInt32(trajectoryHistory.Substring(1, 1))][i]).gameObject;
        }
        return latticeFound;
    }

    // GetSixSurroundingAnchors()
    // (ex. trajectoryHistory "00" : Six surounding visual anchor around the "north - north" visual anchor)
    public static GameObject[] GetSixSurroundingAnchors(ref GameObject latticeVisualAnchor, string trajectoryHistory)
    {
        string[] latticePointStr = { "1-1", "1-2", "1-3", "1-4", "1-5", "1-6" };      
        string[] latticePointStr0 = { "2-1", "2-2", "1-2", "0-1", "1-6", "2-12" };  
        string[] latticePointStr1 = { "2-2", "2-3", "2-4", "1-3", "0-1", "1-1" };    
        string[] latticePointStr2 = { "1-2", "2-4", "2-5", "2-6", "1-4", "0-1" };  
        string[] latticePointStr3 = { "0-1", "1-3", "2-6", "2-7", "2-8", "1-5" };   
        string[] latticePointStr4 = { "1-6", "0-1", "1-4", "2-8", "2-9", "2-10" };     
        string[] latticePointStr5 = { "2-12", "1-1", "0-1", "1-5", "2-10", "2-11" };  
        string[] latticePointStr00 = { "3-1", "3-2", "2-2", "1-1", "2-12", "3-18" };   
        string[] latticePointStr01 = { "3-2", "3-3", "2-3", "1-2", "1-1", "2-1" };   
        string[] latticePointStr02 = { "2-2", "2-3", "2-4", "1-3", "0-1", "1-1" };   
        string[] latticePointStr03 = { "1-1", "1-2", "1-3", "1-4", "1-5", "1-6" };  
        string[] latticePointStr04 = { "2-12", "1-1", "0-1", "1-5", "2-10", "2-11" }; 
        string[] latticePointStr05 = { "3-18", "2-1", "1-1", "1-6", "2-11", "3-17" };  
        string[] latticePointStr10 = { "3-2", "3-3", "2-3", "1-2", "1-1", "2-1" };
        string[] latticePointStr11 = { "3-3", "3-4", "3-5", "2-4", "1-2", "2-2" };    
        string[] latticePointStr12 = { "2-3", "3-5", "3-6", "2-5", "1-3", "1-2" };    
        string[] latticePointStr13 = { "1-2", "2-4", "2-5", "2-6", "1-4", "0-1" };  
        string[] latticePointStr14 = { "1-1", "1-2", "1-3", "1-4", "1-5", "1-6" };
        string[] latticePointStr15 = { "2-1", "2-2", "1-2", "0-1", "1-6", "2-12" };
        string[] latticePointStr20 = { "2-2", "2-3", "2-4", "1-3", "0-1", "1-1" };
        string[] latticePointStr21 = { "2-3", "3-5", "3-6", "2-5", "1-3", "1-2" };
        string[] latticePointStr22 = { "2-4", "3-6", "3-7", "3-8", "2-6", "1-3" };  
        string[] latticePointStr23 = { "1-3", "2-5", "3-8", "3-9", "2-7", "1-4" };    
        string[] latticePointStr24 = { "0-1", "1-3", "2-6", "2-7", "2-8", "1-5" };
        string[] latticePointStr25 = { "1-1", "1-2", "1-3", "1-4", "1-5", "1-6" };
        string[] latticePointStr30 = { "1-1", "1-2", "1-3", "1-4", "1-5", "1-6" };
        string[] latticePointStr31 = { "1-2", "2-4", "2-5", "2-6", "1-4", "0-1" };
        string[] latticePointStr32 = { "1-3", "2-5", "3-8", "3-9", "2-7", "1-4" };
        string[] latticePointStr33 = { "1-4", "2-6", "3-9", "3-10", "3-11", "2-8" };   
        string[] latticePointStr34 = { "1-5", "1-4", "2-7", "3-11", "3-12", "2-9" };   
        string[] latticePointStr35 = { "1-6", "0-1", "1-4", "2-8", "2-9", "2-10" };
        string[] latticePointStr40 = { "2-12", "1-1", "0-1", "1-5", "2-10", "2-11" };
        string[] latticePointStr41 = { "1-1", "1-2", "1-3", "1-4", "1-5", "1-6" };
        string[] latticePointStr42 = { "0-1", "1-3", "2-6", "2-7", "2-8", "1-5" };
        string[] latticePointStr43 = { "1-5", "1-4", "2-7", "3-11", "3-12", "2-9" };
        string[] latticePointStr44 = { "2-10", "1-5", "2-8", "3-12", "3-13", "3-14" };
        string[] latticePointStr45 = { "2-11", "1-6", "1-5", "2-9", "3-14", "3-15" };
        string[] latticePointStr50 = { "3-18", "2-1", "1-1", "1-6", "2-11", "3-17" };
        string[] latticePointStr51 = { "2-1", "2-2", "1-2", "0-1", "1-6", "2-12" };
        string[] latticePointStr52 = { "1-1", "1-2", "1-3", "1-4", "1-5", "1-6" };
        string[] latticePointStr53 = { "1-6", "0-1", "1-4", "2-8", "2-9", "2-10" };
        string[] latticePointStr54 = { "2-11", "1-6", "1-5", "2-9", "3-14", "3-15" };
        string[] latticePointStr55 = { "3-17", "2-12", "1-6", "2-10", "3-15", "3-16" };   

        GameObject[] latticeFound = new GameObject[6];
        if (trajectoryHistory == "")
        {
            for (int i = 0; i < 6; i++)
                latticeFound[i] = latticeVisualAnchor.transform.Find(latticePointStr[i]).gameObject;
        }
        else if (trajectoryHistory.Length == 1)
        {
            string[][] latticeStrFirst = { latticePointStr0, latticePointStr1, latticePointStr2, latticePointStr3, latticePointStr4, latticePointStr5 };
            for (int i = 0; i < 6; i++)
                latticeFound[i] = latticeVisualAnchor.transform.Find(latticeStrFirst[Convert.ToInt32(trajectoryHistory)][i]).gameObject;
        }
        else if (trajectoryHistory.Length == 2)
        {
            string[][] latticeStrSecond0 = { latticePointStr00, latticePointStr01, latticePointStr02, latticePointStr03, latticePointStr04, latticePointStr05 };
            string[][] latticeStrSecond1 = { latticePointStr10, latticePointStr11, latticePointStr12, latticePointStr13, latticePointStr14, latticePointStr15 };
            string[][] latticeStrSecond2 = { latticePointStr20, latticePointStr21, latticePointStr22, latticePointStr23, latticePointStr24, latticePointStr25 };
            string[][] latticeStrSecond3 = { latticePointStr30, latticePointStr31, latticePointStr32, latticePointStr33, latticePointStr34, latticePointStr35 };
            string[][] latticeStrSecond4 = { latticePointStr40, latticePointStr41, latticePointStr42, latticePointStr43, latticePointStr44, latticePointStr45 };
            string[][] latticeStrSecond5 = { latticePointStr50, latticePointStr51, latticePointStr52, latticePointStr53, latticePointStr54, latticePointStr55 };
            string[][][] latticeStrSeconds = { latticeStrSecond0, latticeStrSecond1, latticeStrSecond2, latticeStrSecond3, latticeStrSecond4, latticeStrSecond5 };
            for (int i = 0; i < 6; i++)
                latticeFound[i] = latticeVisualAnchor.transform.Find(latticeStrSeconds[Convert.ToInt32(trajectoryHistory.Substring(0, 1))][Convert.ToInt32(trajectoryHistory.Substring(1, 1))][i]).gameObject;
        }
        return latticeFound;
    }

    public static void progressiveUnfoldingEffectApply(ref GameObject latticeVisualAnchor, Eval_Manager.ProgressiveUnfoldingEffect _progressiveUnfoldingEffect)
    {
        if (_progressiveUnfoldingEffect == Eval_Manager.ProgressiveUnfoldingEffect.ProgressiveLatticeMenu)
        {
            for (int i = 0; i < latticeVisualAnchor.transform.childCount; i++)
                latticeVisualAnchor.transform.GetChild(i).GetChild(0).GetComponent<Renderer>().material.SetVector("_Color", new Vector4(0xFF, 0xFF, 0xFF, 0x00));
        }
    }
}
