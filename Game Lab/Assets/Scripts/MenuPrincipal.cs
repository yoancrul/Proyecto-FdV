using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject levelSelect;
    public GameObject controlMenu;
    public GameObject opcionesMenu;
    public GameObject controlesTeclado;
    public GameObject botonesMando;
    public GameObject botonInicio;
    public GameObject botonNiveles;
    public GameObject botonControl;
    public GameObject botonOpciones;
    private PlayerInput playerInput;
    private bool anticaidas;
    private bool tutorial;
    public Toggle anticaidasToggle; // El objeto Toggle en el menú de opciones
    private const string ANTICAIDAS_KEY = "Anticaidas"; // Clave para PlayerPrefs
    private bool isUpdatingToggleAnticaidas = false; // Bandera para evitar recursión infinita
    public Toggle tutorialToggle; // Toggle para activar o desactivar tutoriales
    private const string TUTORIAL_KEY = "Tutorial"; // Clave para guardar el estado del tutorial
    private bool isUpdatingToggleTutorial = false;

    private void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        botonesMando.SetActive(GameManager.controlMando);
        levelSelect.SetActive(false);
        controlMenu.SetActive(false);
        opcionesMenu.SetActive(false);
        // Cargar el estado de "anticaidas" de PlayerPrefs
        if (PlayerPrefs.HasKey(ANTICAIDAS_KEY))
        {
            anticaidas = PlayerPrefs.GetInt(ANTICAIDAS_KEY) == 1; // 1 = true, 0 = false
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
            opcionesMenu.SetActive(false);
        }
    }
    public void BackToMenu()
    {
        EventSystem.current.SetSelectedGameObject(botonInicio);
        menuPrincipal.SetActive(true);
        levelSelect.SetActive(false);
        controlMenu.SetActive(false);
        opcionesMenu.SetActive(false);
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
    }
    public void CambiarControles()
    {
        GameManager.controlMando = !GameManager.controlMando;
        controlesTeclado.SetActive(!GameManager.controlMando);
        botonesMando.SetActive(GameManager.controlMando);
    }
    public void Opciones()
    {
        EventSystem.current.SetSelectedGameObject(botonOpciones);
        menuPrincipal.SetActive(false);
        opcionesMenu.SetActive(true);
    }

}
