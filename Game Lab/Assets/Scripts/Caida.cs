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

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
}
