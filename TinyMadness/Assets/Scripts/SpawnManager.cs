using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;

	[SerializeField]
	private GameObject[]	spawnableObjs;
	[SerializeField]
	private string[]		tags;
	public GameObject		spawnedObj;

	public bool				canSpawn = false;
	public float			savedSpawnDelay = 0.0f;
	public float			spawnDelay = 10.0f;

	void Awake ()
	{
		Instance = this;
		savedSpawnDelay = spawnDelay;
	}
	
	void Update ()
	{
		if(canSpawn)
		{
			SpawnRandomObj();
		}
	}

	public void SpawnRandomObj()
	{
		canSpawn = false;
		if(spawnDelay != savedSpawnDelay)
		{
			spawnDelay = savedSpawnDelay;
		}

		int randomObjIndex = Random.Range(0, spawnableObjs.Length);
		spawnedObj = GameObject.Instantiate(spawnableObjs[randomObjIndex].gameObject, spawnableObjs[randomObjIndex].gameObject.transform.position, Quaternion.identity) as GameObject;

		SwipeManager.Instance.spawnedObj = spawnedObj.GetComponent<Shape>();
	}

	

	public IEnumerator CanSpawnCoroutine()
	{
		while(spawnDelay > 0 && !canSpawn)
		{
			spawnDelay -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		if (GameplayManager.Instance.timeAttackGame || GameplayManager.Instance.survivalGame)
		{
			spawnedObj.GetComponent<Shape>().Kill();
			canSpawn = true;
			SwipeManager.Instance.canSwipe = true;
		}

		StopCoroutine("CanSpawnCoroutine");
	}
}
