﻿using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance;
	private bool _alreadyInstantiated;

	private Action _lastGameModeLaunched;

	void Awake()
	{
		if(_instance != null)
			_alreadyInstantiated = true;
		else
			_instance = this;
	}

	void Start ()
	{
		if (_alreadyInstantiated) 
		{
			Destroy(gameObject);
			return;
		}

	}
	
	void Update ()
	{

	}

	public void StartCountDown()
	{
		StartCoroutine(CountDown());
	}

	IEnumerator CountDown(Action callBack = null)
	{
		yield return new WaitForSeconds(0.5f);
		for (int i = 0; i <= 3; i++)
		{
			MenuManager.GetInstance().ZoomInObject(MenuManager.GetInstance().countDownObject.transform.GetChild(i).gameObject, 0.5f);
			yield return new WaitForSeconds(0.5f);
		}

		if(callBack != null)
			callBack();
	}
	
	public void StartTimeAttackGame()
	{
		Debug.Log("Start Time Attack Game");
		MenuManager.GetInstance().bringOutPanel(MenuManager.GetInstance().activeMenuPanel);
		StartCoroutine(CountDown(GameplayManager.Instance.GoTimeAttackGame));
		_lastGameModeLaunched = StartTimeAttackGame;
		MenuManager.GetInstance().BringInUIShapes();
	}

	public void StartSurvivalGame()
	{
		Debug.Log("Start Survival Attack Game");
		MenuManager.GetInstance().bringOutPanel(MenuManager.GetInstance().activeMenuPanel);
		StartCoroutine(CountDown(GameplayManager.Instance.GoSurvivalGame));
		_lastGameModeLaunched = StartSurvivalGame;
		MenuManager.GetInstance().BringInUIShapes();
	}

	public void StartLastGameModeUsed()
	{
		if (_lastGameModeLaunched != null)
			_lastGameModeLaunched();
		else
			Debug.LogWarning("No last game mode found.");
	}

	public void Replay()
	{
		StartLastGameModeUsed();
	}

	public static GameManager GetInstance()
	{
		return _instance;
	}

}
