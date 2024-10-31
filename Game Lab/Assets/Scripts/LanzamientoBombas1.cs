using UnityEngine;

public class LanzamientoBombas1 : MonoBehaviour {
    private Rigidbody2D player;
    public GameObject bombPrefab;
    public float velocidadLanzamiento = 3f;

    // Referencias a las bombas lanzadas
    private GameObject bombaDerecha = null;
    private GameObject bombaIzquierda = null;
    private GameObject bombaArriba = null;
    private GameObject bombaAbajo = null;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }
    void Update() {
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
    }
}