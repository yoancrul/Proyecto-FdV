using UnityEngine;

public class ZonaBombasInfinitas : MonoBehaviour
{
    private int bombasMaximasPrevias;
    private int bombasDisponiblesPrevias;
    private bool enZonaBombasInfinitas = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerBombs = collision.GetComponent<PlayerMovement>();
            playerBombs.EnZonaBombasInfinitas();
            if (playerBombs != null)
            {
                if (!enZonaBombasInfinitas)
                {
                    // Guardar las bombas máximas y disponibles previas
                    bombasMaximasPrevias = playerBombs.bombasMaximas;
                }

                // Establecer bombas infinitas
                enZonaBombasInfinitas = true;
                playerBombs.bombasMaximas = int.MaxValue; // Representa infinito
                playerBombs.bombasDisponibles = int.MaxValue;
                playerBombs.bombasUI.text = "Bombas: ∞";

                Debug.Log("Jugador entró en la zona de bombas infinitas.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerBombs = collision.GetComponent<PlayerMovement>();
            playerBombs.EnZonaBombasInfinitas();
            if (playerBombs != null && enZonaBombasInfinitas)
            {
                // Restaurar bombas máximas y disponibles previas
                playerBombs.bombasMaximas = bombasMaximasPrevias;
                playerBombs.bombasDisponibles = bombasMaximasPrevias;
                playerBombs.bombasUI.text = $"Bombas: {bombasMaximasPrevias}";

                enZonaBombasInfinitas = false;

                Debug.Log("Jugador salió de la zona de bombas infinitas.");
            }
        }
    }
}

