using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Control Binding")]
public class ControlBindings : ScriptableObject
{
    public KeyCode attackAKey;
    public KeyCode attackBKey;
    public KeyCode attackCKey;
    public KeyCode upgradeKey;
    public KeyCode dashKey;
    public KeyCode dodgeKey;
    public KeyCode attackAbilityKey;
    public KeyCode defenseAbilityKey;
}
