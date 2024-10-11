using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bombs : MonoBehaviour
{

    public float fuerzaExplosion = 30f;
    public float radioExplosion = 3f;
    public float duracionExplosion = 0.5f;
    public GameObject player;
    private Rigidbody2D rb;
    public float tiempoHastaExplosion = 3f;

    private CircleCollider2D colliderBomba;
    private float contador = 0f;
    private float radioInicial;

    // Start is called before the first frame update
    void Start()
    {
        colliderBomba = GetComponent<CircleCollider2D>();
        radioInicial = colliderBomba.radius;
        rb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        contador += Time.deltaTime;

        if(contador >= tiempoHastaExplosion){
            colliderBomba.radius = radioExplosion;

            Vector2 direccionImpulso = player.transform.position - transform.position;
            rb.AddForce(direccionImpulso.normalized * fuerzaExplosion);
        }

        if(contador >= tiempoHastaExplosion + duracionExplosion){
            colliderBomba.radius = radioInicial;
            Destroy(gameObject);
        }
        

    }

}
