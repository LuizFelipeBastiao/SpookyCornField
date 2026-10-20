using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ammoText;

    public GameObject endScreen;
    public TextMeshProUGUI endMessage;

    public GameObject pauseMenu;

    public GameObject player;
    private PlayerController playerController;

    public AudioClip somMorte;
    public AudioSource musicaFundo;
    private AudioSource audioSource;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnAmmoChanged += UpdateAmmoText;
            GameManager.Instance.OnPlayerWin += ShowWinScreen;
            GameManager.Instance.OnPlayerLose += ShowLoseScreen;
            GameManager.Instance.OnPauseGame += TogglePauseMenu;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnAmmoChanged -= UpdateAmmoText;
            GameManager.Instance.OnPlayerWin -= ShowWinScreen;
            GameManager.Instance.OnPlayerLose -= ShowLoseScreen;
            GameManager.Instance.OnPauseGame -= TogglePauseMenu;
        }
    }

    private void Awake()
    {
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }

        // AudioSource 2D para os sons da UI (não é afetado pelo Time.timeScale = 0)
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        ammoText.text = "Ammo: " + playerController.ammo.ToString();
    }

    void TogglePauseMenu()
    {
        if (pauseMenu != null)
        {
            bool isActive = pauseMenu.activeSelf;
            pauseMenu.SetActive(!isActive);
        }
    }

    void ShowWinScreen()
    {
        endScreen.SetActive(true);
        endMessage.text = "You Survived the Cornfield!";
    }

    void ShowLoseScreen()
    {
        endScreen.SetActive(true);
        endMessage.text = "You Died!";

        if (musicaFundo != null)
            musicaFundo.Stop();

        if (somMorte != null)
            audioSource.PlayOneShot(somMorte);
    }

}
