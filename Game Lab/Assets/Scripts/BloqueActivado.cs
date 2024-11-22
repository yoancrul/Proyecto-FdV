using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueActivado : MonoBehaviour
{
    public Puerta puertaAsociada;
    private bool activo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("bomba"))
        {
            activo = true;
            Debug.Log("Activo");
            GetComponent<Renderer>().material.color = new Color(0,204,0);
            puertaAsociada.Abrir();
        }
    }
}
