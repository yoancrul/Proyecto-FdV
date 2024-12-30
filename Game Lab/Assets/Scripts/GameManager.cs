using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static bool pausado = false;
    public GameObject pauseMenu;
    public GameObject controlMenu;
    public GameObject opciones;
    public static bool controlMando = false;
    public static bool tutorial = true;   
    public GameObject controlesTeclado;
    public GameObject controlesMando;
    public GameObject player;
    public GameObject botonInicio;
    public GameObject botonControl;
    public GameObject botonOpciones;
    public bool anticaidas;
    private PlayerInput playerInput;
    public GameObject botonesMando;
    public GameObject finalNivel;
    public TMP_Text tiempoFinalTexto; // Referencia al texto de tiempo final en la pantalla de fin del nivel
    public int siguienteNivel;

    public Toggle tutorialToggle; // Toggle para activar o desactivar tutoriales
    private const string TUTORIAL_KEY = "Tutorial"; // Clave para guardar el estado del tutorial


    public Toggle anticaidasToggle;

    private const string ANTICAIDAS_KEY = "Anticaidas";
    private bool isUpdatingToggle = false;

    void Start()
    {
        Time.timeScale = 1;
        playerInput = player.GetComponent<PlayerInput>();
        EventManager.OnTimerStart();
        pauseMenu.SetActive(false);
        opciones.SetActive(false);
        controlMenu.SetActive(false);
        botonesMando.SetActive(false);
        finalNivel.SetActive(false);
        if (Time.timeScale == 0)
            pausado = true;

        // Inicializar el estado de anticaídas
        if (PlayerPrefs.HasKey(ANTICAIDAS_KEY))
        {
            anticaidas = PlayerPrefs.GetInt(ANTICAIDAS_KEY) == 1;
        }
        else
        {
            anticaidas = false;
            PlayerPrefs.SetInt(ANTICAIDAS_KEY, 0);
            PlayerPrefs.Save();
        }

        if (anticaidasToggle != null)
        {
            anticaidasToggle.isOn = anticaidas;
            anticaidasToggle.onValueChanged.AddListener(delegate { ToggleAnticaidasFromUI(); });
        }

        // Inicializar el estado del tutorial
        if (PlayerPrefs.HasKey(TUTORIAL_KEY))
        {
            tutorial = PlayerPrefs.GetInt(TUTORIAL_KEY) == 1;
        }
        else
        {
            tutorial = true; // Por defecto, los tutoriales están activados
            PlayerPrefs.SetInt(TUTORIAL_KEY, 1);
            PlayerPrefs.Save();
        }

        if (tutorialToggle != null)
        {
            tutorialToggle.isOn = tutorial;
            tutorialToggle.onValueChanged.AddListener(delegate { ToggleTutorialFromUI(); });
        }
    }
    public void ToggleTutorialFromUI()
    {
        tutorial = tutorialToggle.isOn; // Actualizar la variable tutorial
        PlayerPrefs.SetInt(TUTORIAL_KEY, tutorial ? 1 : 0); // Guardar el estado en PlayerPrefs
        PlayerPrefs.Save();

        // Restablecer el foco en un botón del menú para evitar problemas con el EventSystem
        EventSystem.current.SetSelectedGameObject(botonInicio);
    }

    public void ToggleTutorial()
    {
        tutorial = !tutorial;

        if (tutorialToggle != null)
        {
            tutorialToggle.isOn = tutorial;
        }

        PlayerPrefs.SetInt(TUTORIAL_KEY, tutorial ? 1 : 0);
        PlayerPrefs.Save();
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
        if (callbackContext.performed)
        {
            if (!pausado)
            {
                Time.timeScale = 0;
                pausado = true;
                EventSystem.current.SetSelectedGameObject(botonInicio);
                playerInput.neverAutoSwitchControlSchemes = false;
                controlMenu.SetActive(false);
                pauseMenu.SetActive(true);
                botonesMando.SetActive(true);
                opciones.SetActive(false);
            }
            else
            {
                ResumeGame();
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
        controlMenu.SetActive(false);
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
    public void SiguienteNivel()
    {
        {
            SceneManager.LoadScene("Nivel " + siguienteNivel);
        }
    }
    public void FinalNivel(float tiempoFinal)
    {
        finalNivel.SetActive(true);

        // Actualizar el texto con el tiempo final
        TimeSpan timeSpan = TimeSpan.FromSeconds(tiempoFinal);
        tiempoFinalTexto.text = $"Tiempo Final: {timeSpan:mm\\:ss\\:ff}";

        Time.timeScale = 0;
    }
    public bool TutorialIsOn()
    {
        return tutorial;
    }
}
