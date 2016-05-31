using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
	public static GameplayManager Instance;

	public GameObject[] shapes;
	public Text scoreText;
	public float score = 0;
	public float actualScore = 0;
	public float highScore = 0;

	void Start ()
	{
		Instance = this;
		scoreText.text = actualScore.ToString();
	}

	void Update()
	{
		if(actualScore != score)
		{
			actualScore = score;
			scoreText.text = actualScore.ToString();
		}
	}

	public void Scoring()
	{
		score += 1;
	}

	public void ResetScoring()
	{
		score = 0;
	}
}
