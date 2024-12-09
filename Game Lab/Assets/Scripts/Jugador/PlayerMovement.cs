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

    /*Variables de gestion de la cantidad de bombas maximas, disponibles 
     y el cooldown del lanzamiento de bombas para evitar que el jugador lanze demasiadas */
    public int bombasMaximas = 0; //no es movimiento pero de esta clase deberia ser playerController
    public int bombasDisponibles = 0;
    private float groundCooldown = 0.7f;
    private float siguienteLanzamiento = 0f;

    public Vector2 anticaidasPosition;

    /*Todo variables de movimiento horizontal menos fuerzaSalto obviamente*/
    public float aceleracionMax = 5f;
    public float aceleracionMaxAire = 2f;
    public float deceleracionMax = 6f;
    public float deceleracionMaxAire = 3f;
    public float velocidadMaxGiro = 7f;
    public float velocidadMaxGiroAire = 4f;
    public float velocidadX = 7f; //valor modificable para la velocidad horizontal del jugador
    public float velocidadMax = 20f; //la velocidad(horizontal) del jugador nunca puede superior a velocidadMax
    public float fuerzaSalto = 13f; //valor modificable para el salto del jugador

    private float aceleracion;
    private float deceleracion;
    private float velocidadGiro;
    private float maxSpeedChange;
    [SerializeField] private LayerMask jumpableGround; //esto solo es para definir sobre que layers puede saltar el jugador

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        bombasDisponibles = bombasMaximas;
        if(GameManager.controlMando){
            playerInput.SwitchCurrentControlScheme("Gamepad",Gamepad.all[0]);
            
        } else {
            playerInput.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
        }
    }

    // Update is called once per frame

    void Update()
    {
        if(!GameManager.pausado){
            input = playerInput.actions["Move"].ReadValue<Vector2>();
        
            // Velocidad deseada en ambos ejes
            Vector2 desiredVelocity = new Vector2(input.x * velocidadX, player.velocity.y);
            Vector2 velocity = player.velocity; // Velocidad actual del jugador
            velocidadUI.text = "Velocidad: " + (int)velocity.x;

            // Establecemos valores de aceleración, desaceleración y velocidad de giro según si está en el suelo o en el aire
            aceleracion = IsGrounded() ? aceleracionMax : aceleracionMaxAire;
            deceleracion = IsGrounded() ? deceleracionMax : deceleracionMaxAire;
            velocidadGiro = IsGrounded() ? velocidadMaxGiro : velocidadMaxGiroAire;

            // Movimiento en el eje X
            if (input.x != 0)
            {
                if (Mathf.Sign(input.x) != Mathf.Sign(velocity.x)) // Si está girando (cambiando de dirección)
                {
                    maxSpeedChange = velocidadGiro * Time.deltaTime;
                }
                else if (IsGrounded() && Mathf.Abs(velocity.x) > velocidadX) // Si está en el suelo y esta siendo impulsado por una bomba, decelera
                {
                    maxSpeedChange = deceleracion * Time.deltaTime;
                }else if (IsGrounded()) //Si está moviendose normal en el suelo, acelera
                {
                    maxSpeedChange = aceleracion * Time.deltaTime;
                }
                else // Aceleración en el aire
                {
                    // Si el jugador está en el aire y su velocidad supera la máxima en la dirección del input, no desacelera
                    if (Mathf.Abs(velocity.x) > velocidadX && Mathf.Sign(velocity.x) == Mathf.Sign(input.x))
                    {
                        maxSpeedChange = 0; // No desacelerar si sigue la dirección del movimiento actual
                    }
                    else
                    {
                        maxSpeedChange = aceleracion * Time.deltaTime; // Aceleración normal si está por debajo de la velocidad máxima
                    }
                }
            }
            else
            {
                // Deceleración solo si no hay input horizontal
                maxSpeedChange = IsGrounded() ? deceleracion * Time.deltaTime : 0;
            }

            // Suavizamos la transición de la velocidad actual a la velocidad deseada en X
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            // Limitar la velocidad máxima a velocidadMax cuando no hay input
            if (Mathf.Abs(velocity.x) > velocidadMax)
            {
                velocity.x = Mathf.Sign(velocity.x) * velocidadMax;
            }

            // Actualizamos la velocidad del jugador
            player.velocity = velocity;

            // Comprobar si está en el suelo para saltar
            if (IsGrounded() && bombasDisponibles != bombasMaximas && CheckCooldown())
            {
                IgualarBombas();
            }
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
    public void Jump(InputAction.CallbackContext callbackContext){
        if (!GameManager.pausado && IsGrounded() && callbackContext.performed)
        {
            player.velocity = new Vector2(player.velocity.x, fuerzaSalto);
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
        bombasUI.text = "Bombas: " + bombasDisponibles;
        Debug.Log("Bombas disponibles: " + bombasDisponibles);
    }

    public void QuitarBombaDisponible()
    {
        bombasDisponibles--;
        bombasUI.text = "Bombas: " + bombasDisponibles;
        Debug.Log("Bombas disponibles: " + bombasDisponibles);
    }
    
    public void IgualarBombas()
    {
        bombasDisponibles = bombasMaximas;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void DeviceLost(PlayerInput playerInput){
        GameManager.controlMando = false;
        playerInput.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
    }
}
