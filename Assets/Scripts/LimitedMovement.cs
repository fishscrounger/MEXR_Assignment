using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LimitedMovement : MonoBehaviour
{
    public float MinX;
    public float MinY;

    public float MaxX;
    public float MaxY;

    public float Speed;

    private Vector2 MovementInput;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(MovementInput.magnitude > 0.0f)
        {
            transform.position += new Vector3(MovementInput.x * Time.deltaTime * Speed, MovementInput.y * Time.deltaTime * Speed, 0.0f);

            if (transform.position.x > MaxX)
                transform.position = new Vector3(MaxX, transform.position.y, transform.position.z);
            else
                if (transform.position.x < MinX)
                transform.position = new Vector3(MinX, transform.position.y, transform.position.z);

            if (transform.position.y > MaxY)
                transform.position = new Vector3(transform.position.x, MaxY, transform.position.z);
            else
                if (transform.position.y < MinY)
                transform.position = new Vector3(transform.position.x, MinY, transform.position.z);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }
}
