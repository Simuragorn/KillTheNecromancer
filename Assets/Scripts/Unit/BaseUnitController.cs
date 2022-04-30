using Assets.Scripts.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private List<OrderSprite> orderSprites;
    [SerializeField] private SpriteRenderer orderSpriteRenderer;

    [SerializeField] private int backToSpawnDelayInSeconds;
    public bool IsDead { private set; get; }

    protected int _layer;

    public bool IsChasing { protected set; get; }
    public UnitEnum UnitId => unitId;
    public Unit Unit { private set; get; }

    protected UnitsCamp camp;

    protected bool _isMoveDisabled;

    private bool isInited;

    public UnitActionEnum CurrentAction { get; protected set; }
    public UnitOrderEnum CurrentOrder { get; protected set; }
    public bool IsFreeWay => CurrentAction != UnitActionEnum.Chasing &&
            CurrentAction != UnitActionEnum.Attacking;

    public bool IsMoveDisabled =>
        CurrentAction == UnitActionEnum.Attacking || _isMoveDisabled;

    public bool IsFlipped { private set; get; }

    public virtual void Init(UnitsCamp unitCamp)
    {
        camp = unitCamp;
        StartSpawn();

        if (camp != null)
        {
            OnSpawned();
            unitCamp.Units.Add(this);
        }

        Unit = GameManager.Instance.GetUnitById(UnitId);
        _layer = 1 << gameObject.layer;
        if (IsPlayerUnit)
            PlayerUnitsManager.Instance.PlayerUnits.Add(gameObject, this);
        else
            EnemyUnitsManager.Instance.AddUnit(this);
        health.OnDeath += StartDeath;

        health.Init(Unit.Health);
        sight.Init(this);
        isInited = true;
    }

    public void Flip()
    {
        IsFlipped = !IsFlipped;
        float xScale = IsFlipped ? -1 : 1;
        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
    }

    private void Start()
    {
        if (!isInited)
            Init(null);
    }

    public virtual void StartSpawn()
    {
        StartCoroutine(Spawning());
    }

    protected virtual IEnumerator Spawning()
    {
        unitMove.enabled = false;
        sight.enabled = false;
        yield return new WaitForSeconds(1);
        if (!IsDead)
            OnSpawned();
    }

    protected virtual void OnSpawned()
    {
        CurrentAction = UnitActionEnum.Idle;
        unitMove.enabled = true;
        sight.enabled = true;
    }

    public virtual void ChangeOrder(UnitOrderEnum order)
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
        Sprite orderSprite = orderSprites.FirstOrDefault(o => o.Order == order).Sprite;
        orderSpriteRenderer.sprite = orderSprite;
    }

    public void MakeTransparent()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, transparentValue);
    }

    public void RemoveTransparency()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
    }

    public abstract void SetTarget(Vector2 targetPosition);

    public virtual void GetDamage(int damage, Vector2? damagedFromPosition = null)
    {
        if (IsBlocked())
            animator.SetTrigger("Block");
        else
            health.GetDamage(damage);
        Stun();
        if (CurrentOrder == UnitOrderEnum.Attack && damagedFromPosition.HasValue)
            MoveTo(damagedFromPosition.Value, MoveTypeEnum.ToEnemy);
    }

    private bool IsBlocked()
    {
        int actualPercentage = UnityEngine.Random.Range(0, 101);
        return actualPercentage <= Unit.BlockChancePercentage;
    }

    public bool IsPlayerUnit => _layer == GameManager.Instance.AllyLayer.value;

    public virtual void EnemySpotted(Vector2 enemyPosition)
    {
        MoveTo(enemyPosition, MoveTypeEnum.ToEnemy);
    }

    public virtual void NoEnemySpotted()
    {
        if (CurrentAction == UnitActionEnum.Attacking ||
            CurrentAction == UnitActionEnum.Chasing)
            ResetState();
    }

    public virtual void MoveTo(Vector2 targetPosition, MoveTypeEnum moveType)
    {
        CurrentAction = UnitActionEnum.Moving;
        targetPosition -= (Vector2)unitPosition.transform.localPosition;
        unitMove.MoveTo(targetPosition, moveType);
    }

    public virtual void StartAttack()
    {
        CurrentAction = UnitActionEnum.Attacking;
        animator.SetTrigger("StartAttack");
    }

    public abstract void Attack();

    public void ResetState()
    {
        if (IsDead)
            return;

        CurrentAction = UnitActionEnum.Idle;
        if (camp != null)
            StartCoroutine(StartReturningToCamp());
    }

    public void Stun()
    {
        StartCoroutine(StartStun());
    }

    private IEnumerator StartStun()
    {
        _isMoveDisabled = true;
        yield return new WaitForSeconds(Unit.StunInSeconds);
        _isMoveDisabled = false;
    }

    private IEnumerator StartReturningToCamp()
    {
        yield return new WaitForSeconds(backToSpawnDelayInSeconds);
        if (camp != null && IsDead)
        {
            float xOffset = UnityEngine.Random.Range(-camp.CampRadius, camp.CampRadius);
            float yOffset = UnityEngine.Random.Range(-camp.CampRadius, camp.CampRadius);
            Vector2 targetPosition = new Vector2(camp.transform.position.x + xOffset,
                camp.transform.position.y + yOffset);
            MoveTo(targetPosition, MoveTypeEnum.ToPosition);
        }
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
        IsDead = true;
    }
}

[Serializable]
public class OrderSprite
{
    public UnitOrderEnum Order;
    public Sprite Sprite;
}
