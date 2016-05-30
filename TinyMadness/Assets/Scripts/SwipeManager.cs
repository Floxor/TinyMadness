using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwipeManager : MonoBehaviour
{
	public static SwipeManager Instance;

	public GameObject spawnedObj;

	private bool couldBeSwipe;
	private Vector2 startPos;
	private float startTime;
	[SerializeField]
	private float comfortZone = 10;
	[SerializeField]
	private float maxSwipeTime = 1;
	[SerializeField]
	private float minSwipeDist = 44;

	void Start ()
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

			if(swipeAngle < 45)
			{
				spawnedObj.GetComponent<Renderer>().material.color = Color.green;
			}
			else if(swipeAngle > 135)
			{
				spawnedObj.GetComponent<Renderer>().material.color = Color.red;
			}
			else
			{
				spawnedObj.GetComponent<Renderer>().material.color = Color.blue;
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
					if (Mathf.Abs(touch.position.y - startPos.y) > comfortZone)
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

					if(swipeAngle < 45)
					{
						spawnedObj.GetComponent<Renderer>().material.color = Color.green;
					}
					else if(swipeAngle > 135)
					{
						spawnedObj.GetComponent<Renderer>().material.color = Color.red;
					}
					else
					{
						spawnedObj.GetComponent<Renderer>().material.color = Color.blue;
					}
					break;
			}
		}
#endif
	}

	public void SwipeState(float _dirX, float _dirY)
	{
		List<float> vector = new List<float>();
		vector.Add(_dirX);
		vector.Add(_dirY);
		//vector.Sort();

		string leftAndRight = vector[0] + "," + vector[1];
		Debug.Log(vector[0] + "X et Y" + vector[1]);
		Debug.Log("Vector : "+leftAndRight);

		switch(leftAndRight)
		{
			case "1,0" :
				spawnedObj.GetComponent<Renderer>().material.color = Color.red;
				break;
			case "0,1":
				spawnedObj.GetComponent<Renderer>().material.color = Color.blue;
				break;
			case "-1,0":
				spawnedObj.GetComponent<Renderer>().material.color = Color.green;
				break;
		}
	}
}
