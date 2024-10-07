using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D player;
    private BoxCollider2D coll;
    private SpriteRenderer sp;
    public float velocidadX = 7f; //valor modificable para la velocidad horizontal del jugador
    public float velocidadY = 13f; //valor modificable para el salto del jugador
    [SerializeField] private LayerMask jumpableGround;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal"); //dirección horizontal del jugador
        player.velocity = new Vector2(dirX * velocidadX, player.velocity.y);
        if (IsGrounded() && Input.GetButtonDown("Jump")){
            player.velocity = new Vector2(player.velocity.x, velocidadY);
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
