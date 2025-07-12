using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GameObject bullet = Instantiate(bulletPrefab, ray.origin, Quaternion.identity);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = ray.direction * bulletSpeed;
        }
    }
}
