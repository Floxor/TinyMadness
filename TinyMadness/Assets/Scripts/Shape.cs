using UnityEngine;
using System.Collections;

public class Shape : MonoBehaviour
{
	public float		lifeTime = 5;
	public Renderer		myRend;

	private Animator	myAnimator;
	[SerializeField]
	private string		smallerAnim;
	[SerializeField]
	private string		biggerAnim;

	void Awake ()
	{
		myAnimator = GetComponent<Animator>();
	}

	void Start ()
	{
		myAnimator.SetTrigger(biggerAnim);
		StartCoroutine(ReduceLife());
		myRend = gameObject.GetComponent<Renderer>();
		RandomObjColor();
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

		myAnimator.SetTrigger(smallerAnim);
		GameplayManager.Instance.FailedSwipeOrEndObjLife();
		//StopCoroutine("ReduceLife");
	}
}
