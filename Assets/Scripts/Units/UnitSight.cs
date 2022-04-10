using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSight : MonoBehaviour
{
    [SerializeField] private float sightDistance;
    [SerializeField] private LayerMask enemy;
    [SerializeField] private Unit unit;
    void Update()
    {
        Sight();
    }

    private void Sight()
    {
        var enemyCollider = Physics2D.OverlapCircle(transform.position, sightDistance, enemy);

        if (enemyCollider != null)
        {
            unit.MoveToPosition(enemyCollider.transform.position, MoveTypeEnum.ToEnemy);
        }
    }
}
