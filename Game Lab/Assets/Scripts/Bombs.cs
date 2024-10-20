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

    private float contador = 0f;

    // Start is called before the first frame update
    void Start()
    {
        /* Con este codigo, las bombas no colisionan con el jugador. Quitar este codigo si se busca otro comportamiento. */
        Collider2D colliderPlayer, colliderBomb;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
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
            DetonarBomba();
            detonado = true;
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
        CircleCollider2D colliderBomba = gameObject.AddComponent<CircleCollider2D>();
        colliderBomba.isTrigger = true;
        colliderBomba.radius = radioExplosion;

        StartCoroutine(DestruirDespuesDeExplosion());
    }

    private IEnumerator DestruirDespuesDeExplosion()
    {
        yield return new WaitForSeconds(duracionExplosion);
        Destroy(gameObject);
    }
}




