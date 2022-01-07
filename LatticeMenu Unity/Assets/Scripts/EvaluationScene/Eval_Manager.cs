using UnityEngine;
using System.IO;

public class Eval_Manager : MonoBehaviour
{
    public enum MenuStructure
    {
        S_4x4x4,
        S_6x6x6,
    }

    public enum MenuSize
    {
        Deg_8 = 8,
        Deg_10 = 10,
        Deg_12 = 12,
    }
    public enum ProgressiveUnfoldingEffect
    {
        ProgressiveLatticeMenu = 100,
        FullLatticeMenu = 0,
    }

    [HideInInspector] public MenuStructure _menuStructure;
    [HideInInspector] public MenuSize _menuSize;
    [HideInInspector] public ProgressiveUnfoldingEffect _progressiveUnfoldingEffect;
}
