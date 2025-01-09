using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D player;
    public BoxCollider2D coll;
    private SpriteRenderer sp;

    public TMP_Text bombasUI;
    public TMP_Text velocidadUI;

    private PlayerInput playerInput;
    private Vector2 input;

    private bool enZonaBombasInfinitas = false;

    public int bombasMaximas = 0;
    public int bombasDisponibles = 0;
    private float groundCooldown = 0.7f;
    private float siguienteLanzamiento = 0f;

    public Vector2 anticaidasPosition;
    public Vector2 respawnPosition;
    private Vector2 initialPos;
    private Animator animator;

    public float aceleracionMax = 5f;
    public float aceleracionMaxAire = 2f;
    public float deceleracionMax = 6f;
    public float deceleracionMaxAire = 3f;
    public float velocidadMaxGiro = 7f;
    public float velocidadMaxGiroAire = 4f;
    public float velocidadX = 7f; // Velocidad horizontal base
    public float velocidadMaxX = 20f; // Velocidad máxima horizontal
    public float velocidadMaxY = 15f; // Velocidad máxima vertical al subir
    public float fuerzaSalto = 13f; // Fuerza de salto

    private float aceleracion;
    private float deceleracion;
    private float velocidadGiro;
    private float maxSpeedChange;
    [SerializeField] private LayerMask jumpableGround;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>(); 
        bombasMaximas = 0;
        bombasDisponibles = bombasMaximas;
        respawnPosition = player.transform.position;
        respawnPosition = initialPos;
        if (GameManager.controlMando)
        {
            playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);
        }
        else
        {
            playerInput.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
        }
    }

    void Update()
    {
        if (!GameManager.pausado)
        {
            input = playerInput.actions["Move"].ReadValue<Vector2>();

            bool isRunning = Mathf.Abs(input.x) > 0.1f && IsGrounded();
            bool isJumping = !IsGrounded();
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isJumping", isJumping);

            // Velocidad deseada en ambos ejes
            Vector2 desiredVelocity = new Vector2(input.x * velocidadX, player.velocity.y);
            Vector2 velocity = player.velocity;
            velocidadUI.text = $"Velocidad: {(int)velocity.x}";

            // Establecer aceleración, desaceleración y giro según el estado
            aceleracion = IsGrounded() ? aceleracionMax : aceleracionMaxAire;
            deceleracion = IsGrounded() ? deceleracionMax : deceleracionMaxAire;
            velocidadGiro = IsGrounded() ? velocidadMaxGiro : velocidadMaxGiroAire;

            if (input.x < 0)  {
            animator.SetBool("isFacingLeft", true);
            sp.flipX = true; // Voltea el sprite horizontalmente
            }
            else if (input.x > 0) {
            animator.SetBool("isFacingLeft", false);
            sp.flipX = false; // Vuelve el sprite a su posición original
            }

            // Movimiento en el eje X
            if (input.x != 0)
            {
                if (Mathf.Sign(input.x) != Mathf.Sign(velocity.x)) // Cambiando de dirección
                {
                    maxSpeedChange = velocidadGiro * Time.deltaTime;
                }
                else if (IsGrounded() && Mathf.Abs(velocity.x) > velocidadX) // Impulsado en el suelo
                {
                    maxSpeedChange = deceleracion * Time.deltaTime;
                }
                else if (IsGrounded()) // Movimiento normal en el suelo
                {
                    maxSpeedChange = aceleracion * Time.deltaTime;
                }
                else // Movimiento en el aire
                {
                    if (Mathf.Abs(velocity.x) > velocidadX && Mathf.Sign(velocity.x) == Mathf.Sign(input.x))
                    {
                        maxSpeedChange = 0; // No desacelera en el aire si sigue la dirección del input
                    }
                    else
                    {
                        maxSpeedChange = aceleracion * Time.deltaTime; // Aceleración en el aire
                    }
                }
            }
            else
            {
                maxSpeedChange = IsGrounded() ? deceleracion * Time.deltaTime : 0;
            }

            // Aplicar límite de velocidad máxima horizontal
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            if (Mathf.Abs(velocity.x) > velocidadMaxX)
            {
                velocity.x = Mathf.Sign(velocity.x) * velocidadMaxX;
            }

            // Aplicar límite de velocidad máxima vertical solo al subir
            if (velocity.y > velocidadMaxY) // Solo limita si está subiendo
            {
                velocity.y = velocidadMaxY;
            }

            // Actualizar la velocidad del jugador
            player.velocity = velocity;

            // Comprobar si está en el suelo para recargar bombas
            if (IsGrounded() && bombasDisponibles != bombasMaximas && CheckCooldown())
            {
                IgualarBombas();
            }

            // Actualizar la posición de anticaídas si está en el suelo
            if (IsGrounded())
            {
                anticaidasPosition = transform.position;
            }
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (!GameManager.pausado && IsGrounded() && callbackContext.performed)
        {
            player.velocity = new Vector2(player.velocity.x, fuerzaSalto);
            animator.SetBool("isJumping", true);

        }
    }


//Comprueba si el jugador esta cayendo
public bool IsFalling()
    {
        if(player.velocity.y < 0)
        {
            return true;
        }
        return false;
    }
    //De momento solo sirve para reiniciar la velocidad vertical si el jugador esta cayendo cuando le va a impulsar una bomba
    public void ResetIfFalling()
    {
        if (IsFalling()){
            player.velocity = new Vector2(player.velocity.x, 0f);
        }
    }
    //serie de métodos para la gestion de bombas máximas y bombas disponibles
    public void AumentarBombasMaximas()
    {
        bombasMaximas++;
        bombasUI.text = "Bombas: " + bombasDisponibles;
        Debug.Log("Bombas maximas: " + bombasMaximas);
    }

    /* Podra ser util para proximas actualizaciones de codigo*/
    public void AumentarBombaDisponible()
    {
        bombasDisponibles++;
        if(!enZonaBombasInfinitas)
            bombasUI.text = "Bombas: " + bombasDisponibles;
        Debug.Log("Bombas disponibles: " + bombasDisponibles);
    }

    public void QuitarBombaDisponible()
    {
        bombasDisponibles--;
        if(!enZonaBombasInfinitas)
            bombasUI.text = "Bombas: " + bombasDisponibles;
        Debug.Log("Bombas disponibles: " + bombasDisponibles);
    }
    
    public void IgualarBombas()
    {
        bombasDisponibles = bombasMaximas;
        if (!enZonaBombasInfinitas)
            bombasUI.text = "Bombas: " + bombasDisponibles;
    }
    //Cooldown para el lanzamiento de bombas cuando el jugador no está en el aire
    public bool CheckCooldown()
    {
        if (Time.time > siguienteLanzamiento)
        {
            siguienteLanzamiento = Time.time + groundCooldown;
            return true;
        }
        else
            return false;
    }
    //Matar al jugador
    public void Muere()
    {
        if (respawnPosition == initialPos)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else
        {
            player.transform.position = respawnPosition;
            player.velocity = new Vector2(0, 0);
        }
    }
    public void DeviceLost(PlayerInput playerInput){
        GameManager.controlMando = false;
        playerInput.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
    }
    public void EnZonaBombasInfinitas()
    {
        enZonaBombasInfinitas = !enZonaBombasInfinitas;
    }
}
