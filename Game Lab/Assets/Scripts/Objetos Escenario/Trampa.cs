using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trampa : MonoBehaviour
{
    PlayerMovement playerMovement;
    public AudioClip enemyDeath;
    private AudioSource audioSource;
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerMovement.Muere();
        }
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            if (enemyDeath != null)
            {
                audioSource.PlayOneShot(enemyDeath);
            }

            GameObject enemigo = collision.gameObject;
            Destroy(enemigo);
        }
    }
}
