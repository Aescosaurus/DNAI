using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
	public enum ActionType
	{
		Stay,
		Move,
		Turn,
	
		Count
	}

	Action() {}

	public Action( ActionType action,Dir dir )
	{
		this.action = action;
		this.dir = dir;
	}

	public static Action GenerateRandom()
	{
		var action = new Action();
		action.action = ( ActionType )Random.Range( 0,( int )ActionType.Count );
		action.dir = ( Dir )Random.Range( 0,( int )Dir.Count );
		return( action );
	}

	public ActionType action;
	public Dir dir;
}