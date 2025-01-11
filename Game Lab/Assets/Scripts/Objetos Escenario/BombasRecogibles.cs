using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombasRecogibles : MonoBehaviour
{
    private Animator animator;
        public AudioClip recogidaBomba;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        animator = FindObjectOfType<PlayerMovement>().GetComponent<Animator>();
    }
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
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
            if (recogidaBomba != null)
            {
                audioSource.PlayOneShot(recogidaBomba);
            }
            //quito collaider y textura de la bomba
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            yield return new WaitForSeconds(1f);



            Destroy(gameObject);
        }
    }
}
