using Assets.Scripts.Constants;
using UnityEngine;

public class UnitSight : MonoBehaviour
{
    [SerializeField] private float sightDistance;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Unit unit;

    private int framesPerSight = 4;
    void Update()
    {
        Sight();
    }

    private void Sight()
    {
        if (Time.frameCount % framesPerSight > 0)
        {
            return;
        }


        Collider2D enemyCollider = Physics2D.OverlapCircle(transform.position, sightDistance, enemyLayer);

        if (enemyCollider != null)
        {
            if (unit.IsChasingAvailable())
            {
                unit.MoveTo(enemyCollider.transform.position, MoveTypeEnum.ToEnemy);
            }
        }
    }
}
