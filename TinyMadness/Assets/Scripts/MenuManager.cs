using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

	public GameObject activeMenuPanel;
	public GameObject[] menuPanels;

	void Start() 
	{
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
	}

	void Update() { }

	public void bringInPanel(GameObject newMenuPanel)
	{
		MenuElement[] menuElements = newMenuPanel.GetComponentsInChildren<MenuElement>();

		for (int i = 0; i < menuElements.Length; ++i)
		{
			menuElements[i].playInAnim();
			if (menuElements[i].GetComponent<Button>() != null)
				menuElements[i].GetComponent<Button>().interactable = true;
		}

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
	}

	public void switchToPanel(GameObject newMenuPanel)
	{
		bringOutPanel(activeMenuPanel);
		bringInPanel(newMenuPanel);
	}
}