using Assets.Scripts.Constants;
using UnityEngine;

public class UnitSight : MonoBehaviour
{
    private float sightRange;
    private UnitController unit;

    private int framesPerSight = 4;

    public void Init(UnitController controller)
    {
        unit = controller;
        sightRange = unit.Unit.SightRange;
    }
    void Update()
    {
        Sight();
    }

    private void Sight()
    {
        if (Time.frameCount % framesPerSight > 0 ||
            (unit.CurrentAction == UnitActionEnum.Attacking))
        {
            return;
        }

        Collider2D enemyCollider = Physics2D.OverlapCircle(transform.position, sightRange, unit.Unit.EnemyLayer);

        if (enemyCollider != null)
            unit.MoveTo(enemyCollider.transform.position, MoveTypeEnum.ToEnemy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
