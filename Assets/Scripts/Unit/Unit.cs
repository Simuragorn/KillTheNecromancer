using Assets.Scripts.Constants;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private GameObject unitPosition;
    [SerializeField] private UnitMove unitMove;
    [SerializeField] private Animator animator;
    [SerializeField] private UnitAttackZone attackZone;

    [SerializeField] private Health health;

    public UnitStateEnum CurrentState { get; private set; }


    private void Start()
    {
        int currentLayer = GameManager.Instance.EnemyLayer | (1 << gameObject.layer);
        if (currentLayer == GameManager.Instance.EnemyLayer)
        {
            EnemiesController.Instance.Enemies.Add(gameObject, this);
        }
        health.OnDeath += Death;
    }

    public bool IsNewPathAvailable()
    {
        return CurrentState != UnitStateEnum.Chasing &&
            CurrentState != UnitStateEnum.Attacking;
    }

    public bool IsUnitPositionFreezed()
    {
        return CurrentState == UnitStateEnum.Attacking;
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
            animator.SetTrigger("StartAttack");
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
    }
}
