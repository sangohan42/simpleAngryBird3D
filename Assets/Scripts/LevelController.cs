using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class LevelController : MonoBehaviour {

    public List<LevelSettings> levels = new List<LevelSettings>();
    public Transform levelParent;
    public ScoreController scoreController;
    public UnityARHitTest HitTest;

    private GameObject currentLevelGO;

    // Choose the level randomly (currently only one level so..)
	public void SelectAndInstantiateRandomLevel()
    {
        int randomLevelIndex = Random.Range(0, levels.Count);
        LevelSettings selectedLevel = levels[randomLevelIndex];
        currentLevelGO = Instantiate(selectedLevel.levelPrefab, new Vector3(0,0.08f,0), Quaternion.identity) as GameObject;
        currentLevelGO.transform.SetParent(levelParent);
        currentLevelGO.SetActive(false);

        // Give the GO to the HitTest script
        HitTest.Level = currentLevelGO;

        // Set displayed values
        scoreController.SetStartValues(selectedLevel);
    }

    // not really necessary because in this game I reset the scene so everything is already clean up for me
    public void DestroyLevel()
    {
        Destroy(currentLevelGO);
    }
}
