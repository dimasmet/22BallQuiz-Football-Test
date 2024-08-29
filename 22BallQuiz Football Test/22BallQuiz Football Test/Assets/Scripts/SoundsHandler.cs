using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundsHandler : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource sounds;

    public AudioClip soundLose;
    public AudioClip takeBonusSound;

    public Button soundBtn;
    public Button musicBtn;

    public Sprite activeSoundSprite;
    public Sprite noactiveSoundSprite;

    private bool isMusic = true;
    private bool isSound = true;

    private void Awake()
    {
        soundBtn.onClick.AddListener(() =>
        {
            SoundsRun();
        });
        musicBtn.onClick.AddListener(() =>
        {
            MusicRun();
        });
    }

    public void MusicRun()
    {
        isMusic = !isMusic;

        if (isMusic)
        {
            soundBtn.transform.GetChild(0).GetComponent<Image>().sprite = activeSoundSprite;
            backgroundMusic.Stop();
        }
        else
        {
            soundBtn.transform.GetChild(0).GetComponent<Image>().sprite = noactiveSoundSprite;
            backgroundMusic.Play();
        }
    }

    public void SoundsRun()
    {
        isSound = !isSound;
    }

    public void RunLoseSound()
    {
        if (isSound)
        {
            sounds.PlayOneShot(soundLose);
        }
    }

    public void RunTakeBonusSound()
    {
        if (isSound)
        {
            sounds.PlayOneShot(takeBonusSound);
        }
    }
}
