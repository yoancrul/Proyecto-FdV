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
                // Guardar el número de bombas máximo actual
                bombasMaximasPrevias = playerBombs.bombasMaximas;
                bombasDisponiblesPrevias = playerBombs.bombasDisponibles;

                // Establecer el número máximo de bombas a 0
                playerBombs.bombasMaximas = 0;
                playerBombs.bombasDisponibles = 0;

                Debug.Log("Jugador entró en la zona, bombas máximas ajustadas a 0.");
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
                // Restaurar el número de bombas máximo previo
                playerBombs.bombasMaximas = bombasMaximasPrevias;
                playerBombs.bombasDisponibles = bombasDisponiblesPrevias;

                Debug.Log("Jugador salió de la zona, bombas máximas restauradas.");
            }
        }
    }
}
