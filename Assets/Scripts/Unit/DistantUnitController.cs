using Assets.Scripts.Constants;
using System;
using System.Collections;
using UnityEngine;

public class DistantUnitController : BaseUnitController
{

    public DistantTarget distantTargetMarker;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform projectileSpawn;

    private bool isReloading;
    protected void FixedUpdate()
    {
        if (!isReloading && distantTargetMarker.isActiveAndEnabled)
        {
            StartAttack();
            StartCoroutine(StartReloading());
        }
    }

    public override void MoveTo(Vector2 targetPosition, MoveTypeEnum moveType)
    {
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
        Vector2 targetPosition = distantTargetMarker.transform.position;
        float xDirection = distantTargetMarker.transform.position.x - transform.position.x;
        if (IsFlipped && xDirection > 0)
            Flip();
        else if (!IsFlipped && xDirection < 0)
            Flip();
        distantTargetMarker.transform.position = targetPosition;
    }
    public override void Attack()
    {
        if (distantTargetMarker.isActiveAndEnabled)
            DistantAttack();
    }

    public override void SetTarget(Vector2 targetPosition)
    {
        if (Unit.UnitType == UnitTypeEnum.Distant)
        {
            distantTargetMarker.SetPosition(Unit.DistantSpotOffset, targetPosition);
        }
    }

    protected void DistantAttack()
    {
        Projectile projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        Vector2 targetPosition = GetActualTargetPosition();
        projectile.Launch(targetPosition, Unit.ProjectileSpeed, Unit.EnemyLayer, Unit.Damage);
    }

    protected override void Start()
    {
        base.Start();
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
        Vector2 targetPosition = new Vector2(distantTargetMarker.transform.position.x + xOffset, distantTargetMarker.transform.position.y + yOffset);
        return targetPosition;
    }
}
