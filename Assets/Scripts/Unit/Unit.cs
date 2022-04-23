using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Unit
{
    [SerializeField] private int id;
    public int Id => id;
    [SerializeField] private string unitName;
    public string UnitName => unitName;
    [SerializeField] private int cost;
    public int Cost => cost;
    [SerializeField] private int damage;

    [SerializeField] private int reward;
    public int Reward => reward;
    public int Damage => damage;
    [SerializeField] private float speed;
    public float Speed => speed;
    [SerializeField] private LayerMask enemyLayer;
    public LayerMask EnemyLayer => enemyLayer;
    [SerializeField] private int health;
    public int Health => health;
    [SerializeField] private float reloadInSeconds;
    public float ReloadInSeconds => reloadInSeconds;
    [SerializeField] private float sightRange;
    public float SightRange => sightRange;
}
