using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForDashMechanic : MonoBehaviour
{
    private Animator dashAnim;

    private void Awake()
    {
        dashAnim = GetComponent<Animator>();
    }

    public void StartAnim()
    {
        dashAnim.SetBool("Play", true);
    }

    public void SetFalse()
    {
        dashAnim.SetBool("Play", false);
    }
}
