using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootControl: MonoBehaviour
{
    [Header("Preset Fields")]
    [SerializeField] public Camera PlayerCam;
    [SerializeField] GameObject HitEffect;
    //[SerializeField] GameObject bullet;

    [Header("Settings")]
    [SerializeField][Range(15f, 50f)] private float damage = 25.0f;
    [SerializeField] private float range = 100.0f;
    [SerializeField] public GameObject muzzle;

    public AudioSource gunAudioSource;

    void Start()
    {
        gunAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    
    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(PlayerCam.transform.position, PlayerCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Enemy target = hit.transform.GetComponent<Enemy>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            /*
            Vector3 dir = hit.transform.position - muzzle.transform.position;
            GameObject bulletClone = Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);
            bulletClone.GetComponent<Rigidbody>().velocity = dir * 5.0f;
            Destroy(bulletClone, 1.0f);
            */
            GameObject effect = Instantiate(HitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(effect, 1.0f);
            gunAudioSource.Play();
        }
    }
}