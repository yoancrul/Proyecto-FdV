using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        pausado = true;
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        pausado = false;
    }
}
