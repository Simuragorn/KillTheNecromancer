using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnitsManager : MonoBehaviour
{
    public static EnemyUnitsManager Instance { private set; get; }
    public Dictionary<GameObject, BaseUnitController> Enemies { get; set; }
    public List<UnitsCamp> EnemiesCamps { get; set; }

    [SerializeField] private TextMeshProUGUI enemiesCampsLeftText;
    [SerializeField] private TextMeshProUGUI enemiesLeftText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        Enemies = new Dictionary<GameObject, BaseUnitController>();
        EnemiesCamps = new List<UnitsCamp>();
    }

    public void RemoveCamp(UnitsCamp camp)
    {
        if (!EnemiesCamps.Contains(camp))
        {
            return;
        }
        EnemiesCamps.Remove(camp);
        UpdateCampText();
        PlayerUnitsManager.Instance.GetReward(camp.DestroyReward);
    }
    public void AddCamp(UnitsCamp camp)
    {
        if (!EnemiesCamps.Contains(camp))
        {
            EnemiesCamps.Add(camp);
            UpdateCampText();
        }
    }
    public void RemoveUnit(GameObject unit)
    {
        if (Enemies.TryGetValue(unit, out BaseUnitController controller))
        {
            Enemies.Remove(unit);
            EnemiesCamps.ForEach(c => c.RemoveUnit(controller));
            UpdateEnemiesText();
            CheckVictory();
            PlayerUnitsManager.Instance.GetReward(controller.Unit.Reward);
        }
    }
    public void AddUnit(BaseUnitController unit)
    {
        if (!Enemies.ContainsValue(unit))
        {
            Enemies.Add(unit.gameObject, unit);
            UpdateEnemiesText();
        }
    }
    private void UpdateCampText()
    {
        if (EnemiesCamps.Any())
        {
            enemiesCampsLeftText.text = $"{EnemiesCamps.Count} camps left";
        }
        else
        {
            enemiesCampsLeftText.text = "No camps left";
        }
    }
    private void UpdateEnemiesText()
    {
        if (Enemies.Any())
        {
            enemiesLeftText.text = $"{Enemies.Count} enemies left";
        }
        else
        {
            enemiesLeftText.text = "No enemies left";
        }
    }
    private void CheckVictory()
    {
        if (!Enemies.Any())
        {
            GameManager.Instance.Victory();
        }
    }
}

