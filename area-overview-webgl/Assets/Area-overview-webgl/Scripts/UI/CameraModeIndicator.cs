using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Paint image on click
 * Indicate current camera mode in UI 
 */
public class CameraModeIndicator : MonoBehaviour
{
    private enum DefaultMode
    {
        walk,
        orbit
    }
    [SerializeField] private DefaultMode defaultMode;
    [Space]
    [SerializeField] private Image walk;
    [SerializeField] private Image orbit;
    [SerializeField] private Color32 standardColor;
    [SerializeField] private Color32 pressedColor;
    
    [Space]
    [SerializeField] private Image walkBorder;
    [SerializeField] private Image orbitBorder;
    [SerializeField] private Color32 standardColorBorder;
    [SerializeField] private Color32 pressedColorBorder;

    
    // Indicate walk mode, call from ui
    public void SetWalkColor()
    {
        walk.color = pressedColor;
        orbit.color = standardColor;
        
        walkBorder.color = pressedColorBorder;
        orbitBorder.color = standardColorBorder;
    }

    // Indicate orbit mode, call from ui
    public void SetOrbitColor()
    {
        orbit.color = pressedColor;
        walk.color = standardColor;
        
        orbitBorder.color = pressedColorBorder;
        walkBorder.color = standardColorBorder;
    }
}
