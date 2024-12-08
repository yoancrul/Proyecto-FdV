using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool pausado = false;
    public GameObject pauseMenu;
    public GameObject controlMenu;
    public GameObject opciones;
    public static bool controlMando = false;
    public static bool tutorial = false;
    public GameObject controlesTeclado;
    public GameObject controlesMando;
    public GameObject player;
    public GameObject botonInicio;
    public GameObject botonControl;
    public GameObject botonOpciones;
    public bool anticaidas;
    private PlayerInput playerInput;
    public GameObject botonesMando;

    public Toggle anticaidasToggle; // El objeto Toggle en el menú de opciones

    private const string ANTICAIDAS_KEY = "Anticaidas"; // Clave para PlayerPrefs

    private bool isUpdatingToggle = false; // Bandera para evitar recursión infinita

    void Start()
    {
        Time.timeScale = 1;
        playerInput = player.GetComponent<PlayerInput>();
        EventManager.OnTimerStart();
        pauseMenu.SetActive(false);
        opciones.SetActive(false);
        controlMenu.SetActive(false);
        botonesMando.SetActive(false);
        if (Time.timeScale == 0)
            pausado = true;

        // Cargar el estado de "anticaidas" de PlayerPrefs
        if (PlayerPrefs.HasKey(ANTICAIDAS_KEY))
        {
            anticaidas = PlayerPrefs.GetInt(ANTICAIDAS_KEY) == 0; // 1 = true, 0 = false
        }
        else
        {
            anticaidas = false; // Valor predeterminado
            PlayerPrefs.SetInt(ANTICAIDAS_KEY, 0);
            PlayerPrefs.Save();
        }

        // Sincronizar el Toggle con el valor de anticaidas
        if (anticaidasToggle != null)
        {
            anticaidasToggle.isOn = anticaidas; // Inicializar el estado del Toggle
            anticaidasToggle.onValueChanged.AddListener(delegate { ToggleAnticaidasFromUI(); }); // Escuchar cambios
        }
    }

    void OnDestroy()
    {
        // Quitar el listener del Toggle al destruir el objeto
        if (anticaidasToggle != null)
        {
            anticaidasToggle.onValueChanged.RemoveListener(delegate { ToggleAnticaidasFromUI(); });
        }
    }

    public void ToggleAnticaidasFromUI()
    {
        if (isUpdatingToggle) return; // Prevenir recursión infinita

        anticaidas = anticaidasToggle.isOn; // Actualizar anticaidas con el estado del Toggle
        PlayerPrefs.SetInt(ANTICAIDAS_KEY, anticaidas ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleAnticaidas()
    {
        if (isUpdatingToggle) return; // Prevenir recursión infinita

        isUpdatingToggle = true; // Indicar que estamos actualizando el Toggle
        anticaidas = !anticaidas;

        if (anticaidasToggle != null)
        {
            anticaidasToggle.isOn = anticaidas; // Actualizar el Toggle si se cambia desde otro lugar
        }

        // Guardar el nuevo estado
        PlayerPrefs.SetInt(ANTICAIDAS_KEY, anticaidas ? 1 : 0);
        PlayerPrefs.Save();
        isUpdatingToggle = false; // Finalizar la actualización del Toggle
    }

    public void PauseOrResumeGame(InputAction.CallbackContext callbackContext)
    {
        if (!tutorial)
        {
            if (callbackContext.performed)
            {
                if(!pausado){
                    Time.timeScale = 0;
                    pausado = true;
                    EventSystem.current.SetSelectedGameObject(botonInicio);
                    playerInput.neverAutoSwitchControlSchemes = false;
                    controlMenu.SetActive(false);
                    pauseMenu.SetActive(true);
                    botonesMando.SetActive(true);
                    opciones.SetActive(false);
                } else {
                    ResumeGame();
                }
                
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausado = false;
        playerInput.neverAutoSwitchControlSchemes = true;
        if (controlMando)
        {
            playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);

        }
        else
        {
            playerInput.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
        }
        pauseMenu.SetActive(false);
        opciones.SetActive(false);
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
    public void ReturnControl(InputAction.CallbackContext callbackContext)
    {
        if (pausado && !tutorial && callbackContext.performed)
        {
            if (!pauseMenu.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(botonInicio);
                pauseMenu.SetActive(true);
                controlMenu.SetActive(false);
            }
            else
            {
                ResumeGame();
            }
        }
    }
    public void ReturnButton()
    {
        EventSystem.current.SetSelectedGameObject(botonInicio);
        pauseMenu.SetActive(true);
        controlMenu.SetActive(false);
        opciones.SetActive(false);
    }
    public void AbrirOpciones()
    {
        EventSystem.current.SetSelectedGameObject(botonOpciones);
        pauseMenu.SetActive(false);
        opciones.SetActive(true);
    }
}
