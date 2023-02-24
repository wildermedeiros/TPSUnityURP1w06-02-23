using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform barrelTransform;
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 30f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] StarterAssetsInputs inputs;
    [SerializeField] UnityEvent onShoot;
    
    Transform myCamera;
    bool canShoot = true;

    private void Awake() {
        myCamera = Camera.main.transform;
    }

    private void OnEnable()
    {
        canShoot = true;
    }

    void Update()
    {
        transform.forward = myCamera.forward;
        DisplayAmmo();
        if (inputs.shoot && canShoot == true)
        {
            StartCoroutine(Shoot());
        }
    }

    private void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
        ammoText.text = currentAmmo.ToString();
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        inputs.shoot = false;
        if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
        {
            PlayMuzzleFlash();
            ProcessRaycast();
            PlaySoundEffect();
            ammoSlot.ReduceCurrentAmmo(ammoType);
        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    private void ProcessRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(barrelTransform.position, barrelTransform.forward, out hit, range))
        {
            CreateHitImpact(hit);
            //EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            // if (target == null) return;
            // target.TakeDamage(damage);
        }
        else
        {
            return;
        }
    }

    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, impact.GetComponent<ParticleSystem>().main.duration);
    }

    private void PlaySoundEffect()
    {
        onShoot.Invoke();
    }

}
