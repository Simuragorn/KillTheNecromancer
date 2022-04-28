using Assets.Scripts.Constants;
using System;
using System.Collections;
using UnityEngine;

public class DistantUnitController : BaseUnitController
{

    public DistantTarget distantTargetMarker;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform projectileSpawn;
    private Vector2 targetPosition;

    private bool isReloading;
    protected void FixedUpdate()
    {
        if (!distantTargetMarker.isActiveAndEnabled)
            return;
        float actualDistance = Vector2.Distance(targetPosition, transform.position);
        if (actualDistance > Unit.DistantRange)
        {
            MoveTo(targetPosition, MoveTypeEnum.ToEnemy);
            distantTargetMarker.transform.position = targetPosition;
            return;
        }
        if (!isReloading)
        {
            StartAttack();
            StartCoroutine(StartReloading());
        }
    }

    public override void EnemySpotted(Vector2 enemyPosition)
    {
        distantTargetMarker.SetPosition(Unit.DistantSpotOffset, enemyPosition);
        targetPosition = distantTargetMarker.transform.position;
        if (camp != null)
            camp.EnemyUnitSpotted(enemyPosition);
    }

    public override void NoEnemySpotted()
    {
        base.NoEnemySpotted();
        distantTargetMarker.Hide();
    }

    public override void MoveTo(Vector2 targetPosition, MoveTypeEnum moveType)
    {
        if (moveType != MoveTypeEnum.ToEnemy)
            distantTargetMarker.Hide();
        base.MoveTo(targetPosition, moveType);
    }

    public override void ChangeOrder(UnitOrderEnum order)
    {
        if (order != UnitOrderEnum.DistantAttack)
            distantTargetMarker.Hide();
        base.ChangeOrder(order);
    }

    public override void StartAttack()
    {
        base.StartAttack();
        Vector2 fixedTargetPosition = targetPosition;
        float xDirection = targetPosition.x - transform.position.x;
        if (IsFlipped && xDirection > 0)
            Flip();
        else if (!IsFlipped && xDirection < 0)
            Flip();
        targetPosition = fixedTargetPosition;
        distantTargetMarker.transform.position = targetPosition;
    }
    public override void Attack()
    {
        if (distantTargetMarker.isActiveAndEnabled)
        {
            DistantAttack();
        }
    }

    public override void SetTarget(Vector2 newTargetPosition)
    {
        if (Unit.UnitType == UnitTypeEnum.Distant)
        {
            distantTargetMarker.SetPosition(Unit.DistantSpotOffset, newTargetPosition);
            targetPosition = newTargetPosition;
        }
    }

    protected void DistantAttack()
    {
        Projectile projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        Vector2 targetPosition = GetActualTargetPosition();
        projectile.Launch(targetPosition, Unit.ProjectileSpeed, Unit.EnemyLayer, Unit.Damage);
    }

    public override void Init(UnitsCamp unitCamp)
    {
        base.Init(unitCamp);
        distantTargetMarker.Hide();
    }

    private IEnumerator StartReloading()
    {
        isReloading = true;
        yield return new WaitForSeconds(Unit.ReloadInSeconds);
        isReloading = false;
    }

    private Vector2 GetActualTargetPosition()
    {
        float xOffset = UnityEngine.Random.Range(-Unit.DistantSpotOffset / 2f, Unit.DistantSpotOffset / 2f);
        float yOffset = UnityEngine.Random.Range(-Unit.DistantSpotOffset / 2f, Unit.DistantSpotOffset / 2f);
        Vector2 actualTargetPosition = new Vector2(targetPosition.x + xOffset, targetPosition.y + yOffset);
        return actualTargetPosition;
    }
}
