using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleScore : MonoBehaviour
{
    [SerializeField] float scoreMultiplier = 1;
    public float ScoreMultiplier { get { return scoreMultiplier; } }
}
