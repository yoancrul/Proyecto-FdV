using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLataformaatra : MonoBehaviour
{
    // Start is called before the first frame update public Collider2D platformCollider;  // Asigna el collider de la plataforma aquí
    public float disableDuration = 0.3f; // Duración para desactivar el collider temporalmente
    private Collider2D platformCollider;

    private void Start()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Verifica si el objeto en colisión es el jugador y si se presiona la tecla para caer
        if (collision.gameObject.CompareTag("Player") && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            StartCoroutine(DisableCollider());
        }
    }

    private System.Collections.IEnumerator DisableCollider()
    {
        platformCollider.enabled = false; // Desactiva el collider temporalmente
        yield return new WaitForSeconds(disableDuration); // Espera el tiempo definido
        platformCollider.enabled = true; // Reactiva el collider
    }
}
