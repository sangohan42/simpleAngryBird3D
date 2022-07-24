using System;
using UnityEngine;

[RequireComponent(typeof(PlaceOnARPlane))]
public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    private RandomLevelInstantiator randomLevelInstantiator;
    private PlaceOnARPlane placeOnARPlane;

    public Action<LevelSettings> OnLevelInstantiated { get; set; }

    public GameObject CurrentLevelGO { get; private set; }

    private void Awake()
    {
        // Check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, destroy other instances
            Destroy(gameObject);
        }

        Instance = this;

        randomLevelInstantiator = new RandomLevelInstantiator();
        placeOnARPlane = GetComponent<PlaceOnARPlane>();
    }

    public void InstantiateRandomLevel()
    {
        if (CurrentLevelGO != null)
            Destroy(CurrentLevelGO);

        var (levelGO, levelSettings) = randomLevelInstantiator.Instantiate(new Vector3(0, 0.08f, 0), Quaternion.identity);
        levelGO.SetActive(false);

        placeOnARPlane.ObjectToPlace = levelGO;
        placeOnARPlane.enabled = true;
        
        CurrentLevelGO = levelGO;

        OnLevelInstantiated?.Invoke(levelSettings);
    }
}