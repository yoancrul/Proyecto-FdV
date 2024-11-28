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
    // Start is called before the first frame update
    void Start()
    {
        playerInput = player.GetComponent<PlayerInput>();
        EventManager.OnTimerStart();
        pauseMenu.SetActive(false);
        controlMenu.SetActive(false);
        if (Time.timeScale == 0)
            pausado = true;
    }
    
    public void PauseGame(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.performed){
            EventSystem.current.SetSelectedGameObject(botonInicio);
            playerInput.neverAutoSwitchControlSchemes = false;
            controlMenu.SetActive(false);
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            pausado = true;
        }
    }

    public void ResumeGame()
    {
        playerInput.neverAutoSwitchControlSchemes = true;
        if(controlMando){
            playerInput.SwitchCurrentControlScheme("Gamepad",Gamepad.all[0]);
            
        } else {
            playerInput.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
        }
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        pausado = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GoToMainMenu()
    {
        if (pausado)
            ResumeGame();
        SceneManager.LoadScene("Menu Principal");
    }
    public void RestartLevel()
    {
        if (pausado)
        {
            ResumeGame();
        }
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
