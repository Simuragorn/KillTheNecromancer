using UnityEngine;

public class UnitMove : MonoBehaviour
{
    private Vector2 targetPosition;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject gameObjectForFlip;
    [SerializeField] UnitController unit;

    [SerializeField] private float nextPathPartDistance;

    private bool pathEnded = true;

    public bool IsFlipped { private set; get; }

    public void MoveTo(Vector2 target)
    {
        targetPosition = target;
        pathEnded = false;
    }

    private void Flip()
    {
        IsFlipped = !IsFlipped;
        float xScale = IsFlipped ? -1 : 1;
        gameObjectForFlip.transform.localScale = new Vector3(xScale, gameObjectForFlip.transform.localScale.y, gameObjectForFlip.transform.localScale.z);
    }

    private void CheckFlip(float xDirection)
    {
        if (xDirection < 0 && !IsFlipped)
        {
            Flip();
        }
        else if (xDirection > 0 && IsFlipped)
        {
            Flip();
        }
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
        Move();
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        var xLeading = (targetPosition - (Vector2)transform.position).x;

        animator.SetFloat("xMove", Mathf.Abs(xLeading));
    }
}
