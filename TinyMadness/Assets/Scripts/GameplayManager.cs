﻿using UnityEngine;
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
	public float		clock = 60.0f;
	public float		actualClock;

	void Start ()
	{
		Instance = this;
		scoreText.text = highScore.ToString();
		clockText.text = null;
	}

	void Update()
	{
		if(actualScore != score)
		{
			actualScore = score;
			scoreText.text = actualScore.ToString();
		}
		if(timeAttackGame)
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

	public void FailedSwipeOrEndObjLife()
	{
		if (timeAttackGame)
		{
			ResetScore();
			StartCoroutine(SpawnManager.Instance.CanSpawnCoroutine());
		}
		else
		{
			GameOver();
			CustomStopAllGameplayCoroutines();
		}
	}

	public void StartNewGame()
	{
		scoreText.text = actualScore.ToString();
		SpawnManager.Instance.canSpawn = true;
	}

	public void GameOver()
	{
		SpawnManager.Instance.canSpawn = false;
		timeAttackGame = false;
		survivalGame = false;

		if (actualScore > highScore)
		{
			highScore = actualScore;
		}
		
		scoreText.text = highScore.ToString();
		actualScore = 0;
		score = 0;
		clockText.text = null;

		if(SpawnManager.Instance.spawnedObj)
			SpawnManager.Instance.spawnedObj.GetComponent<Shape>().Kill();


		MenuManager.GetInstance().BringInGameOver();

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

	public void CustomStopAllGameplayCoroutines()
	{
		StopCoroutine("SpawnManager.Instance.CanSpawnCoroutine");
		StopCoroutine("SwipeManager.Instance.MoveInGameplayCoroutine");
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
