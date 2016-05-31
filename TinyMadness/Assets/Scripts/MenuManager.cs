using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	private static MenuManager _instance;
	private bool _alreadyInstantiated;

	public GameObject countDownObject;
	public GameObject activeMenuPanel;
	public GameObject[] menuPanels;

	public MenuElement[] swipeCallbacks;

	[HideInInspector]
	public bool userIsInMenu = true;

	void Awake()
	{
		if(_instance != null)
			_alreadyInstantiated = true;
		else
			_instance = this;
	}

	void Start()
	{
		if (_alreadyInstantiated)
		{
			Destroy(gameObject);
			return;
		}

		StartCoroutine(delayedStart());

	}

	IEnumerator delayedStart()
	{
		yield return new WaitForSeconds(1);

		if (activeMenuPanel != null)
			bringInPanel(activeMenuPanel);
		else if (menuPanels[0] != null)
			bringInPanel(menuPanels[0]);
		else
			Debug.LogError("No default menu set and no panels to bring in for MenuManager !");

		SwipeManager.Instance.OnSwipe += OnSwipe;
	}

	void Update() { }

	public void ZoomInObject(GameObject Obj, float time)
	{
		StartCoroutine(zoomInCoroutine(Obj, time));
	}

	IEnumerator zoomInCoroutine(GameObject Obj, float time)
	{
		float eT = 0;
		Vector3 targetScale;
		Obj.SetActive(true);
		targetScale = Obj.transform.localScale;
		Obj.transform.localScale = Vector3.zero;

		if (Obj.GetComponent<Text>() != null)
			Obj.GetComponent<Text>().CrossFadeAlpha(0, time * 1.3f, true);
		
		while (eT < time)
		{
			eT += Time.deltaTime;
			Obj.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, eT / time);
			yield return null;
		}
	}

	public void bringInPanel(GameObject newMenuPanel)
	{
		MenuElement[] menuElements = newMenuPanel.GetComponentsInChildren<MenuElement>();

		for (int i = 0; i < menuElements.Length; ++i)
		{
			menuElements[i].playInAnim();
			if (menuElements[i].GetComponent<Button>() != null)
				menuElements[i].GetComponent<Button>().interactable = true;
		}
		userIsInMenu = true;
		activeMenuPanel = newMenuPanel;
	}

	public void bringOutPanel(GameObject oldMenuPanel)
	{ 
		MenuElement[] menuElements = oldMenuPanel.GetComponentsInChildren<MenuElement>();

		for (int i = 0; i < menuElements.Length; ++i)
		{
			menuElements[i].playOutAnim();
			if (menuElements[i].GetComponent<Button>() != null)
				menuElements[i].GetComponent<Button>().interactable = false;
		}
		userIsInMenu = false;
	}


	void OnSwipe(int swipeDirection)
	{
		if (!userIsInMenu)
			return;

		if (swipeCallbacks[swipeDirection] != null)
			swipeCallbacks[swipeDirection].callback();

		//Debug.Log("user has swiped: " + swipeDirection);
	}

	public void switchToPanel(GameObject newMenuPanel)
	{
		bringOutPanel(activeMenuPanel);
		bringInPanel(newMenuPanel);
	}

	public static MenuManager GetInstance()
	{
		return _instance;
	}
}