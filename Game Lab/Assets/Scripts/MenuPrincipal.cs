using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject levelSelect;
    public GameObject controlMenu;
    public GameObject controlesTeclado;
    public GameObject controlesMando;

    private void Start()
    {
        levelSelect.SetActive(false);
        controlMenu.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Nivel 0");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LevelSelect()
    {
        menuPrincipal.SetActive(false);
        levelSelect.SetActive(true);
    }
    public void BackToMenu()
    {
        menuPrincipal.SetActive(true);
        levelSelect.SetActive(false);
        controlMenu.SetActive(false);
    }
    public void EnterLevel(int level)
    {
        SceneManager.LoadScene("Nivel " +  level);
    }
    public void Controls()
    {
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
