﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//need to really change the name to piece buttons from player buttons

public class PlayerButtons : MonoBehaviour
{
   
    protected Button[] mButtons;

    public void Setup(GameObject pieceButtonsObject)
    {
        mButtons = pieceButtonsObject.GetComponentsInChildren<Button>();
    }


    void Start()
    {

    }

    

    // Update is called once per frame
    void Update()
    {
        
        
    }



    //get the specific button to show
    public void Show(string buttonName)
    {
        foreach(Button b in mButtons)
        {
            if (b.name.Equals(buttonName))
            { b.gameObject.SetActive(true); break; }
        }
    }
    //get the specific button to hide
    public void Hide(string buttonName)
    {
        foreach(Button b in mButtons)
        {
            if (b.name.Equals(buttonName))
            { b.gameObject.SetActive(false); break; }
        }
    }
    //Hide all buttons
    public void HideAll()
    {
        foreach(Button b in mButtons)
        {
            b.gameObject.SetActive(false);
        }
    }
    //Show all Buttons
    public void ShowAll()
    {
        foreach (Button b in mButtons)
        {
            b.gameObject.SetActive(true);
        }
    }


}
