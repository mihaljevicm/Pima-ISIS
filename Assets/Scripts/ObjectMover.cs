using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour 
{
	public Transform ObjectToMove;
	public float MovementSpeed = 5.0f;
	public bool ShouldLoop = true;
	public float WaitAtWaipoint = 1.0f;

	public List<Transform> Waipoints = new List<Transform>();

	private int _waipointIndex = 0;
	private bool _shouldMove = true;
	private float _moveTimer = 0.0f;

	//property
	public bool ShouldMove
	{
		get
		{
			return _shouldMove;
		}
		set
		{
			_shouldMove = value;
		}
	}

	private Transform _transform;

	void Update()
	{
		if(Time.time >= _moveTimer)
		Move ();
	}

	private void Move()
	{
		if(Waipoints.Count != 0 && _shouldMove)
		{
			ObjectToMove.position = Vector3.MoveTowards (ObjectToMove.position, Waipoints [_waipointIndex].position, MovementSpeed * Time.deltaTime);	
			if (Vector3.Distance (ObjectToMove.position, Waipoints [_waipointIndex].position) <= 0.0f) 
			{
				_waipointIndex++;

				_moveTimer = Time.time + WaitAtWaipoint;
			}

			if (_waipointIndex >= Waipoints.Count) 
			{
				if (ShouldLoop)
					_waipointIndex = 0;
				else
					_shouldMove = false;
			}
		}
	}

}
