using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour
{
    private Vector2? targetPosition;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject gameObjectForFlip;

    [SerializeField] float maxRandomOffset;

    public void SetTarget(Vector2 target)
    {
        float offsetX = Random.Range(0, maxRandomOffset);
        float offsetY = Random.Range(0, maxRandomOffset);
        targetPosition = new Vector2(target.x + offsetX, target.y + offsetY);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (targetPosition.HasValue)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition.Value, speed * Time.deltaTime);
            var xLeading = (targetPosition.Value - (Vector2)transform.position).x;

            float xScale = gameObjectForFlip.transform.localScale.x;
            if (xScale > 0 && xLeading < 0)
            {
                xScale = -1;
            }
            if (xScale < 0 && xLeading > 0)
            {
                xScale = 1;
            }
            gameObjectForFlip.transform.localScale = new Vector3(xScale, gameObjectForFlip.transform.localScale.y, gameObjectForFlip.transform.localScale.z);
            animator.SetFloat("xMove", Mathf.Abs(xLeading));
        }
        else
        {
            animator.SetFloat("xMove", 0);
        }
    }
}
