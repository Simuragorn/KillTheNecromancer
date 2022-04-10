using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Health health;
    void Start()
    {
        health.OnDeath += Death;
        EnemiesController.Instance.Enemies.Add(gameObject, this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetDamage(int damage)
    {
        health.GetDamage(damage);
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
