using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialNv1 : MonoBehaviour
{
    public GameObject canvas;

    public GameObject tutorialMando;
    public GameObject mensajeMando1;
    public GameObject mensajeMando2;
    public GameObject botonMando;


    public GameObject tutorialTeclado;
    public GameObject mensajeTeclado1;
    public GameObject mensajeTeclado2;
    public GameObject botonTeclado;


    public void Continue(){
        GameObject siguiente;
        GameObject boton;
        GameObject mensaje;
        if (GameManager.controlMando)
        {
            siguiente = mensajeMando2;
            mensaje = mensajeMando1;
            boton = botonMando;
        }
        else
        {
            siguiente = mensajeTeclado2;
            mensaje = mensajeTeclado1;
            boton = botonTeclado;
        }
        EventSystem.current.SetSelectedGameObject(boton);
        siguiente.SetActive(true);
        mensaje.SetActive(false);
    }

    public void Fin(GameObject mensaje){
        Time.timeScale = 1;
        GameManager.pausado = false;
        GameManager.enOtroMenu = false;
        mensaje.SetActive(false);
        if(GameManager.controlMando){
            tutorialMando.SetActive(false);
        } else {
            tutorialTeclado.SetActive(false);
        }
        canvas.SetActive(false);
    }
}
