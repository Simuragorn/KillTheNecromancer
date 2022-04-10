using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
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

    public void MoveToPosition(Vector2 targetPosition, MoveTypeEnum moveType)
    {

        targetPosition -= (Vector2)unitPosition.transform.localPosition;
        CurrentState = UnitStateEnum.Moving;
        if (moveType == MoveTypeEnum.ToEnemy)
        {
            ChaseEnemy(targetPosition);
        }
        else
        {
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
        attackZone.DealDamage();
        if (attackZone.EnemyTarget != null)
        {
            animator.SetTrigger("StartAttack");
        }
        else
        {
            ResetState();
        }
    }

    private void ChaseEnemy(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;

        if (targetPosition.x < transform.position.x && !unitMove.isFlipped)
        {
            unitMove.Flip();
        }
        else if (targetPosition.x > transform.position.x && unitMove.isFlipped)
        {
            unitMove.Flip();
        }
        CurrentState = UnitStateEnum.Chasing;
        targetPosition += (Vector2)attackZone.transform.localPosition;
        unitMove.MoveTo(targetPosition);
    }

    private void ResetState()
    {
        CurrentState = UnitStateEnum.Idle;
    }
}
