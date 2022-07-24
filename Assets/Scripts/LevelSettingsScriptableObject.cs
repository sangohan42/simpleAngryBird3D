using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/LevelSettingsScriptableObject", order = 1)]
public class LevelSettingsScriptableObject : ScriptableObject
{
    public List<LevelSettings> levelSettings;
}