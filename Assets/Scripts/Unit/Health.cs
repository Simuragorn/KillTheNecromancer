using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D collider;
    private int health;
    public Action OnDeath;
    public int CurrentHealth => health;

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
                OnDeath();
            return;
        }
        animator.SetTrigger("Hit");
    }

    private void OnEnable()
    {
        collider.enabled = true;
    }

    private void OnDisable()
    {
        collider.enabled = false;
    }
}
