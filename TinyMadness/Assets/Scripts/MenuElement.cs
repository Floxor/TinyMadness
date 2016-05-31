using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class MenuElement : MonoBehaviour
{
	public string inAnim;
	public string outAnim;
	[SerializeField]
	private UnityEvent buttonCallback;

	private Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
		if (inAnim == null)
			Debug.LogWarning("No inAnim name defined for: " + gameObject.name);
		if (outAnim == null)
			Debug.LogWarning("No outAnim name defined for: " + gameObject.name);
	}

	public void playOutAnim()
	{
		animator.SetTrigger(outAnim);
	}

	public void playInAnim()
	{
		animator.SetTrigger(inAnim);
	}

	public void callback()
	{
		buttonCallback.Invoke();
	}


}