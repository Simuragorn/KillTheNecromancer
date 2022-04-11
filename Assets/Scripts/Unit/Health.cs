using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health;
    public Action OnDeath;

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
