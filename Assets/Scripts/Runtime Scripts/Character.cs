using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public abstract void DetermineDirections();
    public abstract void ResetVars();
    public abstract void AnimateAttacks();
    public abstract void AnimateMovement();
    public abstract void AnimateDamage(Vector2 a);
    public abstract void Die();
}

public interface IEnemyCharacter
{
    void DetermineInputs();
}
