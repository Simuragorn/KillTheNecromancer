using Assets.Scripts.Constants;
using System.Collections;
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
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float transparentValue;
    [SerializeField] private float chasingDelayInSeconds;
    public bool IsChasing { private set; get; }
    public UnitEnum UnitId => unitId;

    private UnitsCamp camp;

    public Unit Unit { private set; get; }

    public UnitActionEnum CurrentAction { get; private set; }
    public UnitOrderEnum CurrentOrder { get; private set; }
    public bool IsFreeWay => CurrentAction != UnitActionEnum.Chasing &&
            CurrentAction != UnitActionEnum.Attacking;

    public bool IsPositionFreezed => CurrentAction == UnitActionEnum.Attacking;

    public void Init(UnitsCamp unitCamp)
    {
        camp = unitCamp;
        OnSpawning();

        if (camp != null)
        {
            OnSpawned();
        }
    }

    private void OnSpawning()
    {
        unitMove.enabled = false;
        attackZone.enabled = false;
        sight.enabled = false;
    }

    public void OnSpawned()
    {
        CurrentAction = UnitActionEnum.Idle;
        unitMove.enabled = true;
        attackZone.enabled = true;
        sight.enabled = true;
    }

    public void ChangeOrder(UnitOrderEnum order)
    {
        CurrentOrder = order;
        switch (CurrentOrder)
        {
            case UnitOrderEnum.Attack:
                sight.enabled = true;
                break;
            case UnitOrderEnum.Defense:
                sight.enabled = false;
                CurrentAction = UnitActionEnum.Idle;
                break;
            default:
                break;
        }
    }

    public void MakeTransparent()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, transparentValue);
    }

    public void RemoveTransparency()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
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
            CurrentAction = UnitActionEnum.Moving;
            targetPosition -= (Vector2)unitPosition.transform.localPosition;
            unitMove.MoveTo(targetPosition);
        }
    }

    public void StartAttack()
    {
        CurrentAction = UnitActionEnum.Attacking;
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
        CurrentAction = UnitActionEnum.Idle;
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private void ChaseEnemy(Vector2 targetPosition)
    {
        if (IsChasing || CurrentAction == UnitActionEnum.Attacking)
            return;

        CurrentAction = UnitActionEnum.Chasing;
        unitMove.MoveTo(targetPosition);

        if (camp != null)
        {
            camp.EnemyUnitSpotted(targetPosition);
        }
        StartCoroutine(StartChasingDelay());
    }

    private IEnumerator StartChasingDelay()
    {
        IsChasing = true;
        yield return new WaitForSeconds(chasingDelayInSeconds);
        IsChasing = false;
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
