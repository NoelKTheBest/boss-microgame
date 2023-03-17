using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Preset", menuName = "Presets/Enemy Presets", order = 2)]
public class EnemyPreset : ScriptableObject
{
    public int presetNumber;
    public string presetName;
    [Range(1, 1000)] public float hp; //dont change in preset menu
    [Range(1, 100)] public float atk;
    [Range(1, 100)] public float def;
    [Range(1, 10)] public float baseMoveSpd;
}
