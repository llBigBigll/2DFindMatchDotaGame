using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject MenuCanvas;
    public GameObject GameplayCanvas;
    public GameObject GameLogicHandler;
    public GameObject MenuLogicHandler;

    // Start is called before the first frame update
    void Start()
    {
        SetToMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetToMainMenu() 
    {
        MenuCanvas.SetActive(true);
        GameplayCanvas.SetActive(false);
        MenuLogicHandler.GetComponent<MenuLogicHandler>().isInMenu = true;
    }

    public void SetToGameMenu() 
    {
        GameLogicHandler.GetComponent<GameLogic>().RestartMg();
        MenuCanvas.SetActive(false);
        GameplayCanvas.SetActive(true);
    }
}
