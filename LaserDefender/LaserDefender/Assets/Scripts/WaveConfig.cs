using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
	// Start is called before the first frame update
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] GameObject pathPrefab;
	[SerializeField] float timeBetweenSpawns = 0.5f;
	[SerializeField] float spawnFactor = 0.3f;
	[SerializeField] int noOfEnemies = 5;
	[SerializeField] float moveSpeed = 2f;

	public GameObject GetEnemyPrefab()
	{
		return enemyPrefab;
	}

	public List<Transform> GetWaypoint()
	{
		var waveWaypoint = new List<Transform>();
		foreach(Transform point in pathPrefab.transform)
		{
			waveWaypoint.Add(point);
		}
		return waveWaypoint;
	}

	public float GetTimeBetweenSpawns ()
	{
		return timeBetweenSpawns;

	}
	public float GetSpawnFactor()
	{
		return spawnFactor;

	}

	public float GetSpeed()
	{
		return moveSpeed;

	}

	public int GetEnemyNumber()
	{
		return noOfEnemies;
	}
}

