using UnityEngine;

public class Puerta : MonoBehaviour
{
    public float velocidadApertura = 2f; // Velocidad de movimiento de la puerta
    public float distanciaApertura = 5f; // Distancia máxima de apertura

    public enum DireccionApertura
    {
        Arriba,
        Abajo,
        Izquierda,
        Derecha
    }

    public DireccionApertura direccionApertura = DireccionApertura.Arriba; // Dirección de apertura seleccionable desde el Inspector

    private Vector3 posicionInicial; // Posición original de la puerta
    private Vector3 direccionMovimiento; // Vector de movimiento basado en la dirección seleccionada
    private bool abriendo = false; // Indica si la puerta se está abriendo

    void Start()
    {
        posicionInicial = transform.position; // Guarda la posición inicial
        direccionMovimiento = ObtenerVectorDireccion(direccionApertura); // Calcula el vector de dirección al iniciar
    }

    void Update()
    {
        // Si está abriendo, mueve la puerta en la dirección especificada
        if (abriendo)
        {
            transform.position += direccionMovimiento * velocidadApertura * Time.deltaTime;

            // Verificar si la puerta ha alcanzado la distancia máxima
            if (Vector3.Distance(posicionInicial, transform.position) >= distanciaApertura)
            {
                transform.position = posicionInicial + direccionMovimiento * distanciaApertura;
                abriendo = false; // Detener la apertura
            }
        }
    }

    // Método para iniciar la apertura de la puerta
    public void Abrir()
    {
        abriendo = true;
    }

    // Convierte la dirección seleccionada en el Inspector a un vector
    private Vector3 ObtenerVectorDireccion(DireccionApertura direccion)
    {
        switch (direccion)
        {
            case DireccionApertura.Arriba:
                return Vector3.up;
            case DireccionApertura.Abajo:
                return Vector3.down;
            case DireccionApertura.Izquierda:
                return Vector3.left;
            case DireccionApertura.Derecha:
                return Vector3.right;
            default:
                return Vector3.up;
        }
    }
}

