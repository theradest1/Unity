using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public Canvas menu;
    public GameObject settings;
    public GameObject main;
    public GameObject hidden;
    public GameObject controls;
    
    public void quit(){
        Application.Quit();
    }
    public void open(){
        if(!menu.gameObject.activeSelf){
            menu.gameObject.SetActive(true);
            OpenMenu(main);
        }
        else{
            menu.gameObject.SetActive(false);
        }
    }
    public void OpenMenu(GameObject thing){
        AllOff();
        thing.SetActive(true);
    }
    public void AllOff(){
        main.SetActive(false);
        settings.SetActive(false);
        hidden.SetActive(false);
        controls.SetActive(false);
    }
}
