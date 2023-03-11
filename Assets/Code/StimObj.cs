using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimObj
	:
	MonoBehaviour
{
	void Start()
	{
		StimHandler.AddSelf( this );
	}

	public void Despawn()
	{
		StimHandler.RemoveSelf( this );

		Destroy( gameObject );
	}

	[SerializeField] public Stimulus.Item stimType = Stimulus.Item.Count;
}