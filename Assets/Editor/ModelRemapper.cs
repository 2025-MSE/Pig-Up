using UnityEditor;
using UnityEngine;

public class ModelRemapper : EditorWindow
{
    private string m_ModelFolderPath;

    [MenuItem("MSE/Remap Models' Materials")]
    public static void ShowWindow()
    {
        GetWindow<ModelRemapper>("Model Remapper");
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Models Directory", EditorStyles.boldLabel);

        if (GUILayout.Button("Find", GUILayout.MaxWidth(50)))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("Select Models Directory", "Assets", "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                m_ModelFolderPath = selectedPath;
            } else
            {
                Debug.LogWarning("[!] Selected directory has to be in Unity project folder.");
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(5));

        if (GUILayout.Button("Remap"))
        {
            bool confirmed = EditorUtility.DisplayDialog(
                "Remapper Alert",
                $"You are trying to remap models in {m_ModelFolderPath}.",
                "Confirm",
                "Cancel");

            if (confirmed)
            {
                RemapAll();
            }
        }
    }

    private void RemapAll()
    {
        string relativePath = m_ModelFolderPath.Replace(Application.dataPath, "Assets");
        string[] guids = AssetDatabase.FindAssets("t:Model", new[] { relativePath });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ModelImporter modelImporter = AssetImporter.GetAtPath(path) as ModelImporter;

            if (modelImporter != null)
            {
                modelImporter.SearchAndRemapMaterials(ModelImporterMaterialName.BasedOnTextureName, ModelImporterMaterialSearch.Everywhere);
                modelImporter.SaveAndReimport();
            }
        }
    }
}
