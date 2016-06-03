using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
	public static GameplayManager Instance;

	[System.Serializable]
	public class ShapesChecker
	{
		public GameObject shape;
		public string shapeTag = "";
		public ParticleSystem greenParticules;
		public ParticleSystem redParticules;
	}

	public ShapesChecker[] shapeCheckers;

	public GameObject[] shapes;
	public Text			scoreText;
	public Text			clockText;
	public float		highScore = 0;
	public bool			survivalGame = false;
	public bool			timeAttackGame = false;
	public float		actualClock;
	public float		difficultyFactor = 0.0f;

	private float		score = 0;
	private float		actualScore = 0;
	[SerializeField]
	private float		clock = 60.0f;
	private float		savedDifficultyFactor;
	[SerializeField]
	private float		difficultyClock = 40.0f;

	void Start ()
	{
		Instance = this;
		scoreText.text = highScore.ToString();
		clockText.text = null;
		for (int i = 0; i < shapeCheckers.Length; i++)
		{
			shapeCheckers[i].shapeTag = shapeCheckers[i].shape.transform.tag;
			shapeCheckers[i].greenParticules.transform.position = shapeCheckers[i].shape.transform.position;
			shapeCheckers[i].redParticules.transform.position = shapeCheckers[i].shape.transform.position;
		}
	}

	void Update()
	{
		if(actualScore != score)
		{
			actualScore = score;
			scoreText.text = actualScore.ToString();
		}

		if(timeAttackGame)
			clockText.text = actualClock.ToString("0.00");

		if(savedDifficultyFactor != difficultyFactor)
		{
			savedDifficultyFactor = difficultyFactor;
			StartCoroutine(DifficultyCoroutine());
		}
	}

	public void GoTimeAttackGame()
	{
		timeAttackGame = true;
		StartNewGame();
		StartCoroutine(ReduceClock());
	}

	public void GoSurvivalGame()
	{
		survivalGame = true;
		StartNewGame();
		StartCoroutine(DifficultyCoroutine());
	}

	public void StartNewGame()
	{
		scoreText.text = actualScore.ToString();
		SpawnManager.Instance.canSpawn = true;
		SwipeManager.Instance.canSwipe = true;
	}

	public void FailedSwipeOrEndObjLife()
	{
		if (timeAttackGame)
		{
			ReduceScore();
		}
		else
		{
			GameOver();
			CustomStopAllGameplayCoroutines();
		}
	}

	public void GameOver()
	{
		SpawnManager.Instance.canSpawn = false;
		MenuManager.GetInstance().clockTimeOut.Stop();
		StopCoroutine("DifficultyCoroutine");
		difficultyFactor = savedDifficultyFactor = 0.0f;
		timeAttackGame = false;
		survivalGame = false;

		if (actualScore > highScore)
		{
			highScore = actualScore;
		}
		
		scoreText.text = highScore.ToString();
		actualScore = 0;
		score = 0;
		clockText.text = "";

		if(SpawnManager.Instance.spawnedObj)
			SpawnManager.Instance.spawnedObj.GetComponent<Shape>().Kill();

		MenuManager.GetInstance().BringInGameOver();
	}

	public void AddScore()
	{
		score += 1;
	}

	public void ReduceScore()
	{
		score -= 1;
		//CameraManager.GetInstance().ScreenShake(ShakeForce.medium, ShakeLength.shortTime);
		if (score < 0)
			score = 0;
	}

	public void ResetScore()
	{
		score = 0;
	}

	public void CustomStopAllGameplayCoroutines()
	{
		StopCoroutine("SpawnManager.Instance.CanSpawnCoroutine");
		StopCoroutine("SwipeManager.Instance.MoveInGameplayCoroutine");
	}

	IEnumerator ReduceClock()
	{
		actualClock = clock;
		while (actualClock > 0)
		{
			actualClock -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		actualClock = 0;
		GameOver();
		StopCoroutine("ReduceClock");
	}

	IEnumerator DifficultyCoroutine()
	{
		float timer = difficultyClock;
		while(timer > 0)
		{
			timer -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		difficultyFactor += 0.25f;
		Debug.Log(difficultyFactor);
		StopCoroutine("DifficultyCoroutine");
	}
}