using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class ManejoBombas : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 input;

    private Rigidbody2D player;
    public GameObject bombPrefab;
    public float fuerzaDeLanzamiento = 13f;  // Fuerza con la que se lanzara la bomba
    public PlayerMovement playerMovement;   //DISCLAIMER: se debe cambiar el nombre de esto

    // Variables utilizadas para el lanzamiento que manipula el jugador
    private GameObject bomba = null;
    public Camera mainCamera;  // Camara principal para obtener la posicion del cursor
    private Rigidbody2D rigidforce; // Lanzamiento

    public float velocidadLanzamiento = 3f;

    // Variables utilizadas para generar el cursor de apuntado de las bombas de precisión
    Vector3 posCursor;
    public GameObject cursorPrefab;
    GameObject cursor;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<Rigidbody2D>();
        cursor = Instantiate(cursorPrefab, player.transform.position, Quaternion.identity);
        cursor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.pausado)
        {
            if(GameManager.controlMando){
                input = playerInput.actions["Look"].ReadValue<Vector2>();
                posCursor = new Vector3(input.x,input.y,0);
            } else {
                input = Input.mousePosition - mainCamera.WorldToScreenPoint(player.transform.position);
                posCursor = new Vector3(input.x,input.y,0);
            }
            GenerateCursor(posCursor);
        }

    }

    public void GeneratePreciseBomb(InputAction.CallbackContext callbackContext){
        if(!GameManager.pausado && callbackContext.performed){
            if (bomba == null && playerMovement.bombasDisponibles > 0)
            {
                if(GameManager.controlMando){
                    input = playerInput.actions["Look"].ReadValue<Vector2>();
                    posCursor = new Vector3(input.x,input.y,0);
                } else {
                    input = Input.mousePosition - mainCamera.WorldToScreenPoint(player.transform.position);
                    posCursor = new Vector3(input.x,input.y,0);
                }
                playerMovement.QuitarBombaDisponible();
                bomba = Instantiate(bombPrefab, player.transform.position, Quaternion.identity);

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
    }

    public void GenerateImpulseBomb(InputAction.CallbackContext callbackContext){
        if(!GameManager.pausado && callbackContext.performed){
            input = callbackContext.ReadValue<Vector2>();
            if (bomba == null && playerMovement.bombasDisponibles > 0)
            {
                    playerMovement.QuitarBombaDisponible();
                    Vector3 posicionBomba = new Vector3(player.position.x+input.x, player.position.y+input.y, 0);
                    bomba = Instantiate(bombPrefab, posicionBomba, Quaternion.identity);
                    Vector2 direccion = new Vector2(player.velocity.x + velocidadLanzamiento*input.x, player.velocity.y + velocidadLanzamiento*input.y);
                    bomba.GetComponent<Rigidbody2D>().velocity = direccion;
            }
            else
            {
                if (bomba != null)
                {
                    bomba.GetComponent<Bombs>().DetonarBomba();
                    bomba = null; // Después de detonar, la referencia se elimina
                }
            }
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
