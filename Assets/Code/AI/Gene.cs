using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

	public Gene GenerateModified()
	{
		var modified = new Gene();

		modified.activateStimuli.Clear();
		modified.activateStimuli.AddRange( activateStimuli );

		modified.activateCond = activateCond;

		modified.actions.Clear();
		modified.actions.AddRange( actions );

		var pick = Random.Range( 0,3 );
		switch( pick )
		{
			case 0:
				{
					var stimChoice = Random.Range( 0,3 );

					// prevent removing only stimulus
					while( modified.activateStimuli.Count <= 1 && stimChoice == 1 ) stimChoice = Random.Range( 0,3 );

					if( stimChoice == 0 ) // add
					{
						modified.activateStimuli.Add(
							new Stimulus( ( Stimulus.Item )Random.Range( 0,( int )Stimulus.Item.Count ),
							( Dir )Random.Range( 0,( int )Dir.Count ) ) );
					}
					else if( stimChoice == 1 ) // remove
					{
						modified.activateStimuli.RemoveAt( Random.Range( 0,modified.activateStimuli.Count ) );
					}
					else // modify
					{
						modified.activateStimuli[Random.Range( 0,modified.activateStimuli.Count )] = new Stimulus(
							( Stimulus.Item )Random.Range( 0,( int )Stimulus.Item.Count ),
							( Dir )Random.Range( 0,( int )Dir.Count ) );
					}
				}
				break;
			case 1:
				modified.activateCond = ( ActivateCondition )Random.Range( 0,( int )ActivateCondition.Count );
				break;
			case 2:
				{
					var actionChoice = Random.Range( 0,3 );
					
					while( modified.actions.Count <= 1 && actionChoice == 1 ) actionChoice = Random.Range( 0,3 );

					if( actionChoice == 0 ) // add
					{
						modified.actions.Add( new Action(
							( Action.ActionType )Random.Range( 0,( int )Action.ActionType.Count ),
							( Dir )Random.Range( 0,( int )Dir.Count ) ) );
					}
					else if( actionChoice == 1 ) // remove
					{
						modified.actions.RemoveAt( Random.Range( 0,modified.actions.Count ) );
					}
					else // modify
					{
						modified.actions[Random.Range( 0,modified.actions.Count )] = new Action(
							( Action.ActionType )Random.Range( 0,( int )Action.ActionType.Count ),
							( Dir )Random.Range( 0,( int )Dir.Count ) );
					}
				}
				break;
		}

		return( modified );
	}

	List<Stimulus> activateStimuli = new List<Stimulus>();
	ActivateCondition activateCond = ActivateCondition.Either;
	List<Action> actions = new List<Action>();
}