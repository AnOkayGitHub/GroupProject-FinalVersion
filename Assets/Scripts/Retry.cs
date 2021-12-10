using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Retry : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private AudioSource clickSource;
    [SerializeField] private Level level;

    private void Start()
    {
        level = GetComponent<Level>();
    }

    public void RetryGame()
    {
        clickSource.Play();
        World.coins = 0;
        World.items = new Dictionary<string, Sprite>();
        SceneManager.LoadScene("Main");
    }

    public void ExitToMain()
    {
        clickSource.Play();
        World.coins = 0;
        World.items = new Dictionary<string, Sprite>();
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
        Debug.Log(Time.timeScale);
    }

    public void SkipIntro(Animator animator)
    {
        animator.speed = 20;
        StartCoroutine("WaitForSkip", animator);
    }

    private IEnumerator WaitForSkip(Animator animator)
    {
        yield return new WaitForSeconds(2f);
        level.introMovieTime = 0;
        World.readyToPlay = true;
        animator.speed = 1;
    }
}
