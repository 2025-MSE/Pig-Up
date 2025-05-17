using System.Collections.Generic;
using MSE.Core;
using UnityEditor;
using UnityEngine;

public class BuildingParser : EditorWindow
{
    private Building m_BuildingPrefab;

    [MenuItem("MSE/Parse Building")]
    public static void ShowWindow()
    {
        GetWindow<BuildingParser>("Building Parser");
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        m_BuildingPrefab = (Building)EditorGUILayout.ObjectField(m_BuildingPrefab, typeof(Building), false, GUILayout.Width(200));

        if (GUI.changed)
        {
            EditorUtility.SetDirty(m_BuildingPrefab);
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Parse"))
        {
            bool confirmed = EditorUtility.DisplayDialog(
                "Parser Alert",
                $"You are trying to parse building::{m_BuildingPrefab.name}",
                "Confirm",
                "Cancel");
            if (confirmed)
            {
                List<BuildingParsedData> parsedData = Parse();
                m_BuildingPrefab.ParsedDatas = parsedData.ToArray();

                EditorUtility.SetDirty(m_BuildingPrefab);

                if (!Application.isPlaying)
                {
                    AssetDatabase.SaveAssets();
                }
            }
        }
        if (GUILayout.Button("Generate"))
        {
            bool confirmed = EditorUtility.DisplayDialog(
                "Generate Alert",
                $"You are trying to generate building::{m_BuildingPrefab.name}",
                "Confirm",
                "Cancel");
            if (confirmed)
            {
                BuildingParsedData[] parsedData = m_BuildingPrefab.ParsedDatas;

                EditorUtility.SetDirty(m_BuildingPrefab);

                if (!Application.isPlaying)
                {
                    AssetDatabase.SaveAssets();
                }
            }
        }

        GUILayout.EndHorizontal();
    }

    private List<BuildingParsedData> Parse()
    {
        List<BuildingParsedData> parsedDatas = new List<BuildingParsedData>();

        foreach (Transform trans in m_BuildingPrefab.transform.GetChild(0).transform)
        {
            BuildingParsedData data = new BuildingParsedData();
            data.BlockIndex = trans.GetComponent<Block>().Index;
            data.Position = trans.position;
            data.Rotation = trans.rotation;

            parsedDatas.Add(data);
        }

        return parsedDatas;
    }
}
