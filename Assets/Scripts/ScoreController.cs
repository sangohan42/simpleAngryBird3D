using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (GameController))]
public class ScoreController : MonoBehaviour 
{	
    public int startBirdsCount = 5;				//Birds count we start with (decrease only when we fail to destroy an enemy)
	public int enemyToKillForVictory = 3;
	private int currentEnemyKilledCount = 0;
	private int xpScoreStep = 100;					//XP step. With help of this you can tweak the speed of getting xp level.

	public Text birdsLeft;
    public Text remainingEnemyToKill;
	public Text plusScoreTxt;
	public Text plusBallTxt;
	
	private int currentBirdsCount;					
	private int score;								//Current score
	private int lastRecord;							//Int that keeps current best score 
	private bool hitRecord;							//Boolean that defines if we already hitted last best score or not
	
	
	void OnEnable()
	{
        BirdControl.OnEnemyKilled += EnemyKilled;
        BirdControl.OnFail += Fail;
	}
	
	void OnDisable()
	{
        BirdControl.OnEnemyKilled -= EnemyKilled;
        BirdControl.OnFail -= Fail;
	}
	
	void Start()
	{
		currentBirdsCount = startBirdsCount;
		ResetData();
	}
	
    void EnemyKilled( float fromDistance, int enemyPointValue = 10 )
	{
        currentEnemyKilledCount++;
        remainingEnemyToKill.text = (enemyToKillForVictory - currentEnemyKilledCount).ToString();
		
        int scoreToAdd = (int)fromDistance * enemyPointValue;
        plusScoreTxt.gameObject.SetActive(true);
        plusScoreTxt.text = "+" + scoreToAdd.ToString("F0");
        AddScore(scoreToAdd);

        CheckGameState();
	}
	
	void Fail()
	{
        Debug.Log("FAIL");
        currentBirdsCount -= 1;
			
		CheckGameState();
	}

    void CheckGameState()
    {
        birdsLeft.text = currentBirdsCount.ToString();

        //-----------------------
        // Victory or Game Over
        //-----------------------

        if (currentEnemyKilledCount >= enemyToKillForVictory)
        {
            GameController.Instance.Victory();
        }
        else if (currentBirdsCount < 1)
        {
            GameController.Instance.GameOver();
        }
    }
	
	public void AddScore(int score) 
	{
		this.score += score;

		if(this.score > PlayerPrefs.GetInt("arcadeBestScore",0))
		{
			PlayerPrefs.SetInt("arcadeBestScore",this.score);
			if(lastRecord > 0 && !hitRecord) 
			{
				HitNewRecord();
			}
		}
	}
	
	public void HitNewRecord()
	{
        SoundController.Instance.playNewRecord();
		hitRecord = true;
	}
	
	public void ResetData()
	{
		score = 0;
		currentBirdsCount = startBirdsCount;
		birdsLeft.text = currentBirdsCount.ToString();
        remainingEnemyToKill.text = enemyToKillForVictory.ToString();
		lastRecord = PlayerPrefs.GetInt("arcadeBestScore",0);
	}

}