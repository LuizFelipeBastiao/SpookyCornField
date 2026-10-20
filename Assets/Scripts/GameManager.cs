using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action OnAmmoChanged;
    public event Action OnPlayerWin;
    public event Action OnPlayerLose;
    public event Action OnPauseGame;
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeAmmo()
    {
        OnAmmoChanged?.Invoke();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnPauseGame?.Invoke();
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        OnPauseGame?.Invoke();
    }

    public void WinGame()
    {
        OnPlayerWin?.Invoke();

        Time.timeScale = 0f;
    }

    public void LoseGame()
    {
        OnPlayerLose?.Invoke();
        
        Time.timeScale = 0f;
    }

    public void MainMenu(){
        SceneManager.LoadScene("Menu");
    }

    void Update()
    {
        
    }
}
