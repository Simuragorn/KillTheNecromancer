using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour
{
    private Vector2? targetPosition;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject gameObjectForFlip;
    [SerializeField] Unit unit;

    public bool isFlipped { private set; get; }

    public void ChaseEnemy(Vector2 target)
    {
        targetPosition = new Vector2(target.x, target.y);
    }

    public void MoveTo(Vector2 target)
    {
        targetPosition = new Vector2(target.x, target.y);
    }

    public void Flip()
    {
        isFlipped = !isFlipped;
        float xScale = isFlipped ? -1 : 1;
        gameObjectForFlip.transform.localScale = new Vector3(xScale, gameObjectForFlip.transform.localScale.y, gameObjectForFlip.transform.localScale.z);
    }

    private void Update()
    {
        if (targetPosition.HasValue)
        {
            Move();
        }
        else
        {
            animator.SetFloat("xMove", 0);
        }
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition.Value, speed * Time.deltaTime);
        var xLeading = (targetPosition.Value - (Vector2)transform.position).x;

        animator.SetFloat("xMove", Mathf.Abs(xLeading));
    }
}
