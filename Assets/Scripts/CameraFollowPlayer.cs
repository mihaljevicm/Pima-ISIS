using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour 
{
	public Transform Player;
	public Vector3 PositionOffset;

	private Transform _player;

	void Awake()
	{
		_player = transform;
	}

	void Update()
	{
		Vector3 newPosition = new Vector3 (Player.position.x, Player.position.y, _player.position.z) + PositionOffset;
		_player.position = newPosition;
	}
}
