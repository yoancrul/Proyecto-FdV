using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bombs : MonoBehaviour
{
    public float fuerzaExplosion = 30f;
    public float radioExplosion = 3f;
    public float duracionExplosion = 0.1f;
    public float tiempoHastaExplosion = 3f;

    private CircleCollider2D colliderBomba;
    private Rigidbody2D rigidBomba;
    private float contador = 0f;
    private float radioInicial;

    private Collider2D colliderJugador;

    // Start is called before the first frame update
    void Start()
    {
        colliderBomba = GetComponent<CircleCollider2D>();
        rigidBomba = GetComponent<Rigidbody2D>();
        radioInicial = colliderBomba.radius;
    }

    // Update is called once per frame
    void Update()
    {
        contador += Time.deltaTime;

        if (contador >= tiempoHastaExplosion)
        {
            DetonarBomba();
        }

        if (contador >= tiempoHastaExplosion + duracionExplosion)
        {
            Destroy(gameObject);
        }
    }

    // Usamos OnCollisionEnter2D en lugar de OnTriggerEnter2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        rigidBomba.constraints = RigidbodyConstraints2D.FreezePosition;
        colliderBomba.radius = radioExplosion;

        StartCoroutine(DestruirDespuesDeExplosion());
    }

    private IEnumerator DestruirDespuesDeExplosion()
    {
        yield return new WaitForSeconds(duracionExplosion);
        Destroy(gameObject);
    }
}




