using Assets.Scripts.Constants;
using UnityEngine;
using UnityEngine.UI;

public class FormationButton : MonoBehaviour
{
    [SerializeField] private UnitFormationEnum formation;
    [SerializeField] private Button button;
    [SerializeField] private GameObject formationActiveObject;
    [SerializeField] private KeyCode activationButton;

    private void Awake()
    {
        formationActiveObject.SetActive(false);
        button.onClick.AddListener(ChangeFormation);
        UnitsSelection.Instance.OnFormationChanged += OnFormationChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(activationButton))
            ChangeFormation();
    }

    private void ChangeFormation()
    {
        UnitsSelection.Instance.ChangeFormation(formation);
    }

    private void OnFormationChanged(UnitFormationEnum currentFormation)
    {
        formationActiveObject.SetActive(currentFormation == formation);
    }
}
