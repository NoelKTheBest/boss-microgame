using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Character")]
public class CharacterStats : ScriptableObject
{
    public float hp;
    public float atk;
    public float atkSpd;
    public float def;
    public float spd;
}