using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeWeaponCollisionHandler : MonoBehaviour
{
    private HammerThrowController controller;
    private Rigidbody hammerRb;

    private void Awake()
    {
        hammerRb = GetComponent<Rigidbody>(); 
    }
    public void SetController(HammerThrowController controller)
    {
        this.controller = controller;
    }

    private void OnCollisionEnter(Collision collision)
    {
        controller?.Recall();
    }

}
