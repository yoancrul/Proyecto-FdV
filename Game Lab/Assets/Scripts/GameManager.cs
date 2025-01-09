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
    public GameObject UIGeneral;
    public GameObject pauseMenu;
    public GameObject controlMenu;
    public GameObject opciones;
    public static bool controlMando = false;
    public GameObject controlesTeclado;
    public GameObject controlesMando;
    public GameObject player;
    public GameObject botonInicio;
    public GameObject botonControl;
    public GameObject botonOpciones;
    public GameObject botonFin;
    public bool anticaidas;
    private bool tutorial;
    private PlayerInput playerInput;
    public GameObject botonesMando;
    public GameObject finalNivel;
    public TMP_Text tiempoFinalTexto; // Referencia al texto de tiempo final en la pantalla de fin del nivel
    public int siguienteNivel;

    public Toggle tutorialToggle; // Toggle para activar o desactivar tutoriales
    private const string TUTORIAL_KEY = "Tutorial"; // Clave para guardar el estado del tutorial
    private bool isUpdatingToggleTutorial = false;

    public Toggle anticaidasToggle;
    private const string ANTICAIDAS_KEY = "Anticaidas";
    private bool isUpdatingToggleAnticaidas = false;

    public static Boolean enOtroMenu = false;

    void Start()
    {
        Time.timeScale = 1;
        enOtroMenu = false;
        playerInput = player.GetComponent<PlayerInput>();
        EventManager.OnTimerStart();
        UIGeneral.SetActive(true);
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
    }

    public void ToggleTutorial()
    {
        if (isUpdatingToggleTutorial) return; // Prevenir recursión infinita

        isUpdatingToggleTutorial = true; // Indicar que estamos actualizando el Toggle
        tutorial = !tutorial;

        if (tutorialToggle != null)
        {
            tutorialToggle.isOn = tutorial; // Actualizar el Toggle si se cambia desde otro lugar
        }

        // Guardar el nuevo estado
        PlayerPrefs.SetInt(TUTORIAL_KEY, tutorial ? 1 : 0);
        PlayerPrefs.Save();
        isUpdatingToggleTutorial = false; // Finalizar la actualización del Toggle
    }


    void OnDestroy()
    {
        // Quitar el listener del Toggle al destruir el objeto
        if (anticaidasToggle != null)
        {
            anticaidasToggle.onValueChanged.RemoveListener(delegate { ToggleAnticaidasFromUI(); });
        }
        if (tutorialToggle != null)
        {
            tutorialToggle.onValueChanged.RemoveListener(delegate { ToggleTutorialFromUI(); });
        }
    }

    public void ToggleAnticaidasFromUI()
    {
        if (isUpdatingToggleAnticaidas) return; // Prevenir recursión infinita

        anticaidas = anticaidasToggle.isOn; // Actualizar anticaidas con el estado del Toggle
        PlayerPrefs.SetInt(ANTICAIDAS_KEY, anticaidas ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ToggleAnticaidas()
    {
        if (isUpdatingToggleAnticaidas) return; // Prevenir recursión infinita

        isUpdatingToggleAnticaidas = true; // Indicar que estamos actualizando el Toggle
        anticaidas = !anticaidas;

        if (anticaidasToggle != null)
        {
            anticaidasToggle.isOn = anticaidas; // Actualizar el Toggle si se cambia desde otro lugar
        }

        // Guardar el nuevo estado
        PlayerPrefs.SetInt(ANTICAIDAS_KEY, anticaidas ? 1 : 0);
        PlayerPrefs.Save();
        isUpdatingToggleAnticaidas = false; // Finalizar la actualización del Toggle
    }

    public void PauseOrResumeGame(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if(!enOtroMenu){
                if (!pausado)
                {
                    Time.timeScale = 0;
                    pausado = true;
                    EventSystem.current.SetSelectedGameObject(botonInicio);
                    playerInput.neverAutoSwitchControlSchemes = false;
                    UIGeneral.SetActive(false);
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
    }


    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausado = false;
        playerInput.neverAutoSwitchControlSchemes = true;
        if (controlMando)
        {
            if(Gamepad.all.Count > 0){
                playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);
            } else {
                controlMando = !controlMando;
                playerInput.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
            }
        }
        else
        {
            playerInput.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
        }
        UIGeneral.SetActive(true);
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
        enOtroMenu = false;
        SceneManager.LoadScene("Menu Principal");
    }
    public void RestartLevel()
    {
        pausado = false;
        enOtroMenu = false;
        playerInput.neverAutoSwitchControlSchemes = true;
        if (controlMando)
        {
            if(Gamepad.all.Count > 0){
                playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);
            } else {
                controlMando = !controlMando;
                playerInput.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
            }
        }
        else
        {
            playerInput.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
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
    public void ReturnControl(InputAction.CallbackContext callbackContext)
    {
        if (pausado && !enOtroMenu && callbackContext.performed)
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
            pausado = false;
            enOtroMenu = false;
            SceneManager.LoadScene("Nivel " + siguienteNivel);
        }
    }
    public void FinalNivel(float tiempoFinal)
    {
        enOtroMenu = true;
        pausado = true;
        EventSystem.current.SetSelectedGameObject(botonFin);
        finalNivel.SetActive(true);
        UIGeneral.SetActive(false);

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
