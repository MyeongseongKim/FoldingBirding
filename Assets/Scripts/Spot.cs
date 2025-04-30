using System;
using UnityEngine;

// [Serializable]
public class Spot : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;

    public void Occupy() => IsOccupied = true;
    public void Vacate() => IsOccupied = false;
}