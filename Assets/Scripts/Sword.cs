using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    private Vector2 MovementInput;

    void Start()
    {

    }


    void Update()
    {
        if (MovementInput.magnitude > 0.0f)
        {
            transform.position += new Vector3(MovementInput.x * Time.deltaTime, MovementInput.y * Time.deltaTime, 0.0f);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }
}
