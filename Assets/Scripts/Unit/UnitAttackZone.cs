using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackZone : MonoBehaviour
{
    private UnitController unit;
    [SerializeField] private BoxCollider2D boxCollider;
    private int damage;
    private float reloadInSeconds;
    public bool IsReloading { private set; get; }
    public float Widht { get; private set; }
    public float Height { get; private set; }

    public UnitController EnemyTarget { get; private set; }

    public void Init(UnitController unitController)
    {
        unit = unitController;
        damage = unitController.Unit.Damage;
        reloadInSeconds = unitController.Unit.ReloadInSeconds;

        Widht = boxCollider.size.x;
        Height = boxCollider.size.y;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (unit.CurrentAction != UnitActionEnum.Attacking)
        {
            int targetLayer = 1 << collision.gameObject.layer;
            if (targetLayer == unit.Unit.EnemyLayer)
            {
                TryGetTarget(collision.gameObject);
                if (EnemyTarget != null)
                {
                    StartAttack();
                }
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

    private void TryGetTarget(GameObject enemy)
    {
        UnitController enemyUnit;
        if (unit.IsPlayerUnit)
        {
            EnemyUnitsManager.Instance.Enemies.TryGetValue(enemy, out enemyUnit);
        }
        else
        {
            PlayerUnitsManager.Instance.PlayerUnits.TryGetValue(enemy, out enemyUnit);
        }
        EnemyTarget = enemyUnit;
    }

    private void StartAttack()
    {
        if (IsReloading)
        {
            return;
        }

        unit.StartAttack();
    }

    public void DealDamage()
    {
        if (EnemyTarget != null)
        {
            EnemyTarget.GetDamage(damage);
            StartCoroutine(ReloadAndAttack());
        }
    }

    private IEnumerator ReloadAndAttack()
    {
        IsReloading = true;
        yield return new WaitForSeconds(reloadInSeconds);
        IsReloading = false;

        if (EnemyTarget != null)
        {
            StartAttack();
        }
        else
        {
            unit.ResetState();
        }
    }
}
