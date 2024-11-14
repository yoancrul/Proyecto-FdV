using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D player;
    private BoxCollider2D coll;
    private SpriteRenderer sp;

    public int bombasMaximas = 0; //no es movimiento pero de esta clase deberia ser playerController
    public int bombasDisponibles = 0;

    public float aceleracionMax = 5f;
    public float aceleracionMaxAire = 2f;
    public float deceleracionMax = 6f;
    public float deceleracionMaxAire = 3f;
    public float velocidadMaxGiro = 7f;
    public float velocidadMaxGiroAire = 4f;

    public float velocidadX = 7f; //valor modificable para la velocidad horizontal del jugador
    public float fuerzaSalto = 13f; //valor modificable para el salto del jugador
    private float aceleracion;
    private float deceleracion;
    private float velocidadGiro;
    private float maxSpeedChange;
    [SerializeField] private LayerMask jumpableGround;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        bombasDisponibles = bombasMaximas;
    }
    private void FixedUpdate()
    {
        
    }
    // Update is called once per frame

    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal"); // dirección horizontal del jugador

        // Velocidad deseada en ambos ejes
        Vector2 desiredVelocity = new Vector2(dirX * velocidadX, player.velocity.y);
        Vector2 velocity = player.velocity; // Velocidad actual del jugador

        // Establecemos valores de aceleración, desaceleración y velocidad de giro según si está en el suelo o en el aire
        aceleracion = IsGrounded() ? aceleracionMax : aceleracionMaxAire;
        deceleracion = IsGrounded() ? deceleracionMax : deceleracionMaxAire;
        velocidadGiro = IsGrounded() ? velocidadMaxGiro : velocidadMaxGiroAire;

        // Movimiento en el eje X
        if (dirX != 0)
        {
            if (Mathf.Sign(dirX) != Mathf.Sign(velocity.x)) // Si está girando
            {
                maxSpeedChange = velocidadGiro * Time.deltaTime;
            }
            else
            {
                maxSpeedChange = aceleracion * Time.deltaTime;
            }
        }
        else
        {
            maxSpeedChange = deceleracion * Time.deltaTime;
        }

        // Suavizamos la transición de la velocidad actual a la velocidad deseada en X
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        // Actualizamos la velocidad del jugador
        player.velocity = velocity;

        // Comprobar si está en el suelo para saltar
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            player.velocity = new Vector2(player.velocity.x, fuerzaSalto);
        }
        if (IsGrounded() && bombasDisponibles != bombasMaximas)
        {
            IgualarBombas();
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    //serie de métodos para la gestion de bombas máximas y bombas disponibles
    public void AumentarBombasMaximas()
    {
        bombasMaximas++;
        Debug.Log("Bombas maximas: " + bombasMaximas);
    }
    public void AumentarBombaDisponible()
    {
        bombasDisponibles++;
        Debug.Log("Bombas disponibles: " + bombasDisponibles);
    }
    public void QuitarBombaDisponible()
    {
        bombasDisponibles--;
        Debug.Log("Bombas disponibles: " + bombasDisponibles);
    }
    public void IgualarBombas()
    {
        bombasDisponibles = bombasMaximas;
    }
}
