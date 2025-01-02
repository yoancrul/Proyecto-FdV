using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoMovimiento : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    private Vector2 offset;
    private Material material;
    private Rigidbody2D jugador;

    private void Awake(){
        material = GetComponent<MeshRenderer>().material;
        jugador = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        /*offset.x += 0.01f;
        Debug.Log("offset: "+offset);
        material.mainTextureOffset = offset;
        Debug.Log("mainTextureOffset: " + material.mainTextureOffset);*/
        //Debug.Log("Material: "+ material);
        //offset = velocidadMovimiento * Time.deltaTime;
        //material.mainTextureOffset += offset;
        //Debug.Log("Offset: " + offset);

        if(!GameManager.pausado){
            offset.x += jugador.velocity.x * 0.01f * velocidadMovimiento;
            material.mainTextureOffset = offset;
        }
    }
}
