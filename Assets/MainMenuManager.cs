using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource clickSource;
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private TextMeshProUGUI masterVolText;

    private int minVol = 0;
    private int maxVol = 2;
    private float storedVol;
    private bool muted = false;
    private bool canClick = true;

    private void Awake()
    {
        Time.timeScale = 1f;
        UpdateMasterVolText();
        fadeAnimator.Play("FadeFromBlack", -1, 0f);
        storedVol = AudioListener.volume;
    }

    private void UpdateMasterVolText()
    {
        masterVolText.text = ((int)World.remap(AudioListener.volume, minVol, maxVol, 0, 10)).ToString();
    }

    public void Play()
    {
        if(canClick)
        {
            canClick = false;
            fadeAnimator.Play("FadeToBlack");
            StartCoroutine("WaitToPlay");
            clickSource.Play();
        }
        
    }

    private IEnumerator WaitToPlay()
    {
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("Main");
    }

    public void Exit()
    {
        clickSource.Play();
        Application.Quit();
    }

    public void VolUp()
    {
        if(AudioListener.volume < maxVol && !muted)
        {
            AudioListener.volume += 0.2f;
            UpdateMasterVolText();
            clickSource.Play();
            storedVol = AudioListener.volume;
        }
    }

    public void VolDown()
    {
        if (AudioListener.volume > minVol && !muted)
        {
            AudioListener.volume -= 0.2f;
            UpdateMasterVolText();
            clickSource.Play();
            storedVol = AudioListener.volume;
        }
    }

    public void Mute()
    {
        if(AudioListener.volume != 0)
        {
            AudioListener.volume = 0;
            muted = true;
        }
        else
        {
            muted = false;
            AudioListener.volume = storedVol;
        }
    }
}
