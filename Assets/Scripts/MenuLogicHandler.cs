using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLogicHandler : MonoBehaviour
{
    public GameObject ButtonsHandler;
    public GameObject SettingsHandler;
    public GameObject GameLogicHandler;

    public GameObject CanvasManager;

    public bool isInMenu;
    private bool isInSettings;
    void Start() 
    {
        changeToMainMenu();
        isInMenu = true;
    }

    public void onPlayClicked() 
    {
        CanvasManager.GetComponent<CanvasManager>().SetToGameMenu();
        isInMenu = false;
    }

    public void onSettingsClicked() 
    {
        changeToSettings();
    }

    public void onQuitClicked() 
    {
        Application.Quit();
    }

    public void inSettingsPlaseholder() 
    {
        changeToMainMenu();
    }

    private void changeToSettings()
    {
        isInSettings = true;
        ButtonsHandler.SetActive(false);
        SettingsHandler.SetActive(true);
    }
    private void changeToMainMenu()
    {
        isInSettings = false;
        ButtonsHandler.SetActive(true);
        SettingsHandler.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInSettings)
        {
            if (isInMenu)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Application.Quit();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                changeToMainMenu();
            }
        }
    }
}
