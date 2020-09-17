using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLogicHandler : MonoBehaviour
{
    public GameObject ButtonsHandler;
    public GameObject SettingsHandler;
    public GameObject GameLogicHandler;

    public GameObject CanvasManager;

    void Start() 
    {
        changeToMainMenu();
    }

    public void onPlayClicked() 
    {
        CanvasManager.GetComponent<CanvasManager>().SetToGameMenu();
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
        ButtonsHandler.SetActive(false);
        SettingsHandler.SetActive(true);
    }
    private void changeToMainMenu()
    {
        ButtonsHandler.SetActive(true);
        SettingsHandler.SetActive(false);
    }
}
