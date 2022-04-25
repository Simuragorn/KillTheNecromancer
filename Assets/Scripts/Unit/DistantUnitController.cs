using Assets.Scripts.Constants;
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

    public override void Attack()
    {
        if (distantTargetMarker.isActiveAndEnabled)
        {
            Projectile projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
            projectile.Launch(distantTargetMarker.transform.position, Unit.ProjectileSpeed, Unit.EnemyLayer, Unit.Damage);
        }
    }
    public override void SetTarget(Vector2 targetPosition)
    {
        if (Unit.UnitType == UnitTypeEnum.Distant)
        {
            distantTargetMarker.SetPosition(Unit.DistantSpotOffset, targetPosition);
        }
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
}
