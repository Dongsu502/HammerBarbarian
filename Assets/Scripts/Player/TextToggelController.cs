using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TextToggelController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIWhiteBox.StartScripting(1100, 1104);
            Collider.Destroy(this.gameObject);
        }
    }
}
