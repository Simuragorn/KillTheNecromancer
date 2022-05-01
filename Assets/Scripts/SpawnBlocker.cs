using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SpawnBlocker : MonoBehaviour
{
    [SerializeField] private float radius;
    private CircleCollider2D collider2D;

    private void Start()
    {
        collider2D = GetComponent<CircleCollider2D>();
        collider2D.radius = radius;
        EnemyUnitsManager.Instance.AddSpawnBlocker(this);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnDisable()
    {
        EnemyUnitsManager.Instance.RemoveSpawnBlocker(this);
    }
}
