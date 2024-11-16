using UnityEngine;

public class LanzamientoBombas1 : MonoBehaviour {
    private Rigidbody2D player;
    public GameObject bombPrefab;
    public float velocidadLanzamiento = 3f;
    public PlayerMovement playerMovement;   //DISCLAIMER: No tiene sentido q esto se llame playerMovement pero estoy muy cansao

    // Referencias a las bombas lanzadas
    private GameObject bombaDerecha = null;
    private GameObject bombaIzquierda = null;
    private GameObject bombaArriba = null;
    private GameObject bombaAbajo = null;
    //private string[] mandos;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }
    void Update() {

        /*mandos = Input.GetJoystickNames();
        if(mandos[0].Equals("")){
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
        } else {*/


        // Manejo de la bomba hacia la derecha
        if (Input.GetButtonDown("Derecha"))
            {
                if (bombaDerecha == null && playerMovement.bombasDisponibles>0) // Si no hay bomba lanzada en esa dirección, lanzamos una
                {
                    playerMovement.QuitarBombaDisponible();
                    Vector3 posicionBomba = new Vector3(player.position.x + 1, player.position.y, 0);
                    bombaDerecha = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                    Vector2 direccion = new Vector2(velocidadLanzamiento + player.velocity.x, player.velocity.y);
                    bombaDerecha.GetComponent<Rigidbody2D>().velocity = direccion;
                }
                else // Si ya hay bomba lanzada, detonamos
                {
                    if(bombaDerecha!=null){
                        bombaDerecha.GetComponent<Bombs>().DetonarBomba();
                        bombaDerecha = null; // Después de detonar, la referencia se elimina
                    }
                }
            }

            // Manejo de la bomba hacia la izquierda
            if (Input.GetButtonDown("Izquierda"))
            {
                if (bombaIzquierda == null && playerMovement.bombasDisponibles > 0)
                {
                    playerMovement.QuitarBombaDisponible();
                    Vector3 posicionBomba = new Vector3(player.position.x - 1, player.position.y, 0);
                    bombaIzquierda = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                    Vector2 direccion = new Vector2(-velocidadLanzamiento + player.velocity.x, player.velocity.y);
                    bombaIzquierda.GetComponent<Rigidbody2D>().velocity = direccion;
                }
                else
                {
                    if(bombaIzquierda!=null){
                        bombaIzquierda.GetComponent<Bombs>().DetonarBomba();
                        bombaIzquierda = null;
                    }
                }
            }

            // Manejo de la bomba hacia abajo
            if (Input.GetButtonDown("Abajo"))
            {
                if (bombaAbajo == null && playerMovement.bombasDisponibles > 0)
                {
                    playerMovement.QuitarBombaDisponible();
                    Vector3 posicionBomba = new Vector3(player.position.x, player.position.y, 0);
                    bombaAbajo = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                    Vector2 direccion = new Vector2(player.velocity.x, -velocidadLanzamiento + player.velocity.y);
                    bombaAbajo.GetComponent<Rigidbody2D>().velocity = direccion;
                }
                else
                {
                    if(bombaAbajo!=null){
                        bombaAbajo.GetComponent<Bombs>().DetonarBomba();
                        bombaAbajo = null;
                    }
                }
            }

            // Manejo de la bomba hacia arriba
            if (Input.GetButtonDown("Arriba"))
            {
                if (bombaArriba == null && playerMovement.bombasDisponibles > 0)
                {
                playerMovement.QuitarBombaDisponible();
                    Vector3 posicionBomba = new Vector3(player.position.x, player.position.y + 1, 0);
                    bombaArriba = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                    Vector2 direccion = new Vector2(player.velocity.x, +velocidadLanzamiento + player.velocity.y);
                    bombaArriba.GetComponent<Rigidbody2D>().velocity = direccion;
                }
                else
                {
                    if(bombaArriba!=null){
                        bombaArriba.GetComponent<Bombs>().DetonarBomba();
                        bombaArriba = null;
                    }
                }
            }
        //}

        

        
    }
}