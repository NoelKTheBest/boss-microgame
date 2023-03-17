using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CreatePreset : MonoBehaviour
{
    public void CreatePlayerPreset()
    {
        PlayerPreset newPreset = (PlayerPreset)ScriptableObject.CreateInstance(typeof(PlayerPreset));
        string json = JsonUtility.ToJson(newPreset);
        File.WriteAllText(Application.persistentDataPath + "/Player Presets/preset.json", json);
    }

    public void CreateEnemyPreset()
    {
        EnemyPreset newPreset = (EnemyPreset)ScriptableObject.CreateInstance(typeof(EnemyPreset));
        string json = JsonUtility.ToJson(newPreset);
        File.WriteAllText(Application.persistentDataPath + "/Enemy Presets/preset.json", json);
    }
}
