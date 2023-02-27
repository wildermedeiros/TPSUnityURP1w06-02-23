using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera aimCamera; 
    [SerializeField] Image crosshair = null;
    [SerializeField] Weapon weapon = null;
    [SerializeField] float normalSensitivity = 1f;
    [SerializeField] float aimSensitivity = 1f;
    [SerializeField] float characterRotationAimSpeed = 5f;
    [SerializeField] float aimAnimationSpeed = 15f;

    StarterAssetsInputs inputs;
    ThirdPersonController thirdPersonController;
    GameObject mainCamera;
    Animator animator;

    private void Awake() 
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        inputs = GetComponent<StarterAssetsInputs>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Aim();
        Shoot();
    }

    private void Aim()
    {
        if(inputs.aim)
        {
            aimCamera.Priority = +10;
            crosshair.enabled = true;
            weapon.CanShoot(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetAimingState(true);
            SetAimAnimation(1f);
            RotatesTowardsAimDirection();

        }
        else
        {
            aimCamera.Priority = -10;
            crosshair.enabled = false;
            weapon.CanShoot(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetAimingState(false);
            SetAimAnimation(0f);
            inputs.ShootInput(false);
        }
    }

    private void SetAimAnimation(float weight)
    {
        animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), weight, Time.deltaTime * aimAnimationSpeed));
    }

    private void RotatesTowardsAimDirection()
    {
        Quaternion targetRotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, characterRotationAimSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        if (inputs.shoot && weapon.GetCanShoot())
        {
            StartCoroutine(weapon.Shoot());
            inputs.ShootInput(false);
        }
    }

}
