using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // Damageable Interface for the player to get hit by Mothman
    void TakeDamage(int damage);
    void ShowHitEffect();
}
