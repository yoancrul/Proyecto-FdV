using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D player;
    private Animator anim;
    private SpriteRenderer sp;
    private int jumps = 2;
    private bool isJumping;
    private int timer = 0;
    [SerializeField] private LayerMask jumpableGround;
    private enum Animation { idle, run, jump, fall, doublejump};
    bool jump = true;
    private BoxCollider2D coll;
    // Start is called before the first frame update
    private void Start()
    {
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        Animation state;
        float dirX = Input.GetAxisRaw("Horizontal");
        player.velocity = new Vector2(dirX * 7f, player.velocity.y);
        if (Input.GetKey("left shift")){
            player.velocity = new Vector2(player.velocity.x * 1.5f, player.velocity.y);
        }
        if (IsGrounded() && timer <= 0)
        {
            jumps = 2;
            isJumping = false;
            timer = 30;
        }
        if(jumps==2 && !IsGrounded())
        {
            jumps--;
        }
        if (isJumping && timer > 0)
        {
            timer--;
        }
        if (Input.GetButtonDown("Jump") && jumps > 0 && !isJumping)
        {
            player.velocity = new Vector2(player.velocity.x, 13f);
            isJumping = true;
            jumps -= 1;
        } else if(Input.GetButtonDown("Jump") && jumps > 0 && isJumping)
        {
            player.velocity = new Vector2(player.velocity.x, 13f);
            jumps -= 1;
        }
        if(dirX > 0f)
        {
            state = Animation.run;
            sp.flipX = false;
        }else if(dirX < 0f)
        {
            state = Animation.run;
            sp.flipX = true;
        }
        else
        {
            state = Animation.idle;
        }
        if(player.velocity.y > .1f && jumps == 1)
        {
            state = Animation.jump;
        }else if(player.velocity.y > .1f && jumps == 0)
        {
            state = Animation.doublejump;
        }
        else if(player.velocity.y < 0f)
        {
            state = Animation.fall;
        }

        anim.SetInteger("state", (int) state);
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
