using UnityEngine;
using System.Collections;

public class Shape : MonoBehaviour
{
	public float		lifeTime = 5;
	public Renderer		myRend;

	void Start ()
	{
		StartCoroutine(ReduceLife());
		myRend = gameObject.GetComponent<Renderer>();
		RandomObjColor();
		MenuManager.GetInstance().clockTimeOut.LaunchCountDown(lifeTime);
	}
	
	//void Update ()
	//{
	
	//}

	public void RandomObjColor()
	{
		int randomInt = Random.Range(0, 2);
		if (randomInt % 2 == 0)
		{
			myRend.material.color = Color.red;
		}
		else
		{
			myRend.material.color = Color.green;
		}
	}

	public void Kill()
	{
		Destroy(this.gameObject);
	}

	IEnumerator ReduceLife()
	{
		while (lifeTime > 0)
		{
			lifeTime -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		GameplayManager.Instance.FailedSwipeOrEndObjLife();
		StopCoroutine("ReduceLife");
	}
}
