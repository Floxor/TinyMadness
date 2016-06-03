using UnityEngine;
using System.Collections;

public class Shape : MonoBehaviour
{
	public float		lifeTime = 3;
	public Renderer		myRend;

	private Animator	myAnimator;
	[SerializeField]
	private string		smallerAnim;
	[SerializeField]
	private string		biggerAnim;
	private float		minLifeTime = 1;

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
		MenuManager.GetInstance().clockTimeOut.Launch(lifeTime);
	}

	public void RandomObjColor()
	{
		int randomInt = Random.Range(0, 3);
		if (randomInt%2 == 0)
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
		SpawnManager.Instance.spawnedObj = null;
		Destroy(this.gameObject);
	}

	public IEnumerator ReduceLife()
	{
		lifeTime = lifeTime - GameplayManager.Instance.difficultyFactor;
		if(lifeTime < minLifeTime)
		{
			lifeTime = minLifeTime;
		}

		if(SwipeManager.Instance.canSwipe)
		{
			while (lifeTime > 0)
			{
				lifeTime -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			myAnimator.SetTrigger(smallerAnim);
			GameplayManager.Instance.FailedSwipeOrEndObjLife();
			StartCoroutine(SpawnManager.Instance.CanSpawnCoroutine());
			StopCoroutine("ReduceLife");
		}
		else
		{
			myAnimator.SetTrigger(smallerAnim);
			GameplayManager.Instance.FailedSwipeOrEndObjLife();
			StartCoroutine(SpawnManager.Instance.CanSpawnCoroutine());
		}
	}
}