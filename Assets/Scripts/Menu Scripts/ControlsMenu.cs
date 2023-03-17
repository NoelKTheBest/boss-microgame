using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlsMenu : MonoBehaviour
{
    public TMP_InputField attackAField;
    public TMP_InputField attackBField;
    public TMP_InputField attackCField;
    public TMP_InputField upgradeField;
    public TMP_InputField dashField;
    public TMP_InputField dodgeField;
    public TMP_InputField attackAbiltiyField;
    public TMP_InputField defenseAbilityField;
    public TMP_Text attackAText;
    public TMP_Text attackBText;
    public TMP_Text attackCText;
    public TMP_Text upgradeText;
    public TMP_Text dashText;
    public TMP_Text dodgeText;
    public TMP_Text attackAbiltiyText;
    public TMP_Text defenseAbilityText;
    [SerializeField] [Tooltip("0 - look & 1 - move")] private int inputDirectionForAttack;
    [SerializeField] [Tooltip("0 - look & 1 - move")] private int inputDirectionForDash;
    [SerializeField] [Tooltip("0 - look & 1 - move")] private int inputDirectionForAbility;
    public TMP_Text DirForAtkText;
    public TMP_Text DirForDashText;
    public TMP_Text DirForAblyText;

    private void Start()
    {
        attackAField.characterLimit = 1;
        attackBField.characterLimit = 1;
        attackCField.characterLimit = 1;
        upgradeField.characterLimit = 1;
        dashField.characterLimit = 1;
        dodgeField.characterLimit = 1;
        attackAbiltiyField.characterLimit = 1;
        defenseAbilityField.characterLimit = 1;
        
        //Load Values for input directions and set variables
    }

    //zero
    public void SwitchInputDirectionsForAttack()
    {
        inputDirectionForAttack = 1 - inputDirectionForAttack;

        switch (inputDirectionForAttack)
        {
            case 0:
                DirForAtkText.text = "look";
                break;
            case 1:
                DirForAtkText.text = "move";
                break;
        }
    }

    public void SwitchInputDirectionsForDash()
    {
        inputDirectionForDash = 1 - inputDirectionForDash;

        switch (inputDirectionForDash)
        {
            case 0:
                DirForDashText.text = "look";
                break;
            case 1:
                DirForDashText.text = "move";
                break;
        }
    }

    public void SwitchInputDirectionsForAbility()
    {
        inputDirectionForAbility = 1 - inputDirectionForAbility;

        switch (inputDirectionForAbility)
        {
            case 0:
                DirForAblyText.text = "look";
                break;
            case 1:
                DirForAblyText.text = "move";
                break;
        }
    }

    public void UpdateAtkA(string inputString)
    {
        attackAText.text = inputString;
        attackAField.text = "";
    }

    public void UpdateAtkB(string inputString)
    {
        attackBText.text = inputString;
        attackBField.text = "";
    }

    public void UpdateAtkC(string inputString)
    {
        attackCText.text = inputString;
        attackCField.text = "";
    }

    public void UpdateUpB(string inputString)
    {
        upgradeText.text = inputString;
        upgradeField.text = "";
    }

    public void UpdateDashB(string inputString)
    {
        dashText.text = inputString;
        dashField.text = "";

    }

    public void UpdateDodgeB(string inputString)
    {
        dodgeText.text = inputString;
        dodgeField.text = "";
    }

    public void UpdateAtkAB(string inputString)
    {
        attackAbiltiyText.text = inputString;
        attackAbiltiyField.text = "";
    }

    public void UpdateDefAB(string inputString)
    {
        defenseAbilityText.text = inputString;
        defenseAbilityField.text = "";
    }
}
