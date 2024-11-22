using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float velocidad = 1f; // Velocidad de movimiento
    public Transform limiteSuperior; // Límite superior
    public Transform limiteInferior; // Límite inferior

    private float limiteSuperiorY; // Posición fija en Y del límite superior
    private float limiteInferiorY; // Posición fija en Y del límite inferior

    private bool moviendoHaciaArriba = true; // Dirección inicial

    void Start()
    {
        // Guardar las posiciones iniciales de los límites
        if (limiteSuperior != null)
            limiteSuperiorY = limiteSuperior.position.y;
        if (limiteInferior != null)
            limiteInferiorY = limiteInferior.position.y;
    }

    void Update()
    {
        // Mover el enemigo
        if (moviendoHaciaArriba)
        {
            transform.position += Vector3.up * velocidad * Time.deltaTime;

            // Si llega al límite superior, cambiar dirección
            if (transform.position.y >= limiteSuperiorY)
            {
                moviendoHaciaArriba = false;
            }
        }
        else
        {
            transform.position += Vector3.down * velocidad * Time.deltaTime;

            // Si llega al límite inferior, cambiar dirección
            if (transform.position.y <= limiteInferiorY)
            {
                moviendoHaciaArriba = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Muere();
            }
        }
    }

    // Dibujar los límites en la vista del editor
    void OnDrawGizmos()
    {
        if (limiteSuperior != null && limiteInferior != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(limiteSuperior.position, limiteInferior.position);
            Gizmos.DrawSphere(limiteSuperior.position, 0.1f);
            Gizmos.DrawSphere(limiteInferior.position, 0.1f);
        }
    }
}

