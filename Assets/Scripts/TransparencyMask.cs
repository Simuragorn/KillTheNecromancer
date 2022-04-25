using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyMask : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        BaseUnitController unit;
        if (!EnemyUnitsManager.Instance.Enemies.TryGetValue(collision.gameObject, out unit))
        {
            PlayerUnitsManager.Instance.PlayerUnits.TryGetValue(collision.gameObject, out unit);
        }
        if (unit != null)
        {
            unit.MakeTransparent();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BaseUnitController unit;
        if (!EnemyUnitsManager.Instance.Enemies.TryGetValue(collision.gameObject, out unit))
        {
            PlayerUnitsManager.Instance.PlayerUnits.TryGetValue(collision.gameObject, out unit);
        }
        if (unit != null)
        {
            unit.RemoveTransparency();
        }
    }
}
