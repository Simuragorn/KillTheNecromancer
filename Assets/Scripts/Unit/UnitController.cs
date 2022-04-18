using Assets.Scripts.Constants;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] private GameObject unitPosition;
    [SerializeField] private UnitMove unitMove;
    [SerializeField] private Animator animator;
    [SerializeField] private UnitAttackZone attackZone;
    [SerializeField] private UnitSight sight;
    [SerializeField] private Health health;
    [SerializeField] private UnitEnum unitId;
    public UnitEnum UnitId => unitId;

    private UnitsCamp camp;

    public Unit Unit { private set; get; }

    public UnitStateEnum CurrentState { get; private set; }
    public bool IsFreeWay => CurrentState != UnitStateEnum.Chasing &&
            CurrentState != UnitStateEnum.Attacking;

    public bool IsPositionFreezed => CurrentState == UnitStateEnum.Attacking;

    public void Init(UnitsCamp unitCamp)
    {
        camp = unitCamp;
    }


    private void Start()
    {
        Unit = PlayerUnitsManager.Instance.GetUnitById(UnitId);
        int currentLayer = 1 << gameObject.layer;
        if (UnitEnumExtensions.IsPlayerUnit((UnitEnum)UnitId))
        {
            PlayerUnitsManager.Instance.PlayerUnits.Add(gameObject, this);
        }
        else
        {
            EnemyUnitsManager.Instance.AddUnit(this);
        }
        health.OnDeath += Death;

        attackZone.Init(this);
        health.Init(Unit.Health);
        sight.Init(this);
    }

    public void GetDamage(int damage)
    {
        health.GetDamage(damage);
    }

    public void MoveTo(Vector2 targetPosition, MoveTypeEnum moveType)
    {
        if (moveType == MoveTypeEnum.ToEnemy)
        {
            ChaseEnemy(targetPosition);
        }
        else
        {
            CurrentState = UnitStateEnum.Moving;
            targetPosition -= (Vector2)unitPosition.transform.localPosition;
            unitMove.MoveTo(targetPosition);
        }
    }

    public void StartAttack()
    {
        CurrentState = UnitStateEnum.Attacking;
        animator.SetTrigger("StartAttack");
    }

    public void Attack()
    {
        if (attackZone.EnemyTarget != null)
        {
            attackZone.DealDamage();
        }
        else
        {
            ResetState();
        }
    }

    public void ResetState()
    {
        CurrentState = UnitStateEnum.Idle;
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private void ChaseEnemy(Vector2 targetPosition)
    {
        if (CurrentState == UnitStateEnum.Chasing)
        {
            return;
        }
        CurrentState = UnitStateEnum.Chasing;
        unitMove.MoveTo(targetPosition);

        if (camp != null)
        {
            camp.EnemyUnitSpotted(targetPosition);
        }
    }

    private void OnDestroy()
    {
        if (UnitEnumExtensions.IsPlayerUnit((UnitEnum)Unit.Id))
        {
            PlayerUnitsManager.Instance.RemoveUnit(gameObject);
        }
        else
        {
            EnemyUnitsManager.Instance.RemoveUnit(gameObject);
        }
    }
}
