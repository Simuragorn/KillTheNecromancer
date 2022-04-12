using Assets.Scripts.Constants;
using UnityEngine;

public class UnitSight : MonoBehaviour
{
    [SerializeField] private float sightRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private UnitController unit;

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


        Collider2D enemyCollider = Physics2D.OverlapCircle(transform.position, sightRange, enemyLayer);

        if (enemyCollider != null)
        {
            if (unit.IsNewPathAvailable())
            {
                unit.MoveTo(enemyCollider.transform.position, MoveTypeEnum.ToEnemy);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
