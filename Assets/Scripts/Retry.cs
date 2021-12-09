using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Retry : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private AudioSource clickSource;

    public void RetryGame()
    {
        clickSource.Play();
        SceneManager.LoadScene("Main");
    }

    public void ExitToMain()
    {
        clickSource.Play();
        SceneManager.LoadScene("Menu");
    }

    public void ExitApplication()
    {
        clickSource.Play();
        Application.Quit();
    }

    public void Settings()
    {
        clickSource.Play();
        settings.SetActive(!settings.activeSelf);
    }

    private void OnDisable()
    {
        if(settings != null)
        {
            settings.SetActive(false);
        }
    }
}
