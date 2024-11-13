using UnityEngine;

public class LanzamientoBombas2 : MonoBehaviour {
    private Rigidbody2D player;
    public GameObject bombPrefab;
    public float fuerzaDeLanzamiento = 10f;  // Fuerza con la que se lanzara la bomba

    // Variables utilizadas para el lanzamiento que manipula el jugador
    public GameObject hand; // Posicion en la que el jugador sostrendra la bomba
    private GameObject bombaMano = null;
    public Camera mainCamera;  // Camara principal para obtener la posicion del cursor
    private Rigidbody2D rigidforce; // Lanzamiento

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }
    void Update() {

        // Manejo de la bomba con mando
        if (Input.GetButtonDown("Fire1")){
            if(bombaMano == null){
                float dirBombX = Input.GetAxisRaw("Mouse X");
                float dirBombY = Input.GetAxisRaw("Mouse Y");
                /*Vector3 posicionBomba;
                if(IsGrounded() && dirBombY < 0){
                    posicionBomba = new Vector3(player.position.x+dirBombX, player.position.y, 0);
                } else {
                    posicionBomba = new Vector3(player.position.x+dirBombX, player.position.y+dirBombY, 0);
                }*/

                bombaMano = Instantiate(bombPrefab, hand.transform.position, Quaternion.identity);
                Vector3 posCursor = new Vector3(dirBombX,dirBombY,0);

                rigidforce = bombaMano.GetComponent<Rigidbody2D>();
                if (rigidforce != null) {
                    rigidforce.AddForce(posCursor * fuerzaDeLanzamiento, ForceMode2D.Impulse);
                }
            } else {
                bombaMano.GetComponent<Bombs>().DetonarBomba();
            }
        }

        // Manejo de la bomba en modo de lanzamiento
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (bombaMano == null)
            {
                bombaMano = Instantiate(bombPrefab, hand.transform.position, Quaternion.identity);
                Vector3 posCursor = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                posCursor.z = 0;
                Vector2 direccionLanzamiento = (posCursor - hand.transform.position).normalized;
                Vector2 lanzamiento = direccionLanzamiento * fuerzaDeLanzamiento + player.velocity;
                rigidforce = bombaMano.GetComponent<Rigidbody2D>();
                if (rigidforce != null)
                {
                    rigidforce.AddForce(lanzamiento, ForceMode2D.Impulse);
                }
            }
            else
            {
                bombaMano.GetComponent<Bombs>().DetonarBomba();
            }
        }
    }
}