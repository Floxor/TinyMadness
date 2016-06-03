using UnityEngine;
using System.Collections;

public class Shining : MonoBehaviour
{
	private Animator	myAnimator;
	public string		shineAnim;

	void Start ()
	{
		myAnimator = GetComponent<Animator>();
		StartCoroutine(ShiningCoroutine());
	}
	
	IEnumerator ShiningCoroutine()
	{
		float timer = Random.Range(10, 60);

		myAnimator.SetTrigger(shineAnim);

		while(timer > 0)
		{
			timer -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		StartCoroutine(ShiningCoroutine());
	}
}
