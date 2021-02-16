using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;


public class PlayerMovement : MonoBehaviour
{
    Vector2 movementVector;
    Vector2 lookVector;
    Quaternion targetRotation;
    public float maxSpeed = 5;
    public float rotateSpeed = 5;
    new Rigidbody rigidbody;
    PlayerInput playerInput;

    bool isKeyboard;
    Camera c;
    float pHeight;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody>();
        isKeyboard = playerInput.currentControlScheme == "KBM";
        c = Camera.main;
        pHeight = c.transform.position.y - transform.position.y;
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector3(movementVector.x, 0, movementVector.y) * maxSpeed;

        if (isKeyboard)
        {
            Vector3 worldPos = c.ScreenToWorldPoint(new Vector3(lookVector.x, lookVector.y, pHeight));
            float dx = worldPos.x - transform.position.x;
            float dz = worldPos.z - transform.position.z;
            float angle = 90 - (Mathf.Atan2(dz, dx) * Mathf.Rad2Deg);
            targetRotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
        else
        {
            if (lookVector != Vector2.zero)
            {
                targetRotation = Quaternion.LookRotation(new Vector3(lookVector.x, 0, lookVector.y));
            }
        }

        rigidbody.MoveRotation(Quaternion.RotateTowards(rigidbody.rotation, targetRotation.normalized, rotateSpeed));
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
