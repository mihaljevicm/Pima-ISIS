using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
	private Transform ProjectileParent;
    [SerializeField]
    private GameObject Projectile;

    [SerializeField]
    private List<Transform> FirePoints;

    [SerializeField]
    private float Force = 10.0f;

    [SerializeField]
    private float _shootAnimLenght = 1.0f;
    [SerializeField]
    private float _shootTimer = 1.0f;
    private bool _canShoot = false;


    void Awake()
    {
        _shootAnimLenght = GameManager.gameManager.ShootAnimTime;
        _shootTimer = _shootAnimLenght;
    }

    void Update()
	{
        _canShoot = GameManager.gameManager._canShoot;                  //TODO:Shoot on click instead of waiting _shootTimer to reach 0, but maintain shoot on button hold
		if (_canShoot) 
		{
            _shootTimer -= Time.deltaTime;
            if (_shootTimer <= 0.0f)
            {
                foreach (Transform firePoint in FirePoints)
                {
                    ShootProjectile(firePoint);
                }
                _shootTimer = _shootAnimLenght;
            }
		}
	}

	private void ShootProjectile(Transform firePoint)
	{
		GameObject projectileClone = Instantiate (Projectile, firePoint.position, Quaternion.AngleAxis(-90.0f, Vector3.forward));
		projectileClone.transform.SetParent (ProjectileParent);

		Rigidbody2D projectileRigidbody = projectileClone.GetComponent<Rigidbody2D> ();
		projectileRigidbody.AddForce (transform.up * Force, ForceMode2D.Impulse);

	}
}
