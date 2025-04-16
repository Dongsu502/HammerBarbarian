using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUseItem : MonoBehaviour
{
    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int currentItemNum = UIWhiteBox.GetCurrentItemNum();

            switch (currentItemNum)
            {
                case 0:
                    Debug.Log($"{currentItemNum}번 아이템 사용");
                    break;
                case 1:
                    Debug.Log($"{currentItemNum}번 아이템 사용");
                    break;
                case 2:
                    Debug.Log($"{currentItemNum}번 아이템 사용");
                    break;
            }
        }
    }
}
