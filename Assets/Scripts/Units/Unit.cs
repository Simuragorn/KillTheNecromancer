using Assets.Scripts.Constants;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private bool _isSelected;
    [SerializeField] private SpriteRenderer selectedImage;
    [SerializeField] private GameObject unitPosition;
    [SerializeField] private UnitMove unitMove;
    [SerializeField] private Animator animator;
    [SerializeField] private UnitAttackZone attackZone;

    public UnitStateEnum CurrentState { get; private set; }

    private void Awake()
    {
        SelectOrDeselect(false);
    }

    private void Start()
    {
        UnitsSelection.Instance.AllGameObjectUnits.Add(gameObject, this);
    }

    public void SelectOrDeselect(bool isSelected)
    {
        _isSelected = isSelected;
        selectedImage.enabled = _isSelected;
    }

    public bool IsChasingAvailable()
    {
        return CurrentState != UnitStateEnum.Chasing && 
            CurrentState != UnitStateEnum.Attacking;
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

    private void ChaseEnemy(Vector2 targetPosition)
    {
        if (CurrentState == UnitStateEnum.Chasing)
        {
            return;
        }
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        CurrentState = UnitStateEnum.Chasing;
        float attackDistance = Mathf.Min(attackZone.Widht, attackZone.Height) / 2;
        targetPosition -= direction * attackDistance;
        unitMove.MoveTo(targetPosition);
    }

    public void ResetState()
    {
        CurrentState = UnitStateEnum.Idle;
    }
}
