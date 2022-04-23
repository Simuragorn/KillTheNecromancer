using Pathfinding;
using UnityEngine;

public class UnitMove : MonoBehaviour
{
    private Vector2 targetPosition;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject gameObjectForFlip;
    [SerializeField] UnitController unit;

    //[SerializeField] private Seeker seeker;
    [SerializeField] private float nextPathPartDistance;


    //private Path currentPath;
    //private int pathPartIndex = 0;
    private bool pathEnded = true;

    public bool IsFlipped { private set; get; }

    public void MoveTo(Vector2 target)
    {
        targetPosition = target;
        pathEnded = false;
        //seeker.StartPath(transform.position, targetPosition, OnPathBuilded);
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

    //private void OnPathBuilded(Path path)
    //{
    //    if (!path.error)
    //    {
    //        pathEnded = false;
    //        currentPath = path;
    //        pathPartIndex = 0;
    //    }
    //}

    private void FixedUpdate()
    {
        //UpdatePath();
        if (pathEnded || unit.IsPositionFreezed)
        {
            animator.SetFloat("xMove", 0);
            return;
        }

        //targetPosition = currentPath.vectorPath[pathPartIndex];

        float xDirection = targetPosition.x - transform.position.x;
        CheckFlip(xDirection);
        pathEnded = (targetPosition == null ||
            Vector2.Distance(transform.position, targetPosition) <= nextPathPartDistance);
        Move();
    }

    //private void UpdatePath()
    //{
    //    float distance = Vector2.Distance(transform.position, targetPosition);
    //    if (distance <= nextPathPartDistance)
    //    {
    //        pathPartIndex++;
    //    }

    //    if (currentPath == null || pathPartIndex >= currentPath.vectorPath.Count)
    //    {
    //        pathEnded = true;
    //    }
    //}

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        var xLeading = (targetPosition - (Vector2)transform.position).x;

        animator.SetFloat("xMove", Mathf.Abs(xLeading));
    }
}
