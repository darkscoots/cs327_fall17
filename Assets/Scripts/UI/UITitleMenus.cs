﻿// Author(s): Paul Calande

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UITitleMenus : MonoBehaviour
{
    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void GoToHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void GoToAbout()
    {
        SceneManager.LoadScene("About");
    }
}