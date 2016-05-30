using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;

	[SerializeField]
	private GameObject[] spawnableObjs;
	private GameObject spawnedObj;
	private Renderer spawnedObjMat;
	[SerializeField]
	private bool canSpawn = false;

	void Start ()
	{
		Instance = this;
	}
	
	void Update ()
	{
		//if(delegate.OnPointMarked)
		//{
		//	canSpawn = true;
		//}
		if(canSpawn)
		{
			SpawnRandomObj();
		}

		if(spawnedObj == null)
		{
			canSpawn = true;
		}
	
	}

	public void SpawnRandomObj()
	{
		canSpawn = false;

		int randomIndex = Random.Range(0, spawnableObjs.Length);
		spawnedObj = GameObject.Instantiate(spawnableObjs[randomIndex].gameObject, spawnableObjs[randomIndex].gameObject.transform.position, Quaternion.identity) as GameObject;
		
		spawnedObjMat = spawnedObj.GetComponent<Renderer>();
		RandomObjColor();

		SwipeManager.Instance.spawnedObj = spawnedObj;
	}

	public void RandomObjColor()
	{
		int randomInt = Random.Range(0, 2);
		if(randomInt%2 == 0)
		{
			spawnedObjMat.material.color = Color.red;
		}
		else
		{
			spawnedObjMat.material.color = Color.green;
		}
	}
}
