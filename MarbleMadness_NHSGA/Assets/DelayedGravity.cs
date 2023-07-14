using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DelayedGravity : MonoBehaviour
{

    Keyboard keyboard;
    Vector3 origin;

    void Start()
    {
        keyboard = Keyboard.current;
        origin = gameObject.transform.position;
    }

    void Update()
    {
        if (keyboard.spaceKey.isPressed)
        {
            gameObject.transform.position = origin;
        }
    }

}
