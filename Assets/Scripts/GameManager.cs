using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;


    public float ShootAnimTime = 1.0f;
    public bool _canShoot = false;

    void Awake()
    {
        gameManager = this;
    }
}