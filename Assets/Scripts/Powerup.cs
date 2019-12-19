using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum Powerups { SpeedBoost, Hover};
    public Powerups powerup;

    public float duration = 4;
    public float speedBoost = 4;
    public float hoverValue = 2;
}
