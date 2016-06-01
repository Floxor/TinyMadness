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

	public void StartCountDown()
	{
		StartCoroutine(CountDown());
	}

	IEnumerator CountDown(Action callBack = null)
	{
		yield return new WaitForSeconds(0.5f);
		MenuManager.GetInstance().ZoomInObject(MenuManager.GetInstance().countDownObject.transform.GetChild(0).gameObject, 0.5f);
		
		yield return new WaitForSeconds(0.5f);
		MenuManager.GetInstance().ZoomInObject(MenuManager.GetInstance().countDownObject.transform.GetChild(1).gameObject, 0.5f);
		
		yield return new WaitForSeconds(0.5f);
		MenuManager.GetInstance().ZoomInObject(MenuManager.GetInstance().countDownObject.transform.GetChild(2).gameObject, 0.5f);
		
		yield return new WaitForSeconds(0.5f);
		MenuManager.GetInstance().ZoomInObject(MenuManager.GetInstance().countDownObject.transform.GetChild(3).gameObject, 0.5f);

		if(callBack != null)
			callBack();
	}
	
	public void StartTimeAttackGame()
	{
		Debug.Log("Start Time Attack Game");
		MenuManager.GetInstance().bringOutPanel(MenuManager.GetInstance().activeMenuPanel);
		StartCoroutine(CountDown(GameplayManager.Instance.GoTimeAttackGame));
		MenuManager.GetInstance().BringInUIShapes();
	}

	public void StartSurvivalGame()
	{
		Debug.Log("Start Survival Attack Game");
		MenuManager.GetInstance().bringOutPanel(MenuManager.GetInstance().activeMenuPanel);
		StartCoroutine(CountDown(GameplayManager.Instance.GoSurvivalGame));
		MenuManager.GetInstance().BringInUIShapes();
	}

	public void Replay()
	{
		if (GameplayManager.Instance.previousGameMode == GameplayManager.GameMode.TimeAttack)
		{
			StartTimeAttackGame();
		}

		if (GameplayManager.Instance.previousGameMode == GameplayManager.GameMode.Survival)
		{
			StartSurvivalGame();
		}
	}

	public static GameManager GetInstance()
	{
		return _instance;
	}

}
