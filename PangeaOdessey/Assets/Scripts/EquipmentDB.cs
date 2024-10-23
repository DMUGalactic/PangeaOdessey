using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Equipment Info", menuName = "Scriptable Object Asset/EquipmentInfo")]
public class EquipmentDB : ScriptableObject
{
    public int[] inventory = new int[12];

    public int[] equipment = new int[4];
}