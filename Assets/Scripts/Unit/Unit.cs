using Assets.Scripts.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Unit
{
    [Header("Base")]
    [SerializeField] private UnitEnum id;
    public UnitEnum Id => id;
    [SerializeField] private string unitName;
    public string UnitName => unitName;
    [SerializeField] private LayerMask enemyLayer;
    public LayerMask EnemyLayer => enemyLayer;
    [SerializeField] private UnitTypeEnum unitType;
    public UnitTypeEnum UnitType => unitType;

    [Header("Price")]
    [SerializeField] private int cost;
    public int Cost => cost;

    [SerializeField] private int reward;
    public int Reward => reward;

    [Header("Stats")]
    [SerializeField] private int damage;
    public int Damage => damage;
    [SerializeField] private float speed;
    public float Speed => speed;
    [SerializeField] private int health;
    public int Health => health;
    [SerializeField] private float reloadInSeconds;
    public float ReloadInSeconds => reloadInSeconds;
    [SerializeField] private float sightRange;
    public float SightRange => sightRange;
    [SerializeField] private float distantRange;
    public float DistantRange => distantRange;
    [SerializeField] private float distantSpotOffset;
    public float DistantSpotOffset => distantSpotOffset;
    [SerializeField] private float projectileSpeed;
    public float ProjectileSpeed => projectileSpeed;
    [SerializeField] private float stunInSeconds;
    public float StunInSeconds => stunInSeconds;
    [SerializeField] private int blockChancePercentage;
    public int BlockChancePercentage => blockChancePercentage;
}
