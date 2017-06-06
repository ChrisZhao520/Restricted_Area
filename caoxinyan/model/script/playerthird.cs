using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerthird : MonoBehaviour
{
    private Animation anim;
    private float tranY = 0f;
    private float walkSpeed  = 1.0f;
    private float gravity = 100.0f;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController charController ;
    void Start()
    {
        charController = GetComponent<CharacterController>();
        anim = GetComponent<Animation>();
        anim.wrapMode = WrapMode.Loop;
    }
    void Update ()
    {
        if(charController.isGrounded == true)
        {
            if(Input.GetAxis("Vertical") > 0.1)
            {
                if(Input.GetButton("Run"))
                {
                    anim.CrossFade("run");
                    walkSpeed = 4;
                }
                else
                {
                    anim["walk"].speed = 1;
                    anim.CrossFade("walk");
                    walkSpeed = 1;
                }
            }
            else if(Input.GetAxis("Vertical") < -0.1)
            {
                anim["walk"].speed = -1;
                anim.CrossFade("walk");
                walkSpeed = 1;
            }
            else
            {
                anim.CrossFade("idle");
            }
            // Create an animation cycle for when the character is turning on the spot
            if(Input.GetAxis("Horizontal")<1 && Input.GetAxis("Vertical")>1)
            {
                anim.CrossFade("walk");
            }
            
            tranY += Input.GetAxis("Horizontal");
            transform.eulerAngles = new Vector3(0,tranY,0);
            // Calculate the movement direction (forward motion)
            Vector3 a = new Vector3(0, 0, Input.GetAxis("Vertical"));
            moveDirection = a;
            moveDirection = transform.TransformDirection(moveDirection);
        }
        moveDirection.y -= gravity * Time.deltaTime;
        charController.Move(moveDirection * (Time.deltaTime * walkSpeed));
    }


}
