using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Play(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Mapa");
    }
}
