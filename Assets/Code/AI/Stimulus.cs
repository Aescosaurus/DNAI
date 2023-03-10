using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stimulus
{
	public enum Item
	{
		Tick,
		DNAgent,
		Food,
		Spike,
		
		Count
	}

	Stimulus() {}

	public Stimulus( Item item,Dir dir )
	{
		this.item = item;
		this.dir = dir;
	}

	public static Stimulus GenerateRandom()
	{
		var stim = new Stimulus();
		stim.item = ( Item )Random.Range( 0,( int )Item.Count );
		stim.dir = ( Dir )Random.Range( 0,( int )Dir.Count );
		return( stim );
	}

	public bool CheckEqual( Stimulus other )
	{
		bool sameItem = ( item == other.item );
		if( item == Item.Tick ) return( true );
		else return( dir == other.dir );
	}

	public Item item;
	public Dir dir;
}