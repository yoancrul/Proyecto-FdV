using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombasRecogibles : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = FindObjectOfType<PlayerMovement>().GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
             if (animator != null)
            {
                animator.SetBool("tieneBomba", true); 
            }
            PlayerMovement playerController = collision.GetComponent<PlayerMovement>();
            if (playerController != null)
            {
                playerController.AumentarBombasMaximas(); // Aumenta en 1 la cantidad maxima de bombas
                playerController.IgualarBombas();
            }
            Destroy(gameObject);
        }
    }
}


