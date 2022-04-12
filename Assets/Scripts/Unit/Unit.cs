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
    public int Damage => damage;
    [SerializeField] private float speed;
    public float Speed => speed;
}
