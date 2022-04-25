using Assets.Scripts.Constants;
using System.Collections;
using UnityEngine;

public abstract class BaseUnitController : MonoBehaviour
{
    [SerializeField] protected GameObject unitPosition;
    [SerializeField] protected UnitMove unitMove;
    [SerializeField] protected Animator animator;
    [SerializeField] protected UnitSight sight;
    [SerializeField] protected Health health;
    [SerializeField] protected UnitEnum unitId;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected float transparentValue;
    [SerializeField] protected float chasingDelayInSeconds;

    protected int _layer;

    public bool IsChasing { protected set; get; }
    public UnitEnum UnitId => unitId;

    protected UnitsCamp camp;

    protected bool _isMoveDisabled;

    public Unit Unit { protected set; get; }

    public UnitActionEnum CurrentAction { get; protected set; }
    public UnitOrderEnum CurrentOrder { get; protected set; }
    public bool IsFreeWay => CurrentAction != UnitActionEnum.Chasing &&
            CurrentAction != UnitActionEnum.Attacking;

    public bool IsMoveDisabled =>
        CurrentAction == UnitActionEnum.Attacking || _isMoveDisabled;

    public void Init(UnitsCamp unitCamp)
    {
        camp = unitCamp;
        OnSpawning();

        if (camp != null)
        {
            OnSpawned();
        }
    }

    protected virtual void OnSpawning()
    {
        unitMove.enabled = false;
        sight.enabled = false;
    }

    public virtual void OnSpawned()
    {
        CurrentAction = UnitActionEnum.Idle;
        unitMove.enabled = true;
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


    protected virtual void Start()
    {
        Unit = GameManager.Instance.GetUnitById(UnitId);
        _layer = 1 << gameObject.layer;
        if (IsPlayerUnit)
            PlayerUnitsManager.Instance.PlayerUnits.Add(gameObject, this);
        else
            EnemyUnitsManager.Instance.AddUnit(this);
        health.OnDeath += StartDeath;

        health.Init(Unit.Health);
        sight.Init(this);
    }

    public abstract void SetTarget(Vector2 targetPosition);

    public void GetDamage(int damage)
    {
        health.GetDamage(damage);
    }

    public bool IsPlayerUnit => _layer == GameManager.Instance.AllyLayer.value;

    public virtual void MoveTo(Vector2 targetPosition, MoveTypeEnum moveType)
    {
        CurrentAction = UnitActionEnum.Moving;
        targetPosition -= (Vector2)unitPosition.transform.localPosition;
        unitMove.MoveTo(targetPosition);
    }

    public void StartAttack()
    {
        CurrentAction = UnitActionEnum.Attacking;
        animator.SetTrigger("StartAttack");
    }

    public abstract void Attack();

    public void ResetState()
    {
        CurrentAction = UnitActionEnum.Idle;
    }

    public void DisableMove()
    {
        _isMoveDisabled = true;
    }

    public void EnableMove()
    {
        _isMoveDisabled = false;
    }

    protected void StartDeath()
    {
        if (IsPlayerUnit)
            PlayerUnitsManager.Instance.RemoveUnit(gameObject);
        else
            EnemyUnitsManager.Instance.RemoveUnit(gameObject);


        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        var scripts = gameObject.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
            script.enabled = false;

        animator.SetTrigger("Death");
    }
}