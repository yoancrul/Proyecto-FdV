using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float velocidad = 1f; // Velocidad de movimiento
    public Transform limiteSuperior; // L�mite superior
    public Transform limiteInferior; // L�mite inferior

    private bool moviendoHaciaArriba = true; // Direcci�n inicial

    void Update()
    {
        // Mover el enemigo
        if (moviendoHaciaArriba)
        {
            transform.position += Vector3.up * velocidad * Time.deltaTime;

            // Si llega al l�mite superior, cambiar direcci�n
            if (transform.position.y >= limiteSuperior.position.y)
            {
                moviendoHaciaArriba = false;
            }
        }
        else
        {
            transform.position += Vector3.down * velocidad * Time.deltaTime;

            // Si llega al l�mite inferior, cambiar direcci�n
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

    // Dibujar los l�mites en la vista del editor
    void OnDrawGizmos()
    {
        if (limiteSuperior != null && limiteInferior != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(limiteSuperior.position, limiteInferior.position);
        }
    }
}
