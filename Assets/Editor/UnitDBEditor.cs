using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitDB))]
public class UnitDBEditor : Editor
{
    private UnitDB db;
    private void Awake()
    {
        db = (UnitDB)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+"))
        {
            db.CreateUnit();
        }
        if (GUILayout.Button("-"))
        {
            db.DeleteUnit();
        }
        if (GUILayout.Button("<="))
        {
            db.PrevUnit();
        }
        if (GUILayout.Button("=>"))
        {
            db.NextUnit();
        }
        GUILayout.EndHorizontal();
    }
}
