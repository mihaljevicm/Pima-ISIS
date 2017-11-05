using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
	private Animator _animator;
	private ObjectMover _objectMover;

	[SerializeField]
	private bool _isStunned = false;

	public float StunDuration = 1.0f;

	private int _stunnedLayer = 0;
	private int _enemyLayer = 0;
	 
	void Awake()
	{
		_animator = GetComponent<Animator> ();
		_objectMover = GetComponentInParent<ObjectMover> ();


		_enemyLayer = LayerMask.NameToLayer ("Enemy");
		_stunnedLayer = LayerMask.NameToLayer ("Stunned");
	}

	private void ChangeLayer(int newLayer)
	{
		gameObject.layer = newLayer;
		for (int i = 0; i < transform.childCount; i++) 
		{
			transform.GetChild (i).gameObject.layer = newLayer;			
		}
	}

	public void ToggleStun(bool isStunned)
	{
		if (_isStunned && isStunned)
			return;
		_isStunned = isStunned;
		_animator.SetBool ("Stunned", _isStunned);
		_objectMover.ShouldMove = !_isStunned;

		if (_isStunned) {
			gameObject.layer = _stunnedLayer;
			Invoke ("DisableStunn", StunDuration);
		} else 
		{
			ChangeLayer (_enemyLayer);
		}


	}

	private void DisableStunn()
	{
		ToggleStun (false);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//TODO: dodati sve ostalo
		if (other.gameObject.tag=="Player")
			Debug.Log ("gsme over");
		//ToggleStun (true);
	}

}
