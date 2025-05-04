using MSE.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
public class StageData : ScriptableObject
{
    public string Name;
    public Building BuildingPrefab;
}
