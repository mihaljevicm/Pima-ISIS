using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;

    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject BulletParent;
    public float ShootAnimTime = 1.0f;
    public bool _canShoot = false;

    void Awake()
    {
        gameManager = this;

        
    }
    private void Start()
    {
        ShootAnimTime = Player.GetComponent<PlayerController>()._shootTime;
        BulletParent.GetComponent<Shoot>()._shootAnimLenght = ShootAnimTime;
    }
}