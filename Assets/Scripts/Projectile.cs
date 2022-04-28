using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool isLaunched;
    private Vector2 targetPosition;
    private float speed;
    private LayerMask enemyMask;
    private int damage;
    private Vector2 launchPosition;

    private float targetMinDistance = 0.1f;

    public void Launch(Vector2 targetPos, float startSpeed, LayerMask enemyLayerMask, int damageValue)
    {
        launchPosition = transform.position;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!isLaunched || 1 << collision.gameObject.layer != enemyMask)
            return;

        bool isPlayerUnit = GameManager.Instance.AllyLayer == enemyMask;
        BaseUnitController unit;
        if (isPlayerUnit)
            PlayerUnitsManager.Instance.PlayerUnits.TryGetValue(collision.gameObject, out unit);
        else
            EnemyUnitsManager.Instance.Enemies.TryGetValue(collision.gameObject, out unit);

        if (unit != null)
        {
            unit.GetDamage(damage, launchPosition);
            StartDestroy();
        }
    }

    private void StartDestroy()
    {
        isLaunched = false;
        animator.SetTrigger("OnDestroy");
    }

    public void OnDestoyed()
    {
        Destroy(gameObject);
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < targetMinDistance)
            StartDestroy();
    }
}
