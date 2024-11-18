using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float velocidad = 1f; // Velocidad de movimiento
    public Transform limiteSuperior; // Límite superior
    public Transform limiteInferior; // Límite inferior

    private bool moviendoHaciaArriba = true; // Dirección inicial

    void Update()
    {
        // Mover el enemigo
        if (moviendoHaciaArriba)
        {
            transform.position += Vector3.up * velocidad * Time.deltaTime;

            // Si llega al límite superior, cambiar dirección
            if (transform.position.y >= limiteSuperior.position.y)
            {
                moviendoHaciaArriba = false;
            }
        }
        else
        {
            transform.position += Vector3.down * velocidad * Time.deltaTime;

            // Si llega al límite inferior, cambiar dirección
            if (transform.position.y <= limiteInferior.position.y)
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
        }
    }
}
