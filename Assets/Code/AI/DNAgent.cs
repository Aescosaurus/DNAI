using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class DNAgent
	:
	MonoBehaviour
{
	void Start()
	{
		body = GetComponent<Rigidbody2D>();

		transform.Rotate( 0.0f,0.0f,Random.Range( 0.0f,360.0f ) );

		GenerateDNA();
	}

	void Update()
	{
		if( actionCheckTimer.Update( Time.deltaTime ) )
		{
			actionCheckTimer.Reset();

			var nearby = StimHandler.GetNearby( ( Vector2 )transform.position,findDist );
			var stimList = new List<Stimulus>();
			foreach( var item in nearby )
			{
				var dir = CalcDir( item.transform.position );
				if( dir != Dir.Count )
				{
					var stimType = item.stimType;
					stimList.Add( new Stimulus( stimType,dir ) );
				}
			}

			// try to prevent from doing nothing if no stimuli
			if( stimList.Count < 1 ) stimList.Add( new Stimulus( Stimulus.Item.Tick,Dir.Straight ) );

			actionList.Clear();
			foreach( var gene in dna )
			{
				if( gene.CheckStimulus( stimList ) )
				{
					actionList.AddRange( gene.GetActions() );
				}
			}
		}

		foreach( var action in actionList )
		{
			PerformAction( action );
		}
	}

	void PerformAction( Action action )
	{
		switch( action.action )
		{
			case Action.ActionType.Stay: // do nothing
				break;
			case Action.ActionType.Move:
				body.AddForce( transform.up * moveSpd,ForceMode2D.Force );
				break;
			case Action.ActionType.Turn:
			{
				float turnDir = 0.0f;
				if( action.dir == Dir.Left ) turnDir = -1.0f;
				else if( action.dir == Dir.Right ) turnDir = 1.0f;
				transform.Rotate(  0.0f,0.0f,turnDir * turnSpd );
			}
				break;
			default:
				Assert.IsTrue( false );
				break;
		}
	}

	void GenerateDNA()
	{
		int geneCount = ( int )Stimulus.Item.Count;
		for( int i = 0; i < geneCount; ++i )
		{
			dna.Add( new Gene() );
			dna[dna.Count - 1].OverwriteStimulus( new Stimulus( ( Stimulus.Item )i,
				( Dir )Random.Range( 0,( int )Dir.Count ) ) );
		}
	}

	public void LoadDNA( List<Gene> genes )
	{
		dna.Clear();

		dna.AddRange( genes );
	}

	public List<Gene> GenerateChild()
	{
		var childDNA = new List<Gene>();

		foreach( var gene in dna )
		{
			childDNA.Add( gene.GenerateModified() );
		}

		return( childDNA );
	}

	Dir CalcDir( Vector3 objPos )
	{
		var diff = ( objPos - transform.position ).normalized;
		var ang = Mathf.Atan2( diff.y,diff.x ) * Mathf.Rad2Deg - 90.0f;
		if( Mathf.Abs( ang ) < straightAngTolerance ) return( Dir.Straight );
		else if( Mathf.Abs( ang ) < perceiveRange )
		{
			var cross = Vector3.Cross( diff,transform.up );
			return( cross.z > 0.0f ? Dir.Right : Dir.Left );
		}
		return( Dir.Count );
	}

	void OnTriggerEnter2D( Collider2D coll )
	{
		if( coll.tag == "Spike" )
		{
			GetComponent<StimObj>().Despawn();
		}
	}

	Rigidbody2D body;

	List<Gene> dna = new List<Gene>();

	List<Action> actionList = new List<Action>();

	// we can modify this value with higher food cost to simulate metabolism
	Timer actionCheckTimer = new Timer( 0.5f );

	const float findDist = 3.0f;

	const float straightAngTolerance = 5.0f;
	const float perceiveRange = 45.0f;

	const float moveSpd = 0.003f;
	const float turnSpd = 0.07f;
}