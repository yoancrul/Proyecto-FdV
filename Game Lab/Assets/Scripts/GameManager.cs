using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static bool pausado = false;
    public GameObject pauseMenu;
    public GameObject controlMenu;
    public static bool controlMando = false;
    public GameObject controlesTeclado;
    public GameObject controlesMando;
    public GameObject player;
    public GameObject botonInicio;
    public GameObject botonControl;
    private PlayerInput playerInput;
    public GameObject botonesMando;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        playerInput = player.GetComponent<PlayerInput>();
        EventManager.OnTimerStart();
        pauseMenu.SetActive(false);
        controlMenu.SetActive(false);
        botonesMando.SetActive(false);
        if (Time.timeScale == 0)
            pausado = true;
    }
    
    public void PauseGame(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.performed){
            Time.timeScale = 0;
            pausado = true;
            EventSystem.current.SetSelectedGameObject(botonInicio);
            playerInput.neverAutoSwitchControlSchemes = false;
            controlMenu.SetActive(false);
            pauseMenu.SetActive(true);
            botonesMando.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausado = false;
        playerInput.neverAutoSwitchControlSchemes = true;
        if(controlMando){
            playerInput.SwitchCurrentControlScheme("Gamepad",Gamepad.all[0]);
            
        } else {
            playerInput.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
        }
        pauseMenu.SetActive(false);
        botonesMando.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GoToMainMenu()
    {
        pausado = false;
        SceneManager.LoadScene("Menu Principal");
    }
    public void RestartLevel()
    {
        pausado = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Controls()
    {
        EventSystem.current.SetSelectedGameObject(botonControl);
        pauseMenu.SetActive(false);
        controlMenu.SetActive(true);
        controlesTeclado.SetActive(!controlMando);
        controlesMando.SetActive(controlMando);
    }
    public void CambiarControles()
    {
        controlMando = !controlMando;
        controlesTeclado.SetActive(!controlMando);
        controlesMando.SetActive(controlMando);
    }
    public void ReturnControl(InputAction.CallbackContext callbackContext){
        if(pausado && callbackContext.performed){
            if(!pauseMenu.activeSelf){
                EventSystem.current.SetSelectedGameObject(botonInicio);
                pauseMenu.SetActive(true);
                controlMenu.SetActive(false);
            } else {
                ResumeGame();
            }
        }
    }
    public void ReturnButton(){
        EventSystem.current.SetSelectedGameObject(botonInicio);
        pauseMenu.SetActive(true);
        controlMenu.SetActive(false);
    }
}
