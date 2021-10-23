using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageFinish : MonoBehaviour {
    public GameObject gameOverText;
    public GameObject buttons;
    public string nextScene = "Map";
    public GameObject victoryScreen;
    public AudioClip fanfare;
    public GameObject pauseMenu;
    public SkillsPanel skillsPanel;
    public SkillManager skills;
    
    private CameraControl cameraControl;
    private bool paused;
    private bool panelOpen;

    void Start() {
        cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
        skillsPanel.updateSkills(skills);
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Cancel")) {
            if(!paused) {
                pauseGame();
            } else {
                if(panelOpen) {
                    closePanel();
                } else {
                    unpauseGame();
                }
            }
        }
	}
    
    public void gameOver() {
        Animator anim = gameOverText.GetComponent<Animator>();
        if(anim) {
            anim.SetTrigger("FadeIn");
        }
        Invoke("showButtons", 5f);
    }
    
    void showButtons() {
        buttons.SetActive(true);
    }
    
    public void continueGame() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void quitGame() {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void pauseGame() {
        paused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    
    public void unpauseGame() {
        if(panelOpen) {
            closePanel();
        }
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void openPanel() {
        panelOpen = true;
        skillsPanel.gameObject.SetActive(true);
    }
    
    public void closePanel() {
        panelOpen = false;
        skillsPanel.gameObject.SetActive(false);
    }
    
    public void bossDead() {
        Invoke("playFanfare", 0.5f);
        Invoke("showVictory", 2f);
        //Invoke("finishStage", 5f);
    }
    
    public void playFanfare() {
        cameraControl.changeMusic(fanfare);
    }
    
    public void showVictory() {
        victoryScreen.SetActive(true);
    }
    
    public void finishStage() {
        SceneManager.LoadScene(nextScene);
    }
}
