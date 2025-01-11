using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaNivel : MonoBehaviour
{
    private Rigidbody2D rb;
    public int nivelCargado;
    private GameManager gameManager;
    private Timer timer; // Referencia al cronómetro
    public AudioClip victoria;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        timer = FindObjectOfType<Timer>(); // Encuentra el cronómetro en la escena
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (gameManager == null)
        {
            Debug.LogError("GameManager no encontrado en la escena.");
        }
        if (timer == null)
        {
            Debug.LogError("Timer no encontrado en la escena.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (timer != null && gameManager != null)
            {
                if (victoria != null)
        {
            audioSource.PlayOneShot(victoria);
        }
                float tiempoFinal = timer.GetTime(); // Obtén el tiempo actual del cronómetro
                gameManager.FinalNivel(tiempoFinal); // Llama al método FinalNivel con el tiempo
            }
        }
    }
}

