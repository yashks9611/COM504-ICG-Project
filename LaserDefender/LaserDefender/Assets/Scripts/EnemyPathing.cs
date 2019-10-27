using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
	WaveConfig waveConfig;

	List<Transform> waypoint;
	int waypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
		waypoint = waveConfig.GetWaypoint();
		transform.position = waypoint[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
	{
		Move();
	}

	public void SetWaveConfig(WaveConfig currentWaveConfig)
	{
		waveConfig = currentWaveConfig;
	}
	private void Move()
	{
		if (waypointIndex <= waypoint.Count - 1)
		{
			var targetPos = waypoint[waypointIndex].transform.position;
			var moveThisFrame = waveConfig.GetSpeed() * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, targetPos, moveThisFrame);
			if (transform.position == targetPos)
			{
				waypointIndex++;
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
