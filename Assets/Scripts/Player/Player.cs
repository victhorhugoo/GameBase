using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public CharacterController characterController;
    public float speed = 1f;
    public float turnSpeed = 1f;
    public float gravity = -9.8f;
    public float jumpHeight = 2f;

    [Header("Run Setup")]
    public KeyCode KeyRun = KeyCode.LeftShift;
    public float speedRun = 1.5f;

    private float vSpeed = 0f;

    private void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);

        var inputAxisVertical = Input.GetAxis("Vertical");
        var speedVector = transform.forward * inputAxisVertical * speed;

        bool isGrounded = characterController.isGrounded;

        if (isGrounded)
        {
            if (vSpeed < 0)
                vSpeed = -1f; // mantém grudado no chão

            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                vSpeed = jumpHeight;
                //animator.SetTrigger("Jump");
            }
        }
        else
        {
            vSpeed -= gravity * Time.deltaTime;
        }
        speedVector.y = vSpeed;

        var isWalking = inputAxisVertical != 0;
        if (isWalking)
        {
            if (Input.GetKey(KeyRun))
            {
                speedVector *= speedRun;
                animator.speed = speedRun;

            }
            else
            {
                animator.speed = 1f;
            }
        }

        characterController.Move(speedVector * Time.deltaTime);

        if(inputAxisVertical != 0)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

}
