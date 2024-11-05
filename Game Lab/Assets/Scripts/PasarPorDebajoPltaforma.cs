public class PasarPorDebajoPltaforma : MonoBehaviour
{
    private Collider2D coll;
    private bool jugadorenlaPlataforma;


    private void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
       if(jugadorenlaPlataforma && Input.GetAxisRaw("Vertical") < 0)
        {
            coll.enabled = false;
            StartCoroutine(ActivarCollider());
        }
    }

    private IEnumerator ActivarCollider()
    {
        yield return new WaitForSeconds(0.5f);
        coll.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision, bool value)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            jugadorenlaPlataforma = value;
        }
    
    }

    private void OnCollisionEnter(Collider2D collision)
    {
        OnTriggerEnter2D(collision, true);
    }


    private void OnCollisionExit(Collider2D collision)
    {
        OnTriggerEnter2D(collision, false);
    }
}