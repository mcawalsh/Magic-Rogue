using UnityEngine;
using System.Collections;

public class GuardScript : MonoBehaviour {

    [SerializeField] float      speed = 1.0f;
    [SerializeField] float      jumpForce = 4.0f;

    private float               inputX, inputY;
    private Animator            animator;
    private Rigidbody2D         body2d;
    private bool                combatIdle = false;
    private bool                isGrounded = true;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		// -- Handle input and movement --
		inputY = Input.GetAxis("Vertical");
        inputX = Input.GetAxis("Horizontal");

		// Swap direction of sprite depending on walk direction
		float newX = 0.0f, newY = 0.0f;
		if (inputX > 0) {
			newX = -1.0f;
		}
		else if (inputX < 0) {
			newX = 1.0f;
			//transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}

		if (inputY > 0)
			newY = 1.0f;
		else
			newY = -1.0f;

		transform.localScale = new Vector3(newX, 1.0f, newY);

        // Move
        body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);

        // -- Handle Animations --
        isGrounded = IsGrounded();
        animator.SetBool("Grounded", isGrounded);

        //Death
        if (Input.GetKeyDown("k"))
            animator.SetTrigger("Death");
        //Hurt
        else if (Input.GetKeyDown("h"))
            animator.SetTrigger("Hurt");
        //Recover
        else if (Input.GetKeyDown("r"))
            animator.SetTrigger("Recover");
        //Change between idle and combat idle
        else if (Input.GetKeyDown("i"))
            combatIdle = !combatIdle;



        //Attack
        else if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

        //Jump
        else if (Input.GetKeyDown("space") && isGrounded)
        {
            animator.SetTrigger("Jump");
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
        }

        //Walk
        else if (Mathf.Abs(inputX) > Mathf.Epsilon && isGrounded)
            animator.SetInteger("AnimState", 2);
        //Combat idle
        else if (combatIdle)
            animator.SetInteger("AnimState", 1);
        //Idle
        else
            animator.SetInteger("AnimState", 0);
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector3.up, 0.03f);
    }
}
