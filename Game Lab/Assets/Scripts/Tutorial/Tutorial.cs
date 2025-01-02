using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject canvas;

    public GameObject tutorialMando;
    public GameObject mensajeMando;
    public GameObject botonMando;
    private GameManager gameManager;
    
    public GameObject tutorialTeclado;
    public GameObject mensajeTeclado;
    public GameObject botonTeclado;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        canvas.SetActive(false);
        tutorialMando.SetActive(false);
        tutorialTeclado.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(gameManager.TutorialIsOn()){
                GameManager.enOtroMenu = true;
                canvas.SetActive(true);
                Time.timeScale = 0;
                GameManager.pausado = true;
                if(GameManager.controlMando){
                    tutorialMando.SetActive(true);
                    mensajeMando.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(botonMando);
                } else {
                    tutorialTeclado.SetActive(true);
                    mensajeTeclado.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(botonTeclado);
                }
            }
            Destroy(gameObject);
        }
    }
}
