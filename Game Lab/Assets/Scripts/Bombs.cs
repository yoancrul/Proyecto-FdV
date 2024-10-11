using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bombs : MonoBehaviour
{

    public float fuerzaExplosion = 30f;
    public float radioExplosion = 3f;
    public float duracionExplosion = 0.5f;
    public float tiempoHastaExplosion = 3f;

    private CircleCollider2D colliderBomba;
    private Rigidbody2D rigidBomba;
    private float contador = 0f;
    private float radioInicial;

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

        if(contador >= tiempoHastaExplosion){
            rigidBomba.constraints = RigidbodyConstraints2D.FreezePosition;
            colliderBomba.radius = radioExplosion;
        }

        if(contador >= tiempoHastaExplosion + duracionExplosion){
            colliderBomba.radius = radioInicial;
            Destroy(gameObject);
        }
        

    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player")) {
            Vector2 direccionImpulso = collision.gameObject.transform.position - transform.position;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(direccionImpulso.normalized * fuerzaExplosion);
        }
    }

}
