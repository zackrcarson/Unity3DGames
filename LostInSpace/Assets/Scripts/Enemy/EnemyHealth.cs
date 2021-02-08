using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int enemyHealth = 100;
    
    public void DamageEnemy(int damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            KillEnemy();
        }

        BroadcastMessage("OnDamageTaken");
    }

    private void KillEnemy()
    {
        // TODO Enemy death sequence and VFX

        Destroy(gameObject);
    }
}
