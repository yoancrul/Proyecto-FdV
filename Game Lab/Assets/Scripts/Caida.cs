using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Caida : MonoBehaviour
{
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Buscar el GameObject con el tag "GameManager" y obtener el componente GameManager
        GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
        else
        {
            Debug.LogError("No se encontró ningún GameObject con el tag 'GameManager'.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (gameManager != null && gameManager.anticaidas) // Verificar si el GameManager y anticaidas no son nulos
            {
                playerMovement.transform.position = playerMovement.anticaidasPosition;
                Rigidbody2D player = playerMovement.gameObject.GetComponent<Rigidbody2D>();
                player.velocity = new Vector2(0, 0);
            }
            else
            {
                if (playerMovement != null)
                {
                    playerMovement.Muere(); // Llama al método Muere()
                }
            }



            
        }
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            GameObject enemigo = collision.gameObject;
            Destroy(enemigo);
        }
    }
}

