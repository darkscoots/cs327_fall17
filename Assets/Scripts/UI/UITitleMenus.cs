﻿// Author(s): Paul Calande, Yixiang Xu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITitleMenus : MonoBehaviour
{
	public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
        AudioController.Instance.StopAllSFX();
        AudioController.Instance.MenuClick();
        AudioController.Instance.StopMusic();
        AudioController.Instance.PlayMusic("Main_Menu_Music_1");
        //Destroy(TutorialController.Instance.gameObject);
    }

    public void GoToHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
        AudioController.Instance.MenuClick();
    }

	public void GoToSettings()
	{
		SceneManager.LoadScene("Settings");
        AudioController.Instance.MenuClick();
    }

    public void GoToAbout()
    {
        SceneManager.LoadScene("About");
        AudioController.Instance.MenuClick();
    }
	public void GoToGamePlay(bool isZenMode) 
	{
        Settings.Instance.SetZenModeEnabled(isZenMode);
		SceneManager.LoadScene ("MainScene");
        AudioController.Instance.MenuClick();
        AudioController.Instance.StopMusic();
        AudioController.Instance.PlayMusic("Gameplay_Music_1");
    }
    public void GoToCheatPage()
    {
        SceneManager.LoadScene("CheatPage");
        AudioController.Instance.MenuClick();
    }
}