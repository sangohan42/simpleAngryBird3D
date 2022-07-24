using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RandomLevelInstantiator
{
    private readonly LevelSettingsScriptableObject levelSettingsSO;

    public RandomLevelInstantiator()
    {
        levelSettingsSO = Resources.Load<LevelSettingsScriptableObject>("LevelSettings");
    }

    public (GameObject levelGO, LevelSettings levelSettings) Instantiate(Vector3 position, Quaternion rotation)
    {
        int randomLevelIndex = Random.Range(0, levelSettingsSO.levelSettings.Count);
        LevelSettings selectedLevel = levelSettingsSO.levelSettings[randomLevelIndex];
        GameObject levelGO = GameObject.Instantiate(selectedLevel.levelPrefab, position, rotation);

        return (levelGO, selectedLevel);
    }
}