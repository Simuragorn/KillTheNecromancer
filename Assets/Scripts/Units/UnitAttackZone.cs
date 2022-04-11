using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackZone : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private int damage;
    public float Widht { get; private set; }
    public float Height { get; private set; }

    public Enemy EnemyTarget { get; private set; }


    private void Start()
    {
        Widht = boxCollider.size.x;
        Height = boxCollider.size.y;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (unit.CurrentState != UnitStateEnum.Attacking)
        {
            if (EnemiesController.Instance.Enemies.TryGetValue(collision.gameObject, out Enemy enemy))
            {
                unit.StartAttack();
                EnemyTarget = enemy;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (EnemyTarget != null && EnemyTarget.gameObject == collision.gameObject)
        {
            EnemyTarget = null;
        }
    }

    public void DealDamage()
    {
        if (EnemyTarget != null)
        {
            EnemyTarget.GetDamage(damage);
        }
    }
}
