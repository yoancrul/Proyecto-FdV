using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Caida : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   void OnCollisionEnter2D(Collision2D collision){
    if (collision.gameObject.CompareTag("Player")){
            Debug.Log("Colisión con suelo detectada");
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.Muere(); // Llama al método Muere()
            }
        }
    if (collision.gameObject.CompareTag("Enemigo"))
        {
            GameObject enemigo = collision.gameObject;
            Destroy(enemigo);
        }
}
}
