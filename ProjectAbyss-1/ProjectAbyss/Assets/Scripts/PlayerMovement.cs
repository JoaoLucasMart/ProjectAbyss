﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        
        public CharacterController2D controller;
        public Animator animator;
        private Rigidbody2D rb;
        public LayerMask whatIsLadder;
        public float distance;
        public float climbSpeed;
        public float runSpeed = 40f;
        float HorizontalMove = 0f;
        bool jump = false;
        bool crouch = false;
        int jumpCount = 1;
        private float inputVertical;
        private bool isClimbing;

        // Update is called once per frame

        void Start()
        {
            //controller = new CharacterController2D();
            rb = controller.m_Rigidbody2D;
        }
        void Update()
        {
            // GetComponent<Grab>().isHolding = true;
            HorizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            animator.SetFloat("Speed", Mathf.Abs(HorizontalMove));

            if (Input.GetButtonDown("Jump") && jumpCount >= 1)
            {
                
                jump = true;
                jumpCount -= 2;
                animator.SetBool("IsJumping", true);
                
            }
            if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;
                animator.SetBool("IsCrouching",true);
            } else if (Input.GetButtonUp("Crouch"))
            {
                animator.SetBool("IsCrouching", false);
                crouch = false;
                
            }
            /*
            if (GetComponent<Grab>().isHolding == true)
            {
                animator.SetBool("IsGrabbing", true);
            }
            else
            {
                animator.SetBool("IsGrabbing", false);
            }*/
        }

        public void OnLanding()
        {
            animator.SetBool("IsJumping", false);

            jumpCount = 1;
            
            
        }
        private void FixedUpdate()
        {
            //move character
            controller.Move(HorizontalMove * Time.fixedDeltaTime, crouch, jump);
            jump = false;
            if(GetComponent<CharacterController2D>().m_Grounded == false)
            {
                jumpCount = 0;
            }

            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
            if(hitInfo.collider != null)
            {
                if(Input.GetKeyDown(KeyCode.W))
                {
                    isClimbing = true;
                }
                else
                {
                    isClimbing = false;
                }

            }

            if(isClimbing == true )
            {
                Debug.Log("is climbing");
                inputVertical = Input.GetAxisRaw("Vertical");
                rb.velocity = new Vector2(rb.velocity.x, inputVertical * climbSpeed);
                rb.gravityScale = -3;
            }
            else
            {
                rb.gravityScale = 3;
                
            }
        }
    }
}