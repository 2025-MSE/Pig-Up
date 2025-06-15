/// <summary>
/// Author: Dongjin Kuk
/// Description: The ScriptableObject that contains block prefabs.
/// </summary>

using MSE.Core;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "Scriptable Objects/BlockData")]
public class BlockData : ScriptableObject
{
    public List<Block> BlockPrefabs;
}
