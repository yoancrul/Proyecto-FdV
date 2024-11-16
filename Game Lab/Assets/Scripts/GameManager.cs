using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool pausado = false;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
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
        if (Input.GetKeyDown("escape")){
            if (!pausado)
            {
                PauseGame();
            }
            else{
                ResumeGame();
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
}
