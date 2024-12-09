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
    public GameObject controlesMando;
    public GameObject botonInicio;
    public GameObject botonNiveles;
    public GameObject botonControl;
    public GameObject botonOpciones;
    private PlayerInput playerInput;
    private bool anticaidas;
    public Toggle anticaidasToggle; // El objeto Toggle en el menú de opciones
    private const string ANTICAIDAS_KEY = "Anticaidas"; // Clave para PlayerPrefs
    private bool isUpdatingToggle = false; // Bandera para evitar recursión infinita

    private void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
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
        controlesMando.SetActive(GameManager.controlMando);
    }
    public void CambiarControles()
    {
        GameManager.controlMando = !GameManager.controlMando;
        controlesTeclado.SetActive(!GameManager.controlMando);
        controlesMando.SetActive(GameManager.controlMando);
    }
    public void Opciones()
    {
        EventSystem.current.SetSelectedGameObject(botonOpciones);
        menuPrincipal.SetActive(false);
        opcionesMenu.SetActive(true);
    }

}
