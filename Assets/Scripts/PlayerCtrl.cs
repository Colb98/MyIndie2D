using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator a;
    private Transform tr;
    public GameObject player;
   
    private bool isHoldJump = false;
    public float jumpCharge = 0f;
    private int jumpForceX = 0;
    public float dForceX = 200;
    public float dForceY = 150;
    public float chargeTime = 2;
    public bool cheat = false;

    public float cheatJumpForce = -1;
    public Vector3 lastGroundPos;
    private float lastTouchGround;
    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        sr = player.GetComponent<SpriteRenderer>();
        a = player.GetComponent<Animator>();
        tr = transform;

        rb.isKinematic = cheat;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        bool isJumping = a.GetBool("OnAir");
        isJumping = isJumping & Mathf.Abs(rb.velocity.y) > 0.00005;

        if (cheat)
        {
            Vector2 pos = transform.position;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                if (Input.GetKey(KeyCode.UpArrow))
                    pos.y += 0.2f;
                else
                    pos.y -= 0.2f;
            }

            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
            {
                Vector2 velocity = new Vector2(0, 0);
                Vector3 scale = tr.localScale;
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    scale.x = -Mathf.Abs(scale.x);
                    pos.x -= 0.1f;
                }
                else
                {
                    scale.x = Mathf.Abs(scale.x);
                    pos.x += 0.1f;
                }

                tr.localScale = scale;
                rb.velocity = velocity;
                a.SetBool("Walk", true);
            }

            transform.position = pos;

            return;
        }

        if (!isHoldJump && !isJumping)
        {
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && !Input.GetKey(KeyCode.Space))
            {
                Vector3 pos = transform.position;
                Vector3 scale = tr.localScale;
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    pos.x += -0.05f;
                    scale.x = -Mathf.Abs(scale.x);
                }
                else
                {
                    pos.x += 0.05f;
                    scale.x = Mathf.Abs(scale.x);
                }

                tr.localScale = scale;
                //rb.velocity = velocity;
                transform.position = pos;
                a.SetBool("Walk", true);
            }

            else
            {
                a.SetBool("Walk", false);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isHoldJump = true;
                    a.SetBool("HoldJump", true);
                }
            }
        }
        else if (!isJumping)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                jumpForceX = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                jumpForceX = 1;
            }
            else
            {
                jumpForceX = 0;
            }

            float fJumpForceX = jumpForceX * dForceX;

            if (Input.GetKeyUp(KeyCode.Space))
            {
                Jump(fJumpForceX);
            }
            else
            {
                jumpCharge += Time.deltaTime;
                if(jumpCharge > chargeTime)
                {
                    Jump(fJumpForceX);
                }
            }
        }
        // Jumping
        else
        {
            // Stuck more than 6s in the air
            if(Time.time - lastTouchGround > 6)
            {
                //Debug.Log("Back to pos: " + Utils.VectorToString(lastGroundPos));
                lastGroundPos.y += 2;
                transform.position = lastGroundPos;
                rb.velocity.Set(0, 0);
                lastTouchGround = Time.time;
            }
        }
    }

    private void Jump(float forceX)
    {
        bool isLeft = tr.localScale.x < 0;
        bool isJumpLeft = forceX < 0;
        isHoldJump = false;
        a.SetBool("OnAir", true);
        a.SetBool("HoldJump", false);
        a.SetBool("BackJump", isLeft ^ isJumpLeft);
        if (cheatJumpForce > -1)
            jumpCharge = cheatJumpForce;
        rb.AddForce(new Vector2(forceX, Mathf.CeilToInt(jumpCharge *  4 / chargeTime) * dForceY * chargeTime / 4));
        jumpCharge = 0f;


        //Debug.Log("leave ground at " + Utils.VectorToString(transform.position));
        lastGroundPos = transform.position;
        lastTouchGround = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 contactPoint = collision.contacts[0].point;
        Vector3 center = transform.position;

        //Debug.Log("Contact Point: " + Utils.VectorToString(contactPoint) + " " + Utils.VectorToString(center));
        // If contact point is below
        if (contactPoint.y < center.y - 0.45f)
        {
            a.SetBool("OnAir", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        a.SetBool("OnAir", true);
    }
}
