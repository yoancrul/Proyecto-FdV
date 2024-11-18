using UnityEngine;

public class Puerta : MonoBehaviour
{
    public float velocidadApertura = 2f; // Velocidad de movimiento de la puerta
    public float alturaApertura = 5f; // Altura m�xima de apertura
    private Vector3 posicionInicial; // Posici�n original de la puerta
    private bool abriendo = false; // Indica si la puerta se est� abriendo

    void Start()
    {
        posicionInicial = transform.position; // Guarda la posici�n inicial
    }

    void Update()
    {
        // Si est� abriendo, mueve la puerta hacia arriba
        if (abriendo)
        {
            transform.position += Vector3.up * velocidadApertura * Time.deltaTime;

            // Verificar si la puerta ha alcanzado la altura m�xima
            if (transform.position.y >= posicionInicial.y + alturaApertura)
            {
                transform.position = new Vector3(transform.position.x, posicionInicial.y + alturaApertura, transform.position.z);
                abriendo = false; // Detener la apertura
            }
        }
    }

    // M�todo para iniciar la apertura de la puerta
    public void Abrir()
    {
        abriendo = true;
    }
}

