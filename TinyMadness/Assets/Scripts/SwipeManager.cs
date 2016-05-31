using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwipeManager : MonoBehaviour
{
	public delegate void OnSwipeDelegate(int direction);
	public static SwipeManager Instance;

	public GameObject	spawnedObj;
	public float		speed = 10.0f;

	public OnSwipeDelegate OnSwipe;

	private bool		couldBeSwipe;
	private Vector2		startPos;
	private float		startTime;
	[SerializeField]
	private float		comfortZone = 10.0f;
	[SerializeField]
	private float		maxSwipeTime = 1.0f;
	[SerializeField]
	private float		minSwipeDist = 44.0f;

	void Awake ()
	{
		Instance = this;
	}
	
	void Update ()
	{
	#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0))
		{
			couldBeSwipe = true;
			startPos = Input.mousePosition;
			startTime = Time.time;
			//Debug.Log(couldBeSwipe + " StartPos x :" + startPos.x + " StartPos y :" + startPos.y + " StartTime :" + startTime);
		}

		if (Input.GetMouseButtonUp(0))
		{
			//Debug.Log(" SwipeEndPosDir x :" + Input.mousePosition.x + " EndPos y :" + Input.mousePosition.y + " SwipeTime :" + swipeTime);

			float swipeTime = Time.time - startTime;
			Vector2 directionSwipe = (Vector2)Input.mousePosition - startPos;
			float swipeAngle = Vector2.Angle(Vector3.right, directionSwipe);

			if(swipeTime < maxSwipeTime)
			{
				if (swipeAngle < 45)
				{
					//spawnedObj.GetComponent<Renderer>().material.color = Color.yellow;
					OnSwipe(2);
					MoveTo(2);
				}
				else if (swipeAngle > 45 && swipeAngle < 135 && (Mathf.Sign(Input.mousePosition.y - startPos.y) == 1))
				{
					//spawnedObj.GetComponent<Renderer>().material.color = Color.blue;
					OnSwipe(1);
					MoveTo(1);
				}
				else if (swipeAngle > 135)
				{
					//spawnedObj.GetComponent<Renderer>().material.color = Color.black;
					OnSwipe(0);
					MoveTo(0);
				}
			}
		}
	#endif
	#if UNITY_ANDROID
		if (Input.touchCount > 0)
		{
			var touch = Input.touches[0];

			switch (touch.phase)
			{
				case TouchPhase.Began:
					couldBeSwipe = true;
					startPos = touch.position;
					startTime = Time.time;
					break;

				case TouchPhase.Moved:
					if (Vector2.Angle(Vector3.right, (Vector2)Input.mousePosition - startPos) > comfortZone)
					{
						couldBeSwipe = false;
					}
					break;

				case TouchPhase.Stationary:
					couldBeSwipe = false;
					break;

				case TouchPhase.Ended:
					float swipeTime = Time.time - startTime;
					Vector2 directionSwipe = (Vector2)Input.mousePosition - startPos;
					float swipeAngle = Vector2.Angle(Vector3.right, directionSwipe);

					if(swipeTime < maxSwipeTime)
					{
						if (swipeAngle < 45)
						{
							spawnedObj.GetComponent<Renderer>().material.color = Color.yellow;
							MoveTo(2);
						}
						else if (swipeAngle > 45 && swipeAngle < 135 && (Mathf.Sign(Input.mousePosition.y - startPos.y) == 1))
						{
							spawnedObj.GetComponent<Renderer>().material.color = Color.blue;
							MoveTo(1);
						}
						else if (swipeAngle > 135)
						{
							spawnedObj.GetComponent<Renderer>().material.color = Color.black;
							MoveTo(0);
						}
					}
					break;
			}
		}
#endif
	}

	public void MoveTo(int _shapeId)
	{
		//if(gameObject.GetComponent<GameplayManager>().enabled)
			StartCoroutine(MoveInGameplayCoroutine(_shapeId));
		//else if (gameObject.GetComponent<MenuManager>().enabled)
		//	StartCoroutine(MoveInMenuCoroutine(_shapeId));
	}

	public IEnumerator MoveInGameplayCoroutine(int __shapeId)
	{
		while(spawnedObj.transform.position != GameplayManager.Instance.shapes[__shapeId].transform.position)
		{
			spawnedObj.transform.position = Vector2.MoveTowards(spawnedObj.transform.position, GameplayManager.Instance.shapes[__shapeId].transform.position, speed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}

		if (spawnedObj.transform.tag == GameplayManager.Instance.shapes[__shapeId].transform.tag && SpawnManager.Instance.spawnedObjMat.material.color == Color.green)
		{
			GameplayManager.Instance.AddScore();
		}
		else if (spawnedObj.transform.tag != GameplayManager.Instance.shapes[__shapeId].transform.tag && SpawnManager.Instance.spawnedObjMat.material.color == Color.red)
		{
			GameplayManager.Instance.AddScore();
		}
		else
		{
			GameplayManager.Instance.ResetScore();
		}
		Destroy(spawnedObj);
		StopCoroutine("MoveInGameplayCoroutine");
	}

	//public IEnumerator MoveInMenuCoroutine(int __optionId)
	//{
	//	while (spawnedObj.transform.position != GameplayManager.Instance.shapes[__optionId].transform.position)
	//	{
	//		spawnedObj.transform.position = Vector2.MoveTowards(spawnedObj.transform.position, GameplayManager.Instance.shapes[__optionId].transform.position, speed * Time.deltaTime);
	//		yield return new WaitForEndOfFrame();
	//	}

	//	if (spawnedObj.transform.tag == GameplayManager.Instance.shapes[__optionId].transform.tag && SpawnManager.Instance.spawnedObjMat.material.color == Color.green)
	//	{
	//		GameplayManager.Instance.AddScore();
	//	}
	//	else if (spawnedObj.transform.tag != GameplayManager.Instance.shapes[__optionId].transform.tag && SpawnManager.Instance.spawnedObjMat.material.color == Color.red)
	//	{
	//		GameplayManager.Instance.AddScore();
	//	}
	//	else
	//	{
	//		if(GameplayManager.Instance.timeAttackGame)
	//			GameplayManager.Instance.ResetScore();
	//		else if(GameplayManager.Instance.survivalGame)
	//			GameplayManager.Instance.GameOver();
	//	}
	//	Destroy(spawnedObj);
	//	StopCoroutine("MoveInMenuCoroutine");
	//}
}
