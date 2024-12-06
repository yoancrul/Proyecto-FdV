using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject levelSelect;
    public GameObject controlMenu;
    public GameObject controlesTeclado;
    public GameObject controlesMando;
    public GameObject botonInicio;
    public GameObject botonNiveles;
    public GameObject botonControl;
    public PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        levelSelect.SetActive(false);
        controlMenu.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Nivel 0");
        GameManager.controlMando = playerInput.currentControlScheme.Equals("Gamepad");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LevelSelect()
    {
        EventSystem.current.SetSelectedGameObject(botonNiveles);
        menuPrincipal.SetActive(false);
        levelSelect.SetActive(true);
    }
    public void BackToMenuControl(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.performed){
            EventSystem.current.SetSelectedGameObject(botonInicio);
            menuPrincipal.SetActive(true);
            levelSelect.SetActive(false);
            controlMenu.SetActive(false);
        }
    }
    public void BackToMenu()
    {
        EventSystem.current.SetSelectedGameObject(botonInicio);
        menuPrincipal.SetActive(true);
        levelSelect.SetActive(false);
        controlMenu.SetActive(false);
    }
    public void EnterLevel(int level)
    {
        SceneManager.LoadScene("Nivel " +  level);
        GameManager.controlMando = playerInput.currentControlScheme.Equals("Gamepad");
    }
    public void Controls()
    {
        EventSystem.current.SetSelectedGameObject(botonControl);
        menuPrincipal.SetActive(false);
        controlMenu.SetActive(true);
        controlesTeclado.SetActive(!GameManager.controlMando);
        controlesMando.SetActive(GameManager.controlMando);
    }
    public void CambiarControles()
    {
        GameManager.controlMando = !GameManager.controlMando;
        controlesTeclado.SetActive(!GameManager.controlMando);
        controlesMando.SetActive(GameManager.controlMando);
    }

}
