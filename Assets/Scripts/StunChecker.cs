using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunChecker : MonoBehaviour 
{
	private Enemy _enemy;

	void Awake()
	{
		_enemy = GetComponentInParent<Enemy> ();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player") 
		{
			_enemy.ToggleStun (true);

			//TODO make public method for jump on playermovement & use + as amplifier
			Rigidbody2D playerRigidbody2D = other.gameObject.GetComponent<Rigidbody2D> ();
			playerRigidbody2D.AddForce(Vector3.up * 500.0f);
		}
	}
}
