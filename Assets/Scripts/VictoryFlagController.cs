using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryFlagController : MonoBehaviour 
{

	private Animator _animator;

	void Awake()
	{
		_animator = GetComponent<Animator> ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
			TriggerVictory ();	
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
			_animator.SetBool ("IsVictory", false);
	}

	public void TriggerVictory()
	{
		_animator.SetBool ("IsVictory", true);
	}

}
