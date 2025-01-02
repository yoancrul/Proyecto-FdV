using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    public GameObject canvas;

    public GameObject tutorialMando;
    public GameObject mM1;
    public GameObject mM2;
    public GameObject bM2;
    public GameObject mM3;
    public GameObject bM3;
    public GameObject mM4;
    public GameObject mM5;
    public GameObject bM5;
    public GameObject mM6;
    public GameObject bM6;

    public GameObject tutorialTeclado;
    public GameObject mT1;
    public GameObject mT2;
    public GameObject bT2;
    public GameObject mT3;
    public GameObject bT3;
    public GameObject mT4;
    public GameObject mT5;
    public GameObject bT5;
    public GameObject mT6;
    public GameObject bT6;

    public void Continue(int caso){
        GameObject siguiente = null;
        GameObject mensaje = null;
        GameObject boton = null;
        if(GameManager.controlMando){
            switch (caso){
                case 0:
                    siguiente = mM2;
                    mensaje = mM1;
                    boton = bM2;
                    break;
                case 1:
                    siguiente = mM3;
                    mensaje = mM2;
                    boton = bM3;
                    break;
                case 2:
                    siguiente = mM5;
                    mensaje = mM4;
                    boton = bM5;
                    break;
                case 3:
                    siguiente = mM6;
                    mensaje = mM5;
                    boton = bM6;
                    break;
                default:
                    Debug.LogError("No deberia llegar hasta aqui");
                    break;
            }
        } else {
            switch (caso){
                case 0:
                    siguiente = mT2;
                    mensaje = mT1;
                    boton = bT2;
                    break;
                case 1:
                    siguiente = mT3;
                    mensaje = mT2;
                    boton = bT3;
                    break;
                case 2:
                    siguiente = mT5;
                    mensaje = mT4;
                    boton = bT5;
                    break;
                case 3:
                    siguiente = mT6;
                    mensaje = mT5;
                    boton = bT6;
                    break;
                default:
                    Debug.LogError("No deberia llegar hasta aqui");
                    break;
            }
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
