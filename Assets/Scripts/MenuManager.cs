using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;
    
    // components
    public GameObject menu;
    public GameObject gameOverPanel;
    public List<GameObject> otherUIObjects;
    
    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        ShowMenu();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(menu.activeSelf) HideMenu();
            else ShowMenu();
        }

        if (Input.GetKeyDown(KeyCode.U)) {
            GameOver();
        }
    }

    public void GameOver() {
        gameOverPanel.SetActive(true);
        StopGame();
    }

    private void ShowMenu() {
        menu.SetActive(true);
        StopGame();
    }

    private void HideMenu() {
        menu.SetActive(false);
        ResumeGame();
    }

    private void StopGame() {
        // disable other UI
        foreach (var uiObject in otherUIObjects) {
            uiObject.SetActive(false);
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        Time.timeScale = 0;
    }

    private void ResumeGame() {
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
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
