using Assets.Scripts.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitDropdown : MonoBehaviour
{
    [SerializeField] private Dropdown dropdown;
    [SerializeField] List<UnitWithSprite> units;
    void Start()
    {
        dropdown.ClearOptions();
        var unitOptions = new List<Dropdown.OptionData>();
        unitOptions = units.Select(u => new Dropdown.OptionData(u.sprite.name, u.sprite)).ToList();
        dropdown.AddOptions(unitOptions);

        StartCoroutine(SelectDefault());
    }

    public void OptionSelected(int index)
    {
        PlayerUnitsManager.Instance.OnSelectUnitForSpawn(units[index].unit);
    }

    private IEnumerator SelectDefault()
    {
        yield return new WaitForSeconds(1);
        OptionSelected(0);
    }
}

[Serializable]
public class UnitWithSprite
{
    public UnitEnum unit;
    public Sprite sprite;
}
