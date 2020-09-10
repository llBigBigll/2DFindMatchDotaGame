﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject MenuCanvas;
    public GameObject GameplayCanvas;

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
    }

    public void SetToGameMenu() 
    {
        MenuCanvas.SetActive(false);
        GameplayCanvas.SetActive(true);
    }
}