using System;
using UnityEngine;

public class UnitMove : MonoBehaviour
{
    private Vector2 targetPosition;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] BaseUnitController unit;

    [SerializeField] private float nextPathPartDistance;

    private bool pathEnded = true;
    public Action OnPathEnded;

    public void MoveTo(Vector2 target)
    {
        targetPosition = target;
        pathEnded = false;
    }

    private void CheckFlip(float xDirection)
    {
        if (xDirection < 0 && !unit.IsFlipped)
            unit.Flip();
        else if (xDirection > 0 && unit.IsFlipped)
            unit.Flip();
    }

    private void FixedUpdate()
    {
        if (pathEnded || unit.IsMoveDisabled)
        {
            animator.SetFloat("xMove", 0);
            return;
        }

        float xDirection = targetPosition.x - transform.position.x;
        CheckFlip(xDirection);
        pathEnded = (targetPosition == null ||
            Vector2.Distance(transform.position, targetPosition) <= nextPathPartDistance);
        if (pathEnded && OnPathEnded != null)
            OnPathEnded();

        Move();
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        var xLeading = (targetPosition - (Vector2)transform.position).x;

        animator.SetFloat("xMove", Mathf.Abs(xLeading));
    }
}
