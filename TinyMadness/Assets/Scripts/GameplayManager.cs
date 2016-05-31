using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
	public static GameplayManager Instance;

	public GameObject[] shapes;
	public Text			scoreText;
	public Text			clockText;
	public float		score = 0;
	public float		actualScore = 0;
	public float		highScore = 0;
	public bool			survivalGame = false;
	public bool			timeAttackGame = false;
	public float		clock = 500.0f;
	public float		actualClock;

	void Start ()
	{
		Instance = this;
		scoreText.text = actualScore.ToString();
		clockText.text = actualClock.ToString("0.00");
	}

	void Update()
	{
		if(actualScore != score)
		{
			actualScore = score;
			scoreText.text = actualScore.ToString();
		}
		clockText.text = actualClock.ToString("0.00");
	}

	public void GoTimeAttackGame()
	{
		timeAttackGame = true;
		StartNewGame();
		StartCoroutine(ReduceClock());
	}

	public void GoSurvivalGame()
	{
		survivalGame = true;
		StartNewGame();
	}

	public void StartNewGame()
	{
		SpawnManager.Instance.canSpawn = true;
	}

	public void GameOver()
	{
		SpawnManager.Instance.canSpawn = false;
		timeAttackGame = false;
		survivalGame = false;

		if(actualScore > highScore)
		{
			actualScore = highScore;
		}

		//Go Menu window Ou Game Over window 
	}

	public void AddScore()
	{
		score += 1;
	}

	public void ReduceScore()
	{
		score -= 1;
	}

	public void ResetScore()
	{
		score = 0;
	}

	IEnumerator ReduceClock()
	{
		actualClock = clock;
		while (actualClock > 0)
		{
			actualClock -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		actualClock = 0;
		GameOver();
		StopCoroutine("ReduceClock");
	}
}
