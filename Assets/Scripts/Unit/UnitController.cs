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

    private int _layer;

    public DistantTarget distantTargetMarker;

    public bool IsChasing { private set; get; }
    public UnitEnum UnitId => unitId;

    private UnitsCamp camp;

    private bool _isMoveDisabled;

    public Unit Unit { private set; get; }

    public UnitActionEnum CurrentAction { get; private set; }
    public UnitOrderEnum CurrentOrder { get; private set; }
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

    public void SetDistantTarget(Vector2 targetPosition)
    {
        if (Unit.UnitType == UnitTypeEnum.Distant)
        {
            distantTargetMarker.SetPosition(Unit.DistantSpotOffset, targetPosition);
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
        Unit = GameManager.Instance.GetUnitById(UnitId);
        _layer = 1 << gameObject.layer;
        if (IsPlayerUnit)
        {
            PlayerUnitsManager.Instance.PlayerUnits.Add(gameObject, this);
        }
        else
        {
            EnemyUnitsManager.Instance.AddUnit(this);
        }
        health.OnDeath += StartDeath;

        attackZone.Init(this);
        health.Init(Unit.Health);
        sight.Init(this);

        if (Unit.UnitType == UnitTypeEnum.Distant)
            distantTargetMarker.Hide();
    }

    public void GetDamage(int damage)
    {
        health.GetDamage(damage);
    }

    public bool IsPlayerUnit => _layer == GameManager.Instance.AllyLayer.value;

    public void MoveTo(Vector2 targetPosition, MoveTypeEnum moveType)
    {
        if (Unit.UnitType == UnitTypeEnum.Distant)
            distantTargetMarker.Hide();

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

    public void DisableMove()
    {
        _isMoveDisabled = true;
    }

    public void EnableMove()
    {
        _isMoveDisabled = false;
    }

    private void StartDeath()
    {
        if (IsPlayerUnit)
        {
            PlayerUnitsManager.Instance.RemoveUnit(gameObject);
        }
        else
        {
            EnemyUnitsManager.Instance.RemoveUnit(gameObject);
        }


        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        var scripts = gameObject.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
            script.enabled = false;

        animator.SetTrigger("Death");
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
}
