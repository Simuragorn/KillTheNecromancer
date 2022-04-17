using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SelectableUnit : MonoBehaviour
{
    private bool _isSelected;
    [SerializeField] private SpriteRenderer selectedImage;
    [SerializeField] private UnitController _unit;
    public UnitController Unit => _unit;
    private void Awake()
    {
        SelectOrDeselect(false);
    }

    private void Start()
    {
        UnitsSelection.Instance.AllGameObjectUnits.Add(gameObject, this);
    }

    public void SelectOrDeselect(bool isSelected)
    {
        _isSelected = isSelected;
        selectedImage.enabled = _isSelected;
    }

    private void OnDestroy()
    {
        if (UnitsSelection.Instance.AllGameObjectUnits.ContainsKey(gameObject))
        {
            UnitsSelection.Instance.AllGameObjectUnits.Remove(gameObject);
        }
        if (UnitsSelection.Instance.selectedUnits.Contains(this))
        {
            UnitsSelection.Instance.selectedUnits.Remove(this);
        }
    }
}
