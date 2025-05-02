using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game;

public class PlayerUseItem : MonoBehaviour
{
    public bool useItem = false;


    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!useItem)
            {
                
            }
              
        }
    }
}
