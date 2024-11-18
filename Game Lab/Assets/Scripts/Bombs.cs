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
                Vector3 hitPosition = col.transform.position;
                Vector3Int tilePos = tilemap.WorldToCell(hitPosition);

                // Verificar si hay un tile en esa posición y eliminar el muro
                if (tilemap.HasTile(tilePos))
                {
                    QuitarMuro(tilemap, tilePos);
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
            tilesDestruidos.Add(tileActual);  // Marcar el tile como destruido

            // Agregar las posiciones adyacentes (arriba, abajo, izquierda, derecha) para revisar
            tilesPorRevisar.Enqueue(tileActual + Vector3Int.up);
            tilesPorRevisar.Enqueue(tileActual + Vector3Int.down);
            tilesPorRevisar.Enqueue(tileActual + Vector3Int.left);
            tilesPorRevisar.Enqueue(tileActual + Vector3Int.right);
        }
    }

    private IEnumerator DestruirDespuesDeExplosion()
    {
        yield return new WaitForSeconds(duracionExplosion);
        Destroy(gameObject);
    }
}




