using Assets.Scripts.Constants;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitSight : MonoBehaviour
{
    private float sightRange;
    private BaseUnitController unit;

    private int framesPerSight = 4;

    public void Init(BaseUnitController controller)
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
        if (Time.frameCount % framesPerSight > 0 || sightRange == 0)
        {
            return;
        }
        var filter = new ContactFilter2D { layerMask = unit.Unit.EnemyLayer, useLayerMask = true, useTriggers = true };
        var colliderResults = new List<Collider2D>();
        Physics2D.OverlapCircle(transform.position, sightRange, filter, colliderResults);

        if (colliderResults.Any())
            unit.EnemySpotted(colliderResults[0].transform.position);
        else
            unit.NoEnemySpotted();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
