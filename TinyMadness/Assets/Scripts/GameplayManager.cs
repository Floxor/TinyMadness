using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
	public static GameplayManager Instance;

	public GameObject[] shapes;
	public Text			scoreText;
	public float		score = 0;
	public float		actualScore = 0;
	public float		highScore = 0;
	public bool			survivalGame = false;
	public bool			timeAttackGame = false;
	public float		clock;
	public float		actualClock;

	void Start ()
	{
		Instance = this;
		scoreText.text = actualScore.ToString();
	}

	void Update()
	{
		if(actualScore != score)
		{
			actualScore = score;
			scoreText.text = actualScore.ToString();
		}
	}

	public void GoTimeAttackGame()
	{
		timeAttackGame = true;
		StartCoroutine(ReduceClock());
	}

	public void GoSurvivalGame()
	{
		survivalGame = true;
	}

	public void StartNewGame()
	{
		//Transistion ?
		//Décompte ?
		SpawnManager.Instance.canSpawn = true;
	}

	public void GameOver()
	{
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
			actualClock.ToString("0.00");
			yield return new WaitForEndOfFrame();
		}
		GameOver();
		StopCoroutine("ReduceClock");
	}
}
