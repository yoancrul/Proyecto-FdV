using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float velocidad = 1f; // Velocidad de movimiento
    public Transform limiteSuperior; // L�mite superior
    public Transform limiteInferior; // L�mite inferior

    private float limiteSuperiorY; // Posici�n fija en Y del l�mite superior
    private float limiteInferiorY; // Posici�n fija en Y del l�mite inferior

    private bool moviendoHaciaArriba = true; // Direcci�n inicial

    void Start()
    {
        // Guardar las posiciones iniciales de los l�mites
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

            // Si llega al l�mite superior, cambiar direcci�n
            if (transform.position.y >= limiteSuperiorY)
            {
                moviendoHaciaArriba = false;
            }
        }
        else
        {
            transform.position += Vector3.down * velocidad * Time.deltaTime;

            // Si llega al l�mite inferior, cambiar direcci�n
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

    // Dibujar los l�mites en la vista del editor
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

