using MSE.Core;
using System.Collections;
using System.IO;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;


public class BlockConverter : EditorWindow
{
    private string modelFolderPath;
    private string prefabFolderPath;

    [MenuItem("MSE/Convert to Blocks")]
    public static void ShowWindow()
    {
        GetWindow<BlockConverter>("Block Converter");
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Block Models Directory(Input)", EditorStyles.boldLabel);

        if (GUILayout.Button("Find", GUILayout.MaxWidth(50)))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("Select Models Directory", "Assets", "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                modelFolderPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
            } else
            {
                Debug.LogWarning("[!] Selected directory has to be in Unity project folder.");
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (modelFolderPath.IsNullOrEmpty())
        {
            GUILayout.Label("Please select directory.", EditorStyles.label);
        } else
        {
            GUILayout.Label(modelFolderPath, EditorStyles.label);
        }
        GUILayout.EndHorizontal();

        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(5));

        GUILayout.BeginHorizontal();
        GUILayout.Label("Block Prefabs Directory(Output)", EditorStyles.boldLabel);

        if (GUILayout.Button("Find", GUILayout.MaxWidth(50)))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("Select Prefabs Directory", "Assets", "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                prefabFolderPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
            } else
            {
                Debug.LogWarning("[!] Selected directory has to be in Unity project folder.");
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (prefabFolderPath.IsNullOrEmpty())
        {
            GUILayout.Label("Please select directory.", EditorStyles.label);
        } else
        {
            GUILayout.Label(prefabFolderPath, EditorStyles.label);
        }
        GUILayout.EndHorizontal();

        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(5));

        if (GUILayout.Button("Convert"))
        {
            bool confirmed = EditorUtility.DisplayDialog(
                "Converter Alert",
                $"You are trying to convert models in {modelFolderPath} into {prefabFolderPath}.",
                "Confirm",
                "Cancel");

            Debug.Log(confirmed);

            if (confirmed)
            {
                ConvertAll();
            }
        }
    }

    private void ConvertAll()
    {
        string[] models = Directory.GetFiles(modelFolderPath, "*.fbx", SearchOption.AllDirectories);

        int bindex = 0;
        foreach (string model in models)
        {
            Convert(model, bindex);
            bindex += 1;
        }
    }

    private void Convert(string modelPath, int index)
    {
        GameObject modelAsset = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
        GameObject modelObj = (GameObject)PrefabUtility.InstantiatePrefab(modelAsset);

        GameObject blockPrefObj = new GameObject(modelObj.name);
        modelObj.transform.SetParent(blockPrefObj.transform);

        Rigidbody rigidbody = blockPrefObj.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        NetworkObject nobj = blockPrefObj.AddComponent<NetworkObject>();
        nobj.DontDestroyWithOwner = true;
        Block blockComp = blockPrefObj.AddComponent<Block>();
        blockComp.Index = index;

        GameObject boundaryObj = new GameObject("Boundary");
        boundaryObj.transform.SetParent(blockPrefObj.transform);
        boundaryObj.AddComponent<BlockBoundary>();
        GameObject detectionObj = new GameObject("Detection");
        detectionObj.transform.SetParent(blockPrefObj.transform);
        detectionObj.AddComponent<BlockDetection>();
        GameObject detecteeObj = new GameObject("Detectee");
        detecteeObj.transform.SetParent(blockPrefObj.transform);

        BoxCollider ghboxCollider = modelObj.AddComponent<BoxCollider>();
        BoxCollider boxCollider = boundaryObj.AddComponent<BoxCollider>();
        BoxCollider dtboxCollider = detectionObj.AddComponent<BoxCollider>();
        BoxCollider dteboxCollider = detecteeObj.AddComponent<BoxCollider>();

        boxCollider.center = ghboxCollider.center;
        boxCollider.size = ghboxCollider.size;
        boxCollider.isTrigger = true;
        dtboxCollider.center = ghboxCollider.center;
        dtboxCollider.size = ghboxCollider.size;
        dtboxCollider.isTrigger = true;
        dteboxCollider.center = ghboxCollider.center;
        dteboxCollider.size = ghboxCollider.size * 0.8f;
        dteboxCollider.isTrigger = true;
        
        DestroyImmediate(ghboxCollider);

        boundaryObj.layer = LayerMask.NameToLayer("BlockBoundary");
        detectionObj.layer = LayerMask.NameToLayer("BlockDetection");
        detecteeObj.layer = LayerMask.NameToLayer("BlockDetectee");

        string prefabPath = $"{prefabFolderPath}/{modelObj.name}.prefab";
        PrefabUtility.SaveAsPrefabAsset(blockPrefObj, prefabPath);

        DestroyImmediate(blockPrefObj);
    }
}
