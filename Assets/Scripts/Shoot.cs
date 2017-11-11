using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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


    public float _shootAnimLenght = 1.0f;
    
    private bool _canShoot = false;
    private bool shooted = false;


    void Update()
    {
        _canShoot = GameManager.gameManager._canShoot;                  //TODO:Shoot on click instead of waiting _shootTimer to reach 0, but maintain shoot on button hold
        if (_canShoot && !shooted)
        {
            ShootProjectile(FirePoints[0]);
            shooted = true;
        }
    }

    private void ShootProjectile(Transform firePoint)
    {
        GameObject projectileClone = Instantiate(Projectile, firePoint.position, Quaternion.AngleAxis(-90.0f, Vector3.forward));
        projectileClone.transform.SetParent(null);

        Rigidbody2D projectileRigidbody = projectileClone.GetComponent<Rigidbody2D>();
        projectileRigidbody.AddForce(transform.up * Force, ForceMode2D.Impulse);
        StartCoroutine(Wait());
    }

    IEnumerator<WaitForSeconds> Wait()
    {
        yield return new WaitForSeconds(_shootAnimLenght);
        shooted = false;
    }
}
