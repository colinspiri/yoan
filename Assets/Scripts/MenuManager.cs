using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public GameObject menu;
    public List<GameObject> otherUIObjects;

    private bool menuActive;
    
    // Start is called before the first frame update
    void Start()
    {
        ShowMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(menuActive) HideMenu();
            else ShowMenu();
        }
    }

    private void ShowMenu() {
        menuActive = true;
        menu.SetActive(true);
        
        // disable other UI
        foreach (var uiObject in otherUIObjects) {
            uiObject.SetActive(false);
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        Time.timeScale = 0;
    }

    private void HideMenu() {
        menuActive = false;
        menu.SetActive(false);
        
        // enable other UI
        foreach (var uiObject in otherUIObjects) {
            uiObject.SetActive(true);
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        Time.timeScale = 1;
    }

    public void Play() {
        HideMenu();
    }

    public void Quit() {
        Application.Quit();
    }
}
