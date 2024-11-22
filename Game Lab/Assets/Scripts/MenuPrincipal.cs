using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject levelSelect;
    private void Start()
    {
        levelSelect.SetActive(false);
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
    }
    public void EnterLevel(int level)
    {
        SceneManager.LoadScene("Nivel " +  level);
    }
}
