using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;

	[SerializeField]
	private GameObject[]	spawnableObjs;
	[SerializeField]
	private string[]		tags;
	private GameObject		spawnedObj;

	public bool				canSpawn = false;
	public Renderer			spawnedObjMat;
	public float			objLife = 10.0f;

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

		int randomObjIndex = Random.Range(0, spawnableObjs.Length);
		spawnedObj = GameObject.Instantiate(spawnableObjs[randomObjIndex].gameObject, spawnableObjs[randomObjIndex].gameObject.transform.position, Quaternion.identity) as GameObject;
		
		spawnedObjMat = spawnedObj.GetComponent<Renderer>();
		RandomObjColor();

		//int randomTagIndex = Random.Range(0, tags.Length);
		//spawnedObj.tag = tags[randomTagIndex];
		//Debug.Log(tags[randomTagIndex]);

		SwipeManager.Instance.spawnedObj = spawnedObj;
		StartCoroutine(ObjLifeReduction());
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

	IEnumerator ObjLifeReduction()
	{
		float time = objLife;
		while(time > 0)
		{
			time -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		Destroy(spawnedObj);
		SwipeManager.Instance.spawnedObj = null;
		GameplayManager.Instance.ResetScore();

		StopCoroutine("ObjLifeReduction");
	}
}
