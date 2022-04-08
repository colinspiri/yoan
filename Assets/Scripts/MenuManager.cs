using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;
    
    // components
    public GameObject menu;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverMessage;
    public TextMeshProUGUI gameOverTomatoes;
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
            else if(!gameOverPanel.activeSelf) ShowMenu();
        }
    }

    public void GameOver(bool playerSurvived = true) {
        gameOverPanel.SetActive(true);
        if (playerSurvived) {
            gameOverPanel.GetComponent<Image>().color = Color.black;
            gameOverMessage.text = "You survived.";
            
            gameOverTomatoes.text = "You harvested " + TomatoCounter.Instance.PlayerTomatoes + " tomatoes.\n" +
                                    "The Torbalan stole " + TomatoCounter.Instance.TorbalanTomatoes + " tomatoes.";
        }
        else {
            gameOverMessage.text = "You failed.";
            
            gameOverTomatoes.text = "The Torbalan stole " + TomatoCounter.Instance.TorbalanTomatoes + " tomatoes.\n" +
                                    "He also stole the " + TomatoCounter.Instance.PlayerTomatoes + " you harvested.";
        }
        
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
        
        AudioManager.Instance.StopGameSound();
    }

    private void ResumeGame() {
        // enable other UI
        foreach (var uiObject in otherUIObjects) {
            uiObject.SetActive(true);
        }
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        Time.timeScale = 1;
        
        AudioManager.Instance.ResumeGameSound();
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
