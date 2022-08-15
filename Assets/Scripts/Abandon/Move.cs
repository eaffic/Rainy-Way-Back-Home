using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
	private Rigidbody2D rb;
	private Animator anim;
	private Collider2D coll;

	public float speed, jumpForce;
	public Transform groundCheck;
	public LayerMask ground;

	public bool isJump, isGround;

	bool jumpPressed;
	int jumpCount;

	public Transform backGround;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		coll = GetComponent<Collider2D>();
	}

	// Update is called once per frame
	void Update()
	{
		backGround.position += new Vector3(rb.velocity.x * 0.002f, rb.velocity.y * 0.002f, 0);
		if (Input.GetButtonDown("Jump") && jumpCount > 0)
		{
			jumpPressed = true;
		}
		SwitchAnim();
	}

	private void FixedUpdate()
	{
		isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
		GroundMovement();
		Jump();
	}

	void GroundMovement()
	{
		float horizontalMove = Input.GetAxisRaw("Horizontal");
		rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);


		if (horizontalMove != 0)
		{
			transform.localScale = new Vector3(horizontalMove, 1, 1);
		}
	}

	void Jump()
	{
		if (isGround)
		{
			jumpCount = 1;
			isJump = false;
		}

		if (jumpPressed && isGround)
		{
			isJump = true;
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			jumpCount--;
			jumpPressed = false;
		}
		else if (jumpPressed && jumpCount > 0 && isJump)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			jumpCount--;
			jumpPressed = false;
		}
	}

	void SwitchAnim()
	{
		anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

		if (isGround)
		{
			anim.SetBool("IsJump", false);
			anim.SetBool("IsFall", false);
		}
		else if (!isGround && rb.velocity.y > 0)
		{
			anim.SetBool("IsJump", true);
		}
		else if (rb.velocity.y <= 0)
		{
			anim.SetBool("IsJump", false);
			anim.SetBool("IsFall", true);
		}
	}
}
