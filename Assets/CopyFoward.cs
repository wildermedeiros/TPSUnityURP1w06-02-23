using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyFoward : MonoBehaviour
{
    [SerializeField] Transform weaponTransform;

    Vector2 screenCenterPoint;

    void Start()
    {
        screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    void Update()
    {        

    }
}
