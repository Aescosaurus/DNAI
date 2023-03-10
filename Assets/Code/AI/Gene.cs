using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Gene
{
	enum ActivateCondition
	{
		Either, // any stimuli activated = trigger condition
		All, // all stimuli must be activated

		Count
	}

	public Gene()
	{
		RandomizeGene();
	}

	public void RandomizeGene()
	{
		int stimuliCount = Random.Range( 1,5 );
		for( int i = 0; i < stimuliCount; ++i )
		{
			activateStimuli.Add( Stimulus.GenerateRandom() );
		}

		// activateCond = ( ActivateCondition )Random.Range( 0,( int )ActivateCondition.Count );

		int actionCount = 1;
		for( int i = 0; i < actionCount; ++i )
		{
			actions.Add( Action.GenerateRandom() );
		}
	}

	public void OverwriteStimulus( Stimulus stim )
	{
		activateStimuli.Clear();
		activateStimuli.Add( stim );
	}

	public bool CheckStimulus( List<Stimulus> stimuli )
	{
		return( CheckActivate( stimuli ) );
	}

	bool CheckActivate( List<Stimulus> stimuli )
	{
		bool satisfied = false;

		switch( activateCond )
		{
			case ActivateCondition.All:
				int requiredMatches = activateStimuli.Count;
				if( stimuli.Count == activateStimuli.Count )
				{
					int curMatches = 0;
					foreach( var stim in stimuli )
					{
						foreach( var myStim in activateStimuli )
						{
							if( stim.CheckEqual( myStim ) )
							{
								++curMatches;
								break;
							}
						}
					}
					satisfied = ( curMatches == requiredMatches );
				}
				else satisfied = false;
				break;
			case ActivateCondition.Either:
				foreach( var stim in stimuli )
				{
					foreach( var myStim in activateStimuli )
					{
						if( stim.CheckEqual( myStim ) )
						{
							satisfied = true;
							break;
						}
					}
				}
				break;
		}

		return( satisfied );
	}

	public List<Action> GetActions()
	{
		return( actions );
	}

	List<Stimulus> activateStimuli = new List<Stimulus>();
	ActivateCondition activateCond = ActivateCondition.Either;
	List<Action> actions = new List<Action>();
}