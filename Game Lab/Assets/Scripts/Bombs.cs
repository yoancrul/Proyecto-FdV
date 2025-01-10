using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bombs : MonoBehaviour
{
    public AudioClip destructionSound;
    public AudioClip explosion; // Sonido para la destrucción del muro
    private AudioSource audioSource;
    public float fuerzaExplosion = 5f;
    public float radioExplosion = 3f;
    public float duracionExplosion = 0.1f;
    public float tiempoHastaExplosion = 3f;
    public bool detonado = false;

    public GameObject explosionIndicatorPrefab; // Prefab para mostrar el radio de explosión

    private GameObject explosionIndicator; // Instancia del indicador visual
    private PlayerMovement playerMovement; // Usado para encontrar bombasDisponibles en el script del jugador

    private float contador = 0f;

    void Start()
    {
        // Añadir o buscar el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Ignorar colisiones con el jugador
        Collider2D colliderPlayer, colliderBomb;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        colliderPlayer = player.GetComponent<Collider2D>();
        colliderBomb = gameObject.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(colliderPlayer, colliderBomb);
    }

    void Update()
    {
        contador += Time.deltaTime;

        if (contador >= tiempoHastaExplosion && !detonado)
        {
            DetonarBomba();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direccionImpulso = collision.gameObject.transform.position - transform.position;
            if (direccionImpulso.y > 0)
            {
                playerMovement.ResetIfFalling();
            }
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(direccionImpulso.normalized * fuerzaExplosion, ForceMode2D.Impulse);
        }
    }

    public void DetonarBomba()
    {
        if (detonado) return; // Evita que se detone dos veces
        detonado = true;

        // Mostrar el radio de explosión en el lugar de la bomba
        MostrarRadioDeExplosion();

        // Añadir el collider de la explosión
        CircleCollider2D colliderBomba = gameObject.AddComponent<CircleCollider2D>();
        colliderBomba.isTrigger = true;
        colliderBomba.radius = radioExplosion;
        

        

        DestruirMuros();

        StartCoroutine(DestruirDespuesDeExplosion());
    }

    private void MostrarRadioDeExplosion()
    {
        if (explosionIndicatorPrefab != null)
        {
            // Crear el indicador en la posición de la bomba
            explosionIndicator = Instantiate(explosionIndicatorPrefab, transform.position, Quaternion.identity);

            // Escalar el indicador para que coincida con el radio de explosión
            explosionIndicator.transform.localScale = new Vector3(radioExplosion * 2, radioExplosion * 2, 1);
        }
    }

    private void DestruirMuros()
    {
        Collider2D[] objetosAfectados = Physics2D.OverlapCircleAll(transform.position, radioExplosion);

        foreach (var col in objetosAfectados)
        {
            if (col.CompareTag("muro"))
            {
                Tilemap tilemap = col.GetComponent<Tilemap>();
                if (tilemap != null)
                {
                    Vector3 explosionCenter = transform.position;
                    BoundsInt bounds = tilemap.cellBounds;

                    foreach (var pos in bounds.allPositionsWithin)
                    {
                        Vector3 worldPosition = tilemap.CellToWorld(pos) + tilemap.cellSize / 2;
                        float distancia = Vector3.Distance(explosionCenter, worldPosition);

                        if (distancia <= radioExplosion && tilemap.HasTile(pos))
                        {
                            if (destructionSound != null){
                                audioSource.PlayOneShot(destructionSound);
                            }
                            QuitarMuro(tilemap, pos);
                        }
                    }
                }
            }
        }
    }

    private void QuitarMuro(Tilemap tilemap, Vector3Int posicionInicial)
    {
        HashSet<Vector3Int> tilesDestruidos = new HashSet<Vector3Int>();
        Queue<Vector3Int> tilesPorRevisar = new Queue<Vector3Int>();
        tilesPorRevisar.Enqueue(posicionInicial);

        while (tilesPorRevisar.Count > 0)
        {
            Vector3Int tileActual = tilesPorRevisar.Dequeue();
            if (!tilemap.HasTile(tileActual)) continue;
            tilemap.SetTile(tileActual, null);
            tilesDestruidos.Add(tileActual);

            Vector3Int[] direcciones = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

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
        //quito collider de la explosion y bomba
        Destroy(GetComponent<CircleCollider2D>());
        
        if (explosion != null){
            audioSource.PlayOneShot(explosion);
        }
        //quito la textura de la bomba
        if (explosionIndicator != null)
        {
            Destroy(explosionIndicator);
        }
        GetComponent<SpriteRenderer>().sprite = null;
         yield return new WaitForSeconds(2f);


        // Destruir el indicador visual
        

        Destroy(gameObject);
    }
}
