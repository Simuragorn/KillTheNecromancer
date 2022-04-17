using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int health;
    public Action OnDeath;

    public void Init(int startHealth)
    {
        health = startHealth;
    }

    public void GetDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (OnDeath != null)
            {
                OnDeath();
            }
        }
    }
}
