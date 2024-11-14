using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool pausado = false;
    // Start is called before the first frame update
    void Start()
    {
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
                pausado = true;
            }
            else{
                ResumeGame();
                pausado = false;
            }
        }
    }
    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
