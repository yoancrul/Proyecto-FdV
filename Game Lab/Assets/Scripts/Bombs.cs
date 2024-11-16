using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bombs : MonoBehaviour
{
    public float fuerzaExplosion = 5f;
    public float radioExplosion = 3f;
    public float duracionExplosion = 0.1f;
    public float tiempoHastaExplosion = 3f;
    public bool detonado = false;
    private PlayerMovement playerMovement; //usado para encontrar bombasDisponibles en el script del jugador

    private float contador = 0f;

    // Start is called before the first frame update
    void Start()
    {
        /* Con este codigo, las bombas no colisionan con el jugador. Quitar este codigo si se busca otro comportamiento. */
        Collider2D colliderPlayer, colliderBomb;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        colliderPlayer = player.GetComponent<Collider2D>();
        colliderBomb = gameObject.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(colliderPlayer, colliderBomb);
    }

    // Update is called once per frame
    void Update()
    {
        contador += Time.deltaTime;

        if (contador >= tiempoHastaExplosion && !detonado)
        {
            //detonado = true; /*** * no permite que la bomba explote con tiempo */
            DetonarBomba();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Aplicamos la fuerza de explosión al jugador
            Vector2 direccionImpulso = collision.gameObject.transform.position - transform.position;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(direccionImpulso.normalized * fuerzaExplosion, ForceMode2D.Impulse);
        }
    }

    public void DetonarBomba()
    {
        if (detonado) return; // Evita que se detone dos veces
        detonado = true;
        CircleCollider2D colliderBomba = gameObject.AddComponent<CircleCollider2D>();
        colliderBomba.isTrigger = true;
        colliderBomba.radius = radioExplosion;

        DestruirMuros();

        StartCoroutine(DestruirDespuesDeExplosion());
    }

    private void DestruirMuros()
{
    Collider2D[] objetosAfectados = Physics2D.OverlapCircleAll(transform.position, radioExplosion);
    for (int i = 0; i < objetosAfectados.Length; i++){
        if (objetosAfectados[i].CompareTag("muro")) {
            Destroy(objetosAfectados[i].gameObject);
        }
    }
}
    private IEnumerator DestruirDespuesDeExplosion()
    {
        yield return new WaitForSeconds(duracionExplosion);
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        
    }
}




