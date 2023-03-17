using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Preset", menuName = "Presets/Player Presets", order = 1)]
public class PlayerPreset : ScriptableObject
{
    public bool inHotbar;
    public int hotbarNumber;
    public string presetName;
    public float hp = 100; //keep static
    [Range(1, 100)] public float atk;
    [Range(1, 100)] public float def;
    [Range(1, 10)] public float baseMoveSpd;
}
