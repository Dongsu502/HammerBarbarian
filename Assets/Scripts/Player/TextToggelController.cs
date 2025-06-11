using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TextToggelController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private int minCount;
    [SerializeField] private int maxCount;

    public void ToggleText(int min, int max)
    {
        UIWhiteBox.StartScripting(min, max);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleText(minCount, maxCount);
            Collider.Destroy(this.gameObject);
        }
    }
}
