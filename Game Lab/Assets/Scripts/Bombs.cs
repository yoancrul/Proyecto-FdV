using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bombs : MonoBehaviour
{

    public float fuerzaExplosion = 30f;
    public float radioExplosion = 3f;
    public float duracionExplosion = 0.5f;
    public LayerMask layerJugador;
    public float tiempoHastaExplosion = 3f;

    private SphereCollider colliderBomba;
    private float contador = 0f;
    private float radioInicial;

    // Start is called before the first frame update
    void Start()
    {
        colliderBomba = GetComponent<SphereCollider>();
        radioInicial = colliderBomba.radius;
    }

    // Update is called once per frame
    void Update()
    {
        contador += Time.deltaTime;

        if(contador >= tiempoHastaExplosion){
            colliderBomba.radius = radioExplosion;

            Collider[] objetosImpulsados = Physics.OverlapSphere(transform.position, radioExplosion, layerJugador);

            foreach (Collider objeto in objetosImpulsados){
                Rigidbody _rigid = objeto.GetComponent<Rigidbody>();
                if(_rigid != null){
                    Vector3 direccionImpulso = objeto.transform.position - transform.position;
                    _rigid.AddForce(direccionImpulso.normalized * fuerzaExplosion, ForceMode.Impulse);
                }
            }
        }

        if(contador >= tiempoHastaExplosion + duracionExplosion){
            colliderBomba.radius = radioInicial;
            Destroy(gameObject);
        }
        

    }

}
