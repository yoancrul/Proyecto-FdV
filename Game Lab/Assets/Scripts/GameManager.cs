using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool pausado = false;
    public GameObject pauseMenu;
    public GameObject controlMenu;
    private bool controles = false;
    public static bool controlMando = false;
    public GameObject controlesTeclado;
    public GameObject controlesMando;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnTimerStart();
        pauseMenu.SetActive(false);
        controlMenu.SetActive(false);
        if (Time.timeScale == 0)
            pausado = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            RestartLevel();
        }
        if (Input.GetButtonDown("Menu")){
            if (!pausado)
            {
                PauseGame();
            }
            else{
                if(controles){
                    ReturnToPauseMenu();
                } else {
                    ResumeGame();
                }
            }
        }
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        pausado = true;
    }

    public void ResumeGame()
    {
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
        controles = true;
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
    public void ReturnToPauseMenu(){
        controles = false;
        pauseMenu.SetActive(true);
        controlMenu.SetActive(false);
    }
}
