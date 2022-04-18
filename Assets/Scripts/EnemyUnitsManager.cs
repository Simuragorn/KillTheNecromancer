using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnitsManager : MonoBehaviour
{
    public static EnemyUnitsManager Instance { private set; get; }
    public Dictionary<GameObject, UnitController> Enemies { get; set; }
    public List<UnitsCamp> EnemiesCamps { get; set; }

    [SerializeField] private Text enemiesCampsLeftText;
    [SerializeField] private Text enemiesLeftText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        Enemies = new Dictionary<GameObject, UnitController>();
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
        if (Enemies.TryGetValue(unit, out UnitController controller))
        {
            Enemies.Remove(unit);
            EnemiesCamps.ForEach(c => c.RemoveUnit(controller));
            UpdateEnemiesText();
            CheckVictory();
        }
    }
    public void AddUnit(UnitController unit)
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

