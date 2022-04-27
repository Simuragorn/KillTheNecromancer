using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SelectableUnit : MonoBehaviour
{
    private bool _isSelected;
    [SerializeField] private SpriteRenderer selectedImage;
    [SerializeField] private BaseUnitController _unit;
    public BaseUnitController Unit => _unit;
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

    private void OnDisable()
    {
        if (UnitsSelection.Instance.AllGameObjectUnits.ContainsKey(gameObject))
        {
            UnitsSelection.Instance.AllGameObjectUnits.Remove(gameObject);
        }
        if (UnitsSelection.Instance.SelectedUnits.Contains(this))
        {
            UnitsSelection.Instance.SelectedUnits.Remove(this);
        }
    }
}
