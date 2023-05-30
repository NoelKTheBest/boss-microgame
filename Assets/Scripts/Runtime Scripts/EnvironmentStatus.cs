using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnvironmentStatus// : MonoBehaviour
{
    /* Instead of making a class that carries an attack value and be forced to 
     *  share or pass a reference to that class to another script, I can create
     *  this static class that lets scripts know about the state of the
     *  environment at any given time. This is especially useful for knowing what
     *  the environment is like during events.
     *  
     * I will use this to have PlayerHurbox.cs check for if fire exists (set by the 
     *  boss controller script), and the player will receive more or less damage
     *  depending on the value of this variable. If true, more damage is done.
     *  If false, less damage is done. The only thing the player will now have to
     *  worry about is receiving damage and updating their own values.
     *  
     * No more passing references or sharing references. No more needing to
     *  manipulate the values of a bunch of different classes.
     */
    public static bool fireExists;
}
