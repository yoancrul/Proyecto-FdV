using UnityEngine;

public class ZonaRestriccionBombas : MonoBehaviour
{
    private int bombasMaximasPrevias;
    private int bombasDisponiblesPrevias;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerBombs = collision.GetComponent<PlayerMovement>();
            if (playerBombs != null)
            {
                // Guardar las bombas máximas y disponibles previas
                bombasMaximasPrevias = playerBombs.bombasMaximas;
                bombasDisponiblesPrevias = playerBombs.bombasDisponibles;

                // Establecer bombas máximas y disponibles a 0
                playerBombs.bombasMaximas = 0;
                playerBombs.bombasDisponibles = 0;

                // Actualizar UI
                playerBombs.bombasUI.text = "Bombas: 0";

                Debug.Log("Jugador entró en la zona de restricción, bombas máximas ajustadas a 0.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerBombs = collision.GetComponent<PlayerMovement>();
            if (playerBombs != null)
            {
                // Restaurar las bombas máximas y disponibles previas
                playerBombs.bombasMaximas = bombasMaximasPrevias;
                playerBombs.bombasDisponibles = bombasDisponiblesPrevias;

                // Actualizar UI
                playerBombs.bombasUI.text = $"Bombas: {playerBombs.bombasDisponibles}";

                Debug.Log("Jugador salió de la zona de restricción, bombas máximas restauradas.");
            }
        }
    }
}
