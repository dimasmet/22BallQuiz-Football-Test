using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreensHandler : MonoBehaviour
{
    public enum ScreenName
    {
        HomeScreen,
        QuestionScreen,
        Settings
    }

    public GameObject homeScreen;
    public GameObject questionScreen;
    public GameObject settingsScreen;

    public static Action<ScreenName> OpenScreen;

    private GameObject activeScreen;

    private void Start()
    {
        OpenScreen += ScreenOpen;

        ScreenOpen(ScreenName.HomeScreen);
    }

    private void OnDestroy()
    {
        OpenScreen -= ScreenOpen;
    }

    public void ScreenOpen(ScreenName screen)
    {
        if (activeScreen != null) activeScreen.SetActive(false);

        switch (screen)
        {
            case ScreenName.HomeScreen:
                activeScreen = homeScreen;
                break;
            case ScreenName.QuestionScreen:
                activeScreen = questionScreen;
                break;
            case ScreenName.Settings:
                activeScreen = settingsScreen;
                break;
        }

        activeScreen.SetActive(true);
    }
}
