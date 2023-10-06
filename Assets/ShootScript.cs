using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShootScript : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        bullet.SetActive(true);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        bulletRb.velocity = bulletSpawnPoint.forward * 100.0f;

        audioSource.Play();

        Destroy(bullet, 2.0f);
    }
}
