using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public float velocidad = 1f;
    //public GameObject enemigo;

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

    // Update is called once per frame
    void Update()
    {
        // Mover el enemigo
        if(!siendoEmpujado && !cayendo)
            GetComponent<Rigidbody2D>().velocity = new Vector2(-velocidad + GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.y);

        // Realizar el Linecast
        RaycastHit2D hit = Physics2D.Linecast(sightStart.position, sightEnd.position);
        RaycastHit2D noGround = Physics2D.Linecast(sightGround1.position, sightGround2.position);
        RaycastHit2D caer = Physics2D.Linecast(sightGround3.position, sightGround4.position);
        if (caer.collider == null) {
            Debug.Log("Cayendo");
            if (!cayendo)
            {
                Invoke("DestruirEnemigo", 3);
            }
            cayendo = true;
            return; }
        else if (hit.collider != null && !cayendo) // Si el Linecast detecta algo
        {
            if (hit.collider.CompareTag("bomba"))
            {
                return;
            }
            if (hit.collider.CompareTag("Player")) // Verifica si lo que detecta es el jugador
            {
                playerMovement.Muere(); // Llama al método Muere() del jugador
            }

            // Cambiar de dirección si colisiona con 
            Gira();
        }   else if(noGround.collider == null && !cayendo)
        {
            Gira();
        }
        cayendo = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) { 
            playerMovement.Muere();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bomba"))
        {
            Debug.Log("El enemigo entró en contacto con la bomba");
            DetenerMovimiento();
            Invoke("RestablecerMovimiento", 1);

            // Obtén el Rigidbody del enemigo
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calcula la dirección correcta (desde la bomba hacia el enemigo)
                Vector2 direccionImpulso = transform.position - collision.transform.position;

                // Obtén la fuerza de la bomba
                float fuerzaExplosion = collision.GetComponent<Bombs>().fuerzaExplosion;

                // Aplica la fuerza al enemigo
                rb.AddForce(direccionImpulso.normalized * fuerzaExplosion, ForceMode2D.Impulse);
            }
        }
    }
    void DestruirEnemigo()
    {
        if(cayendo)
        Destroy(gameObject);
    }
    void DetenerMovimiento()
    {
        siendoEmpujado = true;

    }
    void RestablecerMovimiento()
    {
        siendoEmpujado = false;
    }
    private void Gira()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        velocidad *= -1;
        Debug.Log("Girando");
    }
    //Esto solo es para ver los raycasts
    void OnDrawGizmos()
    {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(sightStart.position, sightEnd.position);
            Gizmos.DrawLine(sightGround1.position, sightGround2.position);
            Gizmos.DrawLine(sightGround3.position, sightGround4.position);
    }
}
