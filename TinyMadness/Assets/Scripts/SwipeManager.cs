using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwipeManager : MonoBehaviour
{
	public delegate void OnSwipeDelegate(int direction);
	public static SwipeManager Instance;

	public Shape			spawnedObj;
	public float			swipeSpeed = 10.0f;
	public bool				couldBeSwipe;
	public bool				canSwipe = true;

	public OnSwipeDelegate	OnSwipe;

	private Vector2			startPos;
	private float			startTime;
	//[SerializeField]
	//private float			comfortZone = 10.0f;
	[SerializeField]
	private float			maxSwipeTime = 1.0f;
	//[SerializeField]
	//private float			minSwipeDist = 44.0f;

	void Awake ()
	{
		Instance = this;
	}

	void Start()
	{
		OnSwipe += MoveTo;
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
					OnSwipe(2);
				else if (swipeAngle > 135)
					OnSwipe(0);
				else if (Input.mousePosition.y - startPos.y > 0)
					OnSwipe(1);
				else if (Input.mousePosition.y - startPos.y < 0)
					OnSwipe(3);
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
					if (Time.time - startTime < maxSwipeTime)
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

					if (swipeTime < maxSwipeTime)
					{
						if (swipeAngle < 45)
							OnSwipe(2);
						else if (swipeAngle > 135)
							OnSwipe(0);
						else if (Input.mousePosition.y - startPos.y > 0)
							OnSwipe(1);
						else if (Input.mousePosition.y - startPos.y < 0)
							OnSwipe(3);
					}
					break;
			}
		}
#endif
	}

	public void MoveTo(int _shapeId)
	{
		couldBeSwipe = false;

		if (GameplayManager.Instance.timeAttackGame || GameplayManager.Instance.survivalGame)
			StartCoroutine(MoveInGameplayCoroutine(_shapeId));
	}

	public IEnumerator MoveInGameplayCoroutine(int __shapeId)
	{
		if (__shapeId == 3)
			yield break;

		if(canSwipe)
		{
			canSwipe = false;

			while (spawnedObj.transform.position != GameplayManager.Instance.shapes[__shapeId].transform.position)
			{
				spawnedObj.transform.position = Vector2.MoveTowards(spawnedObj.transform.position, GameplayManager.Instance.shapes[__shapeId].transform.position, swipeSpeed * Time.deltaTime);
				yield return new WaitForEndOfFrame();
			}

			if (spawnedObj.transform.tag == GameplayManager.Instance.shapes[__shapeId].transform.tag && spawnedObj.myRend.material.color == Color.green)
			{
				GameplayManager.Instance.AddScore();
				GameplayManager.Instance.shapeCheckers[__shapeId].greenParticules.Play();
			}
			else if (spawnedObj.transform.tag != GameplayManager.Instance.shapes[__shapeId].transform.tag && spawnedObj.myRend.material.color == Color.red)
			{
				GameplayManager.Instance.AddScore();
				GameplayManager.Instance.shapeCheckers[__shapeId].redParticules.Play();
			}
			else
			{
				GameplayManager.Instance.FailedSwipeOrEndObjLife();
				#if UNITY_ANDROID
					Handheld.Vibrate();
				#endif
			}
			StartCoroutine(SpawnManager.Instance.CanSpawnCoroutine());
			StopCoroutine("MoveInGameplayCoroutine");
		}
	}
}