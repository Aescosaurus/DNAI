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

		Time.timeScale = timeScale;
	}

	void Update()
	{
		if( waveDuration.Update( Time.deltaTime ) )
		{
			waveDuration.Reset();

			var survivors = StimHandler.GetStimObjs();

			int spawnedObjsSize = spawnedObjs.Count;

			int spawnedChildren = 0;
			foreach( var survivor in survivors )
			{
				var survivorDNAgent = survivor.GetComponent<DNAgent>();
				if( survivorDNAgent != null )
				{
					for( int i = 0; i < survivorDNAgent.GetFoodEaten() * offspringCount; ++i )
					{
						var offspringGenes = survivorDNAgent.GenerateChild( i != 0 );
						var child = SpawnObj( dnaiPrefab );
						child.GetComponent<DNAgent>().LoadDNA( offspringGenes );
						++spawnedChildren;
					}
				}
			}

			for( int i = spawnedObjsSize - 1; i >= 0; --i )
			{
				if( spawnedObjs[i] != null ) spawnedObjs[i].GetComponent<StimObj>().Despawn();
			}
			spawnedObjs.RemoveRange( 0,spawnedObjsSize - 1 );

			if( spawnedChildren < 1 )
			{
				for( int i = 0; i < dnaiCount; ++i ) SpawnObj( dnaiPrefab );
				print( "no children, spawning new generation" );
			}
			
			for( int i = 0; i < foodCount; ++i ) SpawnObj( foodPrefab );
			for( int i = 0; i < spikeCount; ++i ) SpawnObj( spikePrefab );
		}
	}

	GameObject SpawnObj( GameObject prefab )
	{
		var obj = Instantiate( prefab );

		var randPos = ( Vector2 )transform.position + new Vector2(
			Random.Range( -spawnRadius,spawnRadius ),
			Random.Range( -spawnRadius,spawnRadius ) );
		obj.transform.position = randPos;

		spawnedObjs.Add( obj );

		return( obj );
	}

	[SerializeField] GameObject dnaiPrefab = null;
	[SerializeField] GameObject foodPrefab = null;
	[SerializeField] GameObject spikePrefab = null;

	List<GameObject> spawnedObjs = new List<GameObject>();

	[SerializeField] float spawnRadius = 5.0f;

	[SerializeField] int dnaiCount = 10;
	[SerializeField] int foodCount = 10;
	[SerializeField] int spikeCount = 5;

	[SerializeField] Timer waveDuration = new Timer( 20.0f );
	[SerializeField] float timeScale = 1.0f;
	// todo: more/fewer offspring based on quantity of food eaten?
	[SerializeField] int offspringCount = 3;
}