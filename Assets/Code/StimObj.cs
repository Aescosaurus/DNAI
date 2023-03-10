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

	[SerializeField] public Stimulus.Item stimType = Stimulus.Item.Count;
}