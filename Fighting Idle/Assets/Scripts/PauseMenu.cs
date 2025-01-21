using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Resume()
    {
        
        Time.timeScale = 1f;
        
    }

    public void Pause()
    {
        
        Time.timeScale = 0f;
        
    }

    public void GoBackToMainMenuFromPause()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenStatsUpgrades()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
