using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner
	:
	MonoBehaviour
{
	void Start()
	{
		for( int i = 0; i < dnaiCount; ++i ) SpawnObj( dnaiPrefab );
		for( int i = 0; i < foodCount; ++i ) SpawnObj( foodPrefab );
		for( int i = 0; i < spikeCount; ++i ) SpawnObj( spikePrefab );
	}

	void SpawnObj( GameObject prefab )
	{
		var obj = Instantiate( prefab );
		var randPos = ( Vector2 )transform.position + new Vector2(
			Random.Range( -spawnRadius,spawnRadius ),
			Random.Range( -spawnRadius,spawnRadius ) );
		obj.transform.position = randPos;
	}

	[SerializeField] GameObject dnaiPrefab = null;
	[SerializeField] GameObject foodPrefab = null;
	[SerializeField] GameObject spikePrefab = null;

	[SerializeField] float spawnRadius = 5.0f;

	[SerializeField] int dnaiCount = 10;
	[SerializeField] int foodCount = 10;
	[SerializeField] int spikeCount = 5;
}