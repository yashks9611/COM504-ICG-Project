using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

	[SerializeField] List<WaveConfig> waveConfigs;
	[SerializeField] Boolean waveLooping = false;
	[SerializeField] int startingWave = 0;
    // Start is called before the first frame update
    IEnumerator Start()
    {
		do
		{
			yield return StartCoroutine(SpawnAllWaves());
		} while (waveLooping);
	}

	private IEnumerator SpawnAllWaves()
	{
		for(int i =0; i < waveConfigs.Count; i++)
		{
			var currentWave = waveConfigs[i];
			yield return StartCoroutine(SpawnEnemyWaves(currentWave));
		}
	}

	private IEnumerator SpawnEnemyWaves(WaveConfig waveConfigNumber)
	{
		for(int i = startingWave; i < waveConfigNumber.GetEnemyNumber(); i++)
		{
			var newEnemy = Instantiate(waveConfigNumber.GetEnemyPrefab(), 
				waveConfigNumber.GetWaypoint()[0].transform.position, 
				Quaternion.identity);
			newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfigNumber);
			yield return new WaitForSeconds(waveConfigNumber.GetTimeBetweenSpawns());
		}
	}


	// Update is called once per frame
}
