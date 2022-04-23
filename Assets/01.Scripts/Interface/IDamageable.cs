using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void OnDamage(int damage, GameObject damageDealer, Vector2 dir, float force);
}
