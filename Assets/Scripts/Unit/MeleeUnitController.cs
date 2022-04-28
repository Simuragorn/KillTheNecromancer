using Assets.Scripts.Constants;
using System.Collections;
using UnityEngine;

public class MeleeUnitController : BaseUnitController
{
    [SerializeField] protected UnitAttackZone attackZone;

    protected override void OnSpawning()
    {
        base.OnSpawning();
        attackZone.enabled = false;
    }

    public override void OnSpawned()
    {
        base.OnSpawned();
        attackZone.enabled = true;
    }

    public override void MoveTo(Vector2 targetPosition, MoveTypeEnum moveType)
    {
        if (moveType == MoveTypeEnum.ToEnemy)
            ChaseEnemy(targetPosition);
        else
            base.MoveTo(targetPosition, moveType);
    }

    public override void ChangeOrder(UnitOrderEnum order)
    {
        if (order == UnitOrderEnum.DistantAttack)
            return;

        base.ChangeOrder(order);
    }

    public override void Init(UnitsCamp unitCamp)
    {
        base.Init(unitCamp);
        attackZone.Init(this);
    }

    protected void ChaseEnemy(Vector2 targetPosition)
    {
        if (IsChasing || CurrentAction == UnitActionEnum.Attacking)
            return;

        CurrentAction = UnitActionEnum.Chasing;
        unitMove.MoveTo(targetPosition, MoveTypeEnum.ToEnemy);

        if (camp != null)
        {
            camp.EnemyUnitSpotted(targetPosition);
        }
        StartCoroutine(StartChasingDelay());
    }

    protected IEnumerator StartChasingDelay()
    {
        IsChasing = true;
        yield return new WaitForSeconds(chasingDelayInSeconds);
        IsChasing = false;
    }

    public override void Attack()
    {
        if (attackZone.EnemyTarget != null)
            attackZone.DealDamage();
        else
            ResetState();
    }

    public override void SetTarget(Vector2 targetPosition)
    {
        Debug.Log("Target set");
    }
}
