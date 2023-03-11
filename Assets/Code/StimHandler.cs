using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StimHandler
{
	public static void AddSelf( StimObj obj )
	{
		Assert.IsTrue( obj.stimType != Stimulus.Item.Count );

		stimObjs.Add( obj );
	}

	public static void RemoveSelf( StimObj obj )
	{
		stimObjs.Remove( obj );
	}

	public static List<StimObj> GetNearby( Vector2 pos,float findDist )
	{
		var nearby = new List<StimObj>();

		foreach( var obj in stimObjs )
		{
			var diff = ( Vector2 )obj.transform.position - pos;
			if( diff.sqrMagnitude < findDist * findDist )
			{
				nearby.Add( obj );
			}
		}

		return( nearby );
	}

	public static List<StimObj> GetStimObjs()
	{
		return( stimObjs );
	}

	static List<StimObj> stimObjs = new List<StimObj>();
}