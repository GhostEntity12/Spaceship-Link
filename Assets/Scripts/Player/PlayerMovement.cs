using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    Vector2 movementVector;
    Vector2 lookVector;
    public float maxSpeed = 5;
    public float rotateSpeed = 5;
    new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector3(movementVector.x, 0, movementVector.y) * maxSpeed;
        if (lookVector != Vector2.zero)
        {
            rigidbody.MoveRotation(Quaternion.RotateTowards(rigidbody.rotation, Quaternion.LookRotation(new Vector3(lookVector.x, 0, lookVector.y)), rotateSpeed));
        }
    }

    public void OnMove(CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
    }
    public void OnLook(CallbackContext context)
    {
        lookVector = context.ReadValue<Vector2>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {

        }
    }
}
