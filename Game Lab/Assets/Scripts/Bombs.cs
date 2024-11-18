using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;

public class Bombs : MonoBehaviour
{
    public float fuerzaExplosion = 5f;
    public float radioExplosion = 3f;
    public float duracionExplosion = 0.1f;
    public float tiempoHastaExplosion = 3f;
    public bool detonado = false;
    private PlayerMovement playerMovement; //usado para encontrar bombasDisponibles en el script del jugador

    private float contador = 0f;
    // Start is called before the first frame update
    void Start()
    {
        /* Con este codigo, las bombas no colisionan con el jugador. Quitar este codigo si se busca otro comportamiento. */
        Collider2D colliderPlayer, colliderBomb;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        colliderPlayer = player.GetComponent<Collider2D>();
        colliderBomb = gameObject.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(colliderPlayer, colliderBomb);
    }

    // Update is called once per frame
    void Update()
    {
        contador += Time.deltaTime;

        if (contador >= tiempoHastaExplosion && !detonado)
        {
            //detonado = true; /*** * no permite que la bomba explote con tiempo */
            DetonarBomba();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Aplicamos la fuerza de explosión al jugador
            Vector2 direccionImpulso = collision.gameObject.transform.position - transform.position;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(direccionImpulso.normalized * fuerzaExplosion, ForceMode2D.Impulse);
        }
    }

    public void DetonarBomba()
    {
        if (detonado) return; // Evita que se detone dos veces
        detonado = true;
        CircleCollider2D colliderBomba = gameObject.AddComponent<CircleCollider2D>();
        colliderBomba.isTrigger = true;
        colliderBomba.radius = radioExplosion;

        DestruirMuros();

        StartCoroutine(DestruirDespuesDeExplosion());
    }

    private void DestruirMuros()
    {
        // Obtener todas las celdas afectadas por la explosión
        Collider2D[] objetosAfectados = Physics2D.OverlapCircleAll(transform.position, radioExplosion);

        foreach (var col in objetosAfectados)
        {
            // Verificar si la colisión es con un tile del Tilemap
            if (col.CompareTag("muro"))
            {
                Tilemap tilemap = col.GetComponent<Tilemap>();
                if (tilemap == null)
                {
                    Debug.LogWarning("No se encontró un Tilemap en el colisionador del muro.");
                    continue;
                }

                // Obtener todas las posiciones de tiles dentro del radio de la explosión
                Vector3 explosionCenter = transform.position;
                BoundsInt bounds = tilemap.cellBounds;

                // Iterar sobre cada celda dentro de los límites del Tilemap
                foreach (var pos in bounds.allPositionsWithin)
                {
                    // Convertir la posición del tile al mundo para calcular la distancia
                    Vector3 worldPosition = tilemap.CellToWorld(pos) + tilemap.cellSize / 2; // Centro del tile
                    float distancia = Vector3.Distance(explosionCenter, worldPosition);

                    // Si está dentro del radio de la explosión y hay un tile en esa posición, destrúyelo
                    if (distancia <= radioExplosion && tilemap.HasTile(pos))
                    {
                        Debug.Log($"Tile destruido en posición: {pos}");
                        QuitarMuro(tilemap, pos);
                    }
                }
            }
        }
    }

    private void QuitarMuro(Tilemap tilemap, Vector3Int posicionInicial)
    {
        // Usar un conjunto de tiles ya destruidos para evitar repetir la destrucción de celdas
        HashSet<Vector3Int> tilesDestruidos = new HashSet<Vector3Int>();
        Queue<Vector3Int> tilesPorRevisar = new Queue<Vector3Int>();
        tilesPorRevisar.Enqueue(posicionInicial);

        while (tilesPorRevisar.Count > 0)
        {
            Vector3Int tileActual = tilesPorRevisar.Dequeue();

            // Si ya hemos destruido este tile, lo ignoramos
            if (tilesDestruidos.Contains(tileActual))
                continue;

            // Si no hay un tile en esta posición, continuamos
            if (!tilemap.HasTile(tileActual))
                continue;

            // Eliminar el tile actual
            tilemap.SetTile(tileActual, null);
            Debug.Log($"Tile eliminado en posición: {tileActual}");
            tilesDestruidos.Add(tileActual);  // Marcar el tile como destruido

            // Agregar las posiciones adyacentes (arriba, abajo, izquierda, derecha) para revisar
            Vector3Int[] direcciones = new Vector3Int[]
            {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
            };

            foreach (Vector3Int direccion in direcciones)
            {
                Vector3Int adyacente = tileActual + direccion;
                if (!tilesDestruidos.Contains(adyacente) && tilemap.HasTile(adyacente))
                {
                    tilesPorRevisar.Enqueue(adyacente);
                }
            }
        }
    }




    private IEnumerator DestruirDespuesDeExplosion()
    {
        yield return new WaitForSeconds(duracionExplosion);
        Destroy(gameObject);
    }
}




