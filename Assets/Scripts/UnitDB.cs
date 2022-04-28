using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit DB", menuName = "Databases/Unit")]
public class UnitDB : ScriptableObject
{
    [SerializeField, HideInInspector] private List<Unit> units;
    [SerializeField] private Unit currentUnit;
    private int currentIndex;

    public void CreateUnit()
    {
        if (units == null)
        {
            units = new List<Unit>();
        }
        var unit = new Unit();
        units.Add(unit);
        currentIndex = units.Count - 1;
        currentUnit = units[currentIndex];
    }

    public void NextUnit()
    {
        if (currentIndex < units.Count - 1)
        {
            currentIndex++;
            currentUnit = units[currentIndex];
        }
    }

    public void PrevUnit()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            currentUnit = units[currentIndex];
        }
    }

    public void DeleteUnit()
    {
        if (units == null)
        {
            units = new List<Unit>();
            return;
        }
        units.Remove(currentUnit);
        if (units.Count > 0)
        {
            currentUnit = units[0];
        }
        else
        {
            CreateUnit();
        }
        currentIndex = 0;
    }

    public Unit GetUnitById(UnitEnum unitId)
    {
        return units.FirstOrDefault(u => u.Id == unitId);
    }
}
