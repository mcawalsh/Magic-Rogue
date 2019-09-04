using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed;
	private Rigidbody2D myRigidbody;
	private Animator animator;
	private Vector2 change;

    void Start()
    {
		animator = GetComponent<Animator>();
		myRigidbody = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
		change = Vector2.zero;

		change.x = Input.GetAxisRaw("Horizontal");
		change.y = Input.GetAxisRaw("Vertical");

		MovePlayer(change);
    }

	

	private void MovePlayer(Vector2 newPosition)
	{
		if (newPosition != Vector2.zero)
			myRigidbody.MovePosition((Vector2)transform.position + newPosition * speed * Time.deltaTime);

		UpdateAnimator(newPosition);
	}

	private void UpdateAnimator(Vector2 newPosition)
	{
		if (newPosition != Vector2.zero)
		{
			animator.SetFloat("moveX", newPosition.x);
			animator.SetFloat("moveY", newPosition.y);
			animator.SetBool("moving", true);
		}
		else
		{
			animator.SetBool("moving", false);
		}
	}
}
