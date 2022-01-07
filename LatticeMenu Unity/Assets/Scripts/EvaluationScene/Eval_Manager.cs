/*
 * Writer: Taejun Kim, HCI Lab KAIST - https://taejun13.github.io/
 * Last Update: 2022. 1. 7
 * Lattice Menu: A Low-Error Gaze-Based Marking Menu Utilizing Target-Assisted Gaze Gestures on a Lattice of Visual Anchors (ACM CHI 2022)
 * ACM CHI 22': Conference on Human Factors in Computing Systems.
 * DOI: (TBU)
 */

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
