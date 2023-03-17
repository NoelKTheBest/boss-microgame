using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadPresets : MonoBehaviour
{
    public void LoadPlayerPresets()
    {
        File.OpenRead(Application.persistentDataPath + "/Player Presets/");
    }

    public void LoadEnemyPresets()
    {

    }
}
