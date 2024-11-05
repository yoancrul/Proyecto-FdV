using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D player;
    private BoxCollider2D coll;
    private SpriteRenderer sp;

    public float aceleracionMax = 5f;
    public float aceleracionMaxAire = 2f;
    public float deceleracionMax = 6f;
    public float deceleracionMaxAire = 3f;
    public float velocidadMaxGiro = 7f;
    public float velocidadMaxGiroAire = 4f;

    public float friccion = 3f;

    private float aceleracion;
    private float deceleracion;
    private float velocidadGiro;
    private float maxSpeedChange;
    public GameObject bombPrefab;
    public float velocidadX = 7f; //valor modificable para la velocidad horizontal del jugador
    public float fuerzaSalto = 13f; //valor modificable para el salto del jugador
    public float velocidadLanzamiento = 3f;
    [SerializeField] private LayerMask jumpableGround;

    // Referencias a las bombas lanzadas
    private GameObject bombaDerecha = null;
    private GameObject bombaIzquierda = null;
    private GameObject bombaArriba = null;
    private GameObject bombaAbajo = null;


    // Variables utilizadas para el lanzamiento que manipula el jugador
    /*private bool soltado = false;
    private bool derecha = true;*/
    public GameObject hand; // Posicion en la que el jugador sostrendra la bomba
    private GameObject bombaMano = null;
    public float fuerzaDeLanzamiento = 10f;  // Fuerza con la que se lanzara la bomba
    public Camera mainCamera;  // Camara principal para obtener la posicion del cursor
    private Rigidbody2D rigidforce; // Lanzamiento

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
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

        // Manejo de la bomba hacia la derecha
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (bombaDerecha == null) // Si no hay bomba lanzada en esa dirección, lanzamos una
            {
                Vector3 posicionBomba = new Vector3(player.position.x + 1, player.position.y, 0);
                bombaDerecha = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                Vector2 direccion = new Vector2(velocidadLanzamiento + player.velocity.x, player.velocity.y);
                bombaDerecha.GetComponent<Rigidbody2D>().velocity = direccion;
            }
            else // Si ya hay bomba lanzada, detonamos
            {
                bombaDerecha.GetComponent<Bombs>().DetonarBomba();
                bombaDerecha = null; // Después de detonar, la referencia se elimina
            }
        }

        // Manejo de la bomba hacia la izquierda
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (bombaIzquierda == null)
            {
                Vector3 posicionBomba = new Vector3(player.position.x - 1, player.position.y, 0);
                bombaIzquierda = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                Vector2 direccion = new Vector2(-velocidadLanzamiento + player.velocity.x, player.velocity.y);
                bombaIzquierda.GetComponent<Rigidbody2D>().velocity = direccion;
            }
            else
            {
                bombaIzquierda.GetComponent<Bombs>().DetonarBomba();
                bombaIzquierda = null;
            }
        }

        // Manejo de la bomba hacia abajo
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (bombaAbajo == null)
            {
                Vector3 posicionBomba = new Vector3(player.position.x, player.position.y, 0);
                bombaAbajo = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                Vector2 direccion = new Vector2(player.velocity.x, -velocidadLanzamiento + player.velocity.y);
                bombaAbajo.GetComponent<Rigidbody2D>().velocity = direccion;
            }
            else
            {
                bombaAbajo.GetComponent<Bombs>().DetonarBomba();
                bombaAbajo = null;
            }
        }

        // Manejo de la bomba hacia arriba
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (bombaArriba == null)
            {
                Vector3 posicionBomba = new Vector3(player.position.x, player.position.y + 1, 0);
                bombaArriba = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                Vector2 direccion = new Vector2(player.velocity.x, +velocidadLanzamiento + player.velocity.y);
                bombaArriba.GetComponent<Rigidbody2D>().velocity = direccion;
            }
            else
            {
                bombaArriba.GetComponent<Bombs>().DetonarBomba();
                bombaArriba = null;
            }
        }
        // Manejo de la bomba con mando
        if (Input.GetButtonDown("Fire1")){
            if(bombaMano == null){
                float dirBombX = Input.GetAxisRaw("Mouse X");
                float dirBombY = Input.GetAxisRaw("Mouse Y");
                Vector3 posicionBomba;
                if(IsGrounded() && dirBombY < 0){
                    posicionBomba = new Vector3(player.position.x+dirBombX, player.position.y, 0);
                } else {
                    posicionBomba = new Vector3(player.position.x+dirBombX, player.position.y+dirBombY, 0);
                }

                bombaMano = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                Vector3 posCursor = new Vector3(dirBombX,dirBombY,0);

                rigidforce = bombaMano.GetComponent<Rigidbody2D>();
                if (rigidforce != null) {
                    rigidforce.AddForce(posCursor * fuerzaDeLanzamiento, ForceMode2D.Impulse);
                }
                } else {
                    bombaMano.GetComponent<Bombs>().DetonarBomba();
                }
        }

        /*
        // Manejo de la bomba en modo de lanzamiento
        if (Input.GetKeyDown(KeyCode.Q))
        {
            
            if(bombaMano == null)
            {
                soltado=false;
                bombaMano = Instantiate(bombPrefab, hand.transform.localPosition, Quaternion.identity);
                rigidbomb = bombaMano.GetComponent<Rigidbody2D>();
                if(rigidbomb!=null){
                    rigidbomb.gravityScale=0;
                }
                scriptBombs = bombaMano.GetComponent<Bombs>();
                if (scriptBombs != null)
                {
                    scriptBombs.enabled = false;
                }
            }
            else if(bombaMano!=null && soltado==false){
                soltado=true;
                rigidbomb.gravityScale=1;
                scriptBombs.enabled = true;
                Vector3 posCursor = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                posCursor.z = 0;
                if(derecha){
                    if(posCursor.x < hand.transform.position.x){
                        posCursor.x = hand.transform.position.x;
                    }
                }
                else{
                    if(posCursor.x > hand.transform.position.x){
                        posCursor.x = hand.transform.position.x;
                    }
                }
                Vector3 direccionLanzamiento = (posCursor - hand.transform.position).normalized;
                rigidforce = bombaMano.GetComponent<Rigidbody2D>();
                if (rigidforce != null)
                {
                    rigidforce.AddForce(direccionLanzamiento * fuerzaDeLanzamiento, ForceMode2D.Impulse);
                }

            }
            
        }

        
        // Actualizar la posicion de la bomba para que siga a Hand
        if (bombaMano!=null && soltado==false)
        {
            bombaMano.transform.SetPositionAndRotation(hand.transform.position, hand.transform.rotation);
        }

        //Explotar la bomba controlada por el jugador con E
        if (bombaMano!=null && Input.GetKeyDown(KeyCode.E) && soltado == true)
        {
            bombaMano.GetComponent<Bombs>().DetonarBomba();
            bombaMano = null;
        }

        //Si el jugador se mueve a la izquierda, la bomba se queda a la izquierda del jugador
        if(Input.GetKeyDown(KeyCode.A) && derecha == true){
            derecha = false;
            Vector3 newPos = hand.transform.localPosition;
            newPos.x = -newPos.x;
            hand.transform.localPosition = newPos;
        }

        //Si el jugador se mueve a la derecha, la bomba se queda a la derecha del jugador
        if(Input.GetKeyDown(KeyCode.D) && derecha == false){
            derecha = true;
            Vector3 newPos = hand.transform.localPosition;
            newPos.x = -newPos.x;
            hand.transform.localPosition = newPos;
        }*/


    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
