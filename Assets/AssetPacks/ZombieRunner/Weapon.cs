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
    bool canShoot = false;

    private void Awake() {
        myCamera = Camera.main.transform;
    }

    void Update()
    {
        DisplayAmmo();
    }

    private void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
        ammoText.text = currentAmmo.ToString();
    }

    public IEnumerator Shoot()
    {
        canShoot = false;
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
        Vector2 screenCenterPoint = new Vector2(Screen.width /2f, Screen.height /2f);
        Ray rayCenterScreenPoint = Camera.main.ScreenPointToRay(screenCenterPoint);
        
        if (Physics.Raycast(rayCenterScreenPoint, out RaycastHit hit, range))
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

    public void CanShoot(bool shootStatus)
    {
        canShoot = shootStatus;
    }

    public bool GetCanShoot()
    {
        return canShoot;
    }
}
