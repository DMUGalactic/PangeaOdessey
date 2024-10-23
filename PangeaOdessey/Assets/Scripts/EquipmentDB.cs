using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Equipment Info", menuName = "Scriptable Object Asset/EquipmentInfo")]
public class EquipmentDB : ScriptableObject
{
    public string[] inventory = new string[12];

    public string[] equipment = new string[4];

}