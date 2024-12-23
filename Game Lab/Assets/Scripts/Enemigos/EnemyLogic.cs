using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public float velocidad = 1f;

    public Transform sightStart;
    public Transform sightEnd;

    public Transform sightGround1;
    public Transform sightGround2;
    public Transform sightGround3;
    public Transform sightGround4;

    private PlayerMovement playerMovement;

    private bool siendoEmpujado = false;
    private bool cayendo = false;

    public bool colliding;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Capas que vamos a ignorar en el raycast
        int layer1 = LayerMask.NameToLayer("Ignore Raycast");
        int layer2 = LayerMask.NameToLayer("CameraBounds");

        int layersToIgnore = (1 << layer1) | (1 << layer2); // Combinamos las capas con or
        int layerMask = ~layersToIgnore; // Invertimos para incluir el resto de capas

        // Verificar si el enemigo está en el suelo
        bool estaEnElSuelo = Physics2D.Linecast(sightGround1.position, sightGround2.position).collider != null;

        // Mover el enemigo solo si no está siendo empujado y no está cayendo
        if (!siendoEmpujado && !cayendo && estaEnElSuelo)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-velocidad, GetComponent<Rigidbody2D>().velocity.y);
        }

        // Realizar los Linecast para colisiones
        RaycastHit2D hit = Physics2D.Linecast(sightStart.position, sightEnd.position, layerMask);
        RaycastHit2D noGround = Physics2D.Linecast(sightGround1.position, sightGround2.position, layerMask);
        RaycastHit2D caer = Physics2D.Linecast(sightGround3.position, sightGround4.position, layerMask);

        if (caer.collider == null)
        {
            if (!cayendo)
            {
                //Invoke("DestruirEnemigo", 3);
                cayendo = true;
            }
            return;
        }
        else if (hit.collider != null && !cayendo)
        {
            if (hit.collider.CompareTag("bomba"))
            {
                return;
            }
            if (hit.collider.CompareTag("Player"))
            {
                playerMovement.Muere();
            }

            Gira();
        }
        else if (noGround.collider == null && !cayendo)
        {
            Gira();
        }

        cayendo = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerMovement.Muere();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bomba"))
        {
            DetenerMovimiento();
            Invoke("RestablecerMovimiento", 1);

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direccionImpulso = transform.position - collision.transform.position;
                float fuerzaExplosion = collision.GetComponent<Bombs>().fuerzaExplosion;
                rb.AddForce(direccionImpulso.normalized * fuerzaExplosion, ForceMode2D.Impulse);
            }
        }
    }

    void DestruirEnemigo()
    {
        if (cayendo)
            Destroy(gameObject);
    }

    void DetenerMovimiento()
    {
        siendoEmpujado = true;
    }

    void RestablecerMovimiento()
    {
        siendoEmpujado = false;

        // Si no está en el suelo, no retomar movimiento horizontal
        bool estaEnElSuelo = Physics2D.Linecast(sightGround1.position, sightGround2.position).collider != null;
        if (!estaEnElSuelo)
        {
            cayendo = true;
        }
    }

    private void Gira()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        velocidad *= -1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(sightStart.position, sightEnd.position);
        Gizmos.DrawLine(sightGround1.position, sightGround2.position);
        Gizmos.DrawLine(sightGround3.position, sightGround4.position);
    }
}
