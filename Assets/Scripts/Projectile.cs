using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool isLaunched;
    private Vector2 targetPosition;
    private float speed;
    private LayerMask enemyMask;
    private int damage;

    private float targetMinDistance = 0.1f;

    public void Launch(Vector2 targetPos, float startSpeed, LayerMask enemyLayerMask, int damageValue)
    {
        targetPosition = targetPos;
        speed = startSpeed;
        enemyMask = enemyLayerMask;
        damage = damageValue;

        isLaunched = true;

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FixedUpdate()
    {
        if (isLaunched)
            Move();
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < targetMinDistance)
            Destroy(gameObject);
    }
}
