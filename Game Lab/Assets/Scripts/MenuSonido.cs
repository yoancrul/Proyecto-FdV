using UnityEngine;
using UnityEngine.UI;

public class MenuSonido : MonoBehaviour
{
    public AudioClip buttonSound; // El sonido a reproducir
    private AudioSource audioSource;

    void Start()
    {
        // Añade o encuentra el AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ajusta las propiedades del AudioSource si es necesario
        audioSource.playOnAwake = false; // No reproducir automáticamente
    }

    public void PlaySound()
    {
        if (buttonSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonSound);
        }
    }
}
