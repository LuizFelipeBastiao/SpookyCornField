using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ammoText;

    public GameObject endScreen;
    public TextMeshProUGUI endMessage;

    public GameObject player;
    private PlayerController playerController;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnAmmoChanged += UpdateAmmoText;
            GameManager.Instance.OnPlayerWin += ShowWinScreen;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnAmmoChanged -= UpdateAmmoText;
            GameManager.Instance.OnPlayerWin -= ShowWinScreen;
        }
    }

    private void Awake()
    {
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
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

    void ShowWinScreen()
    {
        endScreen.SetActive(true);
        endMessage.text = "You Survived the Cornfield!";
    }

}
