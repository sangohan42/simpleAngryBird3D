using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    private int enemyToKillForVictory;
    private int enemyPointValue;
    private int killedEnemyCount = 0;

    [SerializeField] private Text birdsLeft;
    [SerializeField] private Text remainingEnemyToKill;
    [SerializeField] private Text plusScoreTxt;

    private int remainingBirdsCount;
    private int score; //Current score
    private int lastRecord; //Int that keeps current best score 
    private bool hitRecord; //Boolean that defines if we already hit last best score or not

    [SerializeField] private LevelController levelController;
    [SerializeField] private BirdThrower birdThrower;
    [SerializeField] private SoundController soundController;

    public event Action OnVictory;
    public event Action OnGameOver;

    private void OnEnable()
    {
        Bird.OnEnemyKilled += OnEnemyKilled;
        BirdThrower.OnFail += OnFailToKillEnemy;
        levelController.OnLevelInstantiated += OnLevelInstantiated;
    }

    private void OnDisable()
    {
        Bird.OnEnemyKilled -= OnEnemyKilled;
        BirdThrower.OnFail -= OnFailToKillEnemy;
        levelController.OnLevelInstantiated -= OnLevelInstantiated;
    }

    private void OnLevelInstantiated(LevelSettings levelSettings)
    {
        remainingBirdsCount = levelSettings.availableBirdsNumber;
        enemyToKillForVictory = levelSettings.enemyNumber;
        enemyPointValue = levelSettings.enemyPointValue;

        score = 0;
        birdsLeft.text = remainingBirdsCount.ToString();
        remainingEnemyToKill.text = enemyToKillForVictory.ToString();
        lastRecord = PlayerPrefs.GetInt("arcadeBestScore", 0);
    }

    private void OnEnemyKilled()
    {
        killedEnemyCount++;
        remainingEnemyToKill.text = (enemyToKillForVictory - killedEnemyCount).ToString();

        int scoreToAdd = Mathf.RoundToInt(birdThrower.ThrowingDistance * enemyPointValue);
        plusScoreTxt.gameObject.SetActive(true);
        plusScoreTxt.text = "+" + scoreToAdd.ToString("F0");

        AddScore(scoreToAdd);

        CheckGameState();
    }

    private void OnFailToKillEnemy()
    {
        //Debug.Log("FAIL");
        remainingBirdsCount -= 1;

        CheckGameState();
    }

    private void CheckGameState()
    {
        birdsLeft.text = remainingBirdsCount.ToString();

        //-----------------------
        // Victory or Game Over
        //-----------------------

        if (killedEnemyCount >= enemyToKillForVictory)
        {
            OnVictory?.Invoke();
        }
        else if (remainingBirdsCount == 0)
        {
            OnGameOver?.Invoke();
        }
    }

    private void AddScore(int score)
    {
        this.score += score;

        if (this.score > PlayerPrefs.GetInt("arcadeBestScore", 0))
        {
            PlayerPrefs.SetInt("arcadeBestScore", this.score);
            if (lastRecord > 0 && !hitRecord)
            {
                HitNewRecord();
            }
        }
    }

    private void HitNewRecord()
    {
        soundController.PlayNewRecord();
        hitRecord = true;
    }
}