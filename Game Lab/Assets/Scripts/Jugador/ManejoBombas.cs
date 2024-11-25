using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UIElements;

public class ManejoBombas : MonoBehaviour
{

    private Rigidbody2D player;
    public GameObject bombPrefab;
    public float fuerzaDeLanzamiento = 13f;  // Fuerza con la que se lanzara la bomba
    public PlayerMovement playerMovement;   //DISCLAIMER: se debe cambiar el nombre de esto

    // Variables utilizadas para el lanzamiento que manipula el jugador
    private GameObject bomba = null;
    public Camera mainCamera;  // Camara principal para obtener la posicion del cursor
    private Rigidbody2D rigidforce; // Lanzamiento

    public float velocidadLanzamiento = 3f;

    private string[] mandos;
    Vector3 posCursor;
    public GameObject cursorPrefab;
    GameObject cursor;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        player = GetComponent<Rigidbody2D>();
        cursor = Instantiate(cursorPrefab, player.transform.position, Quaternion.identity);
        cursor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.pausado)
        {
            /*mandos = Input.GetJoystickNames();
            if(mandos[0].Equals("")){*/

            // Para Teclado y Ratón
                //posCursor = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 posCursor = Input.mousePosition - mainCamera.WorldToScreenPoint(player.transform.position);
                posCursor.z = 0;
            /*} else {

            // Para Mando
                float dirBombX = Input.GetAxisRaw("Mouse X");
                float dirBombY = Input.GetAxisRaw("Mouse Y");
                posCursor = new Vector3(dirBombX, dirBombY, 0);
            }*/

            GenerateCursor(posCursor);

            /*mandos = Input.GetJoystickNames();
            if(mandos[0].Equals("")){
                Manejo de la bomba en modo de lanzamiento*/
            //if (Input.GetButtonDown("Fire1"))
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (bomba == null && playerMovement.bombasDisponibles > 0)
                {
                    playerMovement.QuitarBombaDisponible();
                    bomba = Instantiate(bombPrefab, player.transform.position, Quaternion.identity);
                    /*Vector3 posCursor = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    posCursor.z = 0;*/
                    Vector2 direccionLanzamiento = (posCursor - player.transform.position).normalized;
                    Vector2 lanzamiento = direccionLanzamiento * fuerzaDeLanzamiento + player.velocity;
                    rigidforce = bomba.GetComponent<Rigidbody2D>();
                    if (rigidforce != null)
                    {
                        rigidforce.AddForce(lanzamiento, ForceMode2D.Impulse);
                    }
                }
                else
                {
                    if (bomba != null)
                        bomba.GetComponent<Bombs>().DetonarBomba();
                        bomba = null; // Después de detonar, la referencia se elimina
                }
            }
            //} else {
            // Manejo de la bomba con mando
            if (Input.GetButtonDown("Fire1"))
            {
                if (bomba == null && playerMovement.bombasDisponibles > 0)
                {
                    playerMovement.QuitarBombaDisponible();
                    /*float dirBombX = Input.GetAxisRaw("Mouse X");
                    float dirBombY = Input.GetAxisRaw("Mouse Y");*/
                    bomba = Instantiate(bombPrefab, player.transform.position, Quaternion.identity);
                    //Vector3 posCursor = new Vector3(dirBombX, dirBombY, 0);

                    rigidforce = bomba.GetComponent<Rigidbody2D>();
                    if (rigidforce != null)
                    {
                        rigidforce.AddForce(posCursor.normalized * fuerzaDeLanzamiento, ForceMode2D.Impulse);
                    }
                }
                else
                {
                    if (bomba != null)
                        bomba.GetComponent<Bombs>().DetonarBomba();
                        bomba = null; // Después de detonar, la referencia se elimina
                }
            }
            //}

            // Manejo de la bomba hacia la derecha
            if (Input.GetButtonDown("Derecha"))
            {
                if (bomba == null && playerMovement.bombasDisponibles > 0) // Si no hay bomba lanzada en esa dirección, lanzamos una
                {
                        playerMovement.QuitarBombaDisponible();
                        Vector3 posicionBomba = new Vector3(player.position.x + 1, player.position.y, 0);
                        bomba = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                        Vector2 direccion = new Vector2(velocidadLanzamiento + player.velocity.x, player.velocity.y);
                        bomba.GetComponent<Rigidbody2D>().velocity = direccion;
                }
                else // Si ya hay bomba lanzada, detonamos
                {
                    if (bomba != null)
                    {
                        bomba.GetComponent<Bombs>().DetonarBomba();
                        bomba = null; // Después de detonar, la referencia se elimina
                    }
                }
            }

            // Manejo de la bomba hacia la izquierda
            if (Input.GetButtonDown("Izquierda"))
            {
                if (bomba == null && playerMovement.bombasDisponibles > 0)
                {
                        playerMovement.QuitarBombaDisponible();
                        Vector3 posicionBomba = new Vector3(player.position.x - 1, player.position.y, 0);
                        bomba = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                        Vector2 direccion = new Vector2(-velocidadLanzamiento + player.velocity.x, player.velocity.y);
                        bomba.GetComponent<Rigidbody2D>().velocity = direccion;
                }
                else
                {
                    if (bomba != null)
                    {
                        bomba.GetComponent<Bombs>().DetonarBomba();
                        bomba = null;
                    }
                }
            }

            // Manejo de la bomba hacia abajo
            if (Input.GetButtonDown("Abajo"))
            {
                if (bomba == null && playerMovement.bombasDisponibles > 0)
                {
                        playerMovement.QuitarBombaDisponible();
                        Vector3 posicionBomba = new Vector3(player.position.x, player.position.y, 0);
                        bomba = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                        Vector2 direccion = new Vector2(player.velocity.x, -velocidadLanzamiento + player.velocity.y);
                        bomba.GetComponent<Rigidbody2D>().velocity = direccion;
                }
                else
                {
                    if (bomba != null)
                    {
                        bomba.GetComponent<Bombs>().DetonarBomba();
                        bomba = null;
                    }
                }
            }

            // Manejo de la bomba hacia arriba
            if (Input.GetButtonDown("Arriba"))
            {
                if (bomba == null && playerMovement.bombasDisponibles > 0)
                {
                        playerMovement.QuitarBombaDisponible();
                        Vector3 posicionBomba = new Vector3(player.position.x, player.position.y + 1, 0);
                        bomba = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                        Vector2 direccion = new Vector2(player.velocity.x, +velocidadLanzamiento + player.velocity.y);
                        bomba.GetComponent<Rigidbody2D>().velocity = direccion;
                }
                else
                {
                    if (bomba != null)
                    {
                        bomba.GetComponent<Bombs>().DetonarBomba();
                        bomba = null;
                    }
                }
            }
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerController = collision.GetComponent<PlayerMovement>();
            if (playerController != null)
            {
                playerController.AumentarBombasMaximas(); // Aumenta en 1 la cantidad maxima de bombas
                playerController.IgualarBombas();
            }
            Destroy(gameObject);
        }
    }

    private void GenerateCursor(Vector3 posCursor){
        if(posCursor.magnitude <= 0.1f){
            cursor.SetActive(false);
        } else {
            cursor.SetActive(true);
            float angulo = Vector2.SignedAngle(Vector2.up, posCursor);
            cursor.transform.SetPositionAndRotation(player.transform.position + posCursor.normalized * 2f, Quaternion.AngleAxis(angulo,Vector3.forward));
        }
    }
}
