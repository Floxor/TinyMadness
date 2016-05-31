using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance;
	private bool _alreadyInstantiated;

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

	public void GotoMainMenu()
	{
		Debug.Log("Going back to mainMenu");
	}

	public void LaunchGameOver()
	{
		Debug.Log("GAME LOST !");
	}

	public void StartCountDown()
	{
		StartCoroutine(CountDown());
	}

	IEnumerator CountDown(Action callBack = null)
	{
		yield return new WaitForSeconds(0.5f);
		Debug.Log(3);
		yield return new WaitForSeconds(0.5f);
		Debug.Log(2);
		yield return new WaitForSeconds(0.5f);
		Debug.Log(1);
		yield return new WaitForSeconds(0.5f);
		Debug.Log("GO !");

		if(callBack != null)
			callBack();
	}
	
	public void StartTimeAttackGame()
	{
		Debug.Log("Start Time Attack Game");
		MenuManager.GetInstance().bringOutPanel(MenuManager.GetInstance().activeMenuPanel);
		StartCoroutine(CountDown()); //placeholder 
	}

	public void StartSurvivalGame()
	{
		Debug.Log("Start Survival Attack Game");
		MenuManager.GetInstance().bringOutPanel(MenuManager.GetInstance().activeMenuPanel);
		StartCoroutine(CountDown()); //placeholder 
	}

	public static GameManager GetInstance()
	{
		return _instance;
	}

}
