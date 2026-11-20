
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject characterCreationPanel;

    [Header("Settings")]
    [SerializeField] private Slider volumeSlider;

    [Header("Exit Message")]
    [SerializeField] private TMP_Text exitMessage;

    [Header("Additional Panels")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject creditsPanel;


    private void Start()
    {
        AudioListener.volume = 1f;

        ConfigureVolumeSlider();

        ShowMainMenu();
    }

    // =========================
    // MENU PRINCIPAL
    // =========================

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);

        settingsPanel.SetActive(false);
        characterCreationPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        creditsPanel.SetActive(false);

        if (exitMessage != null)
        {
            exitMessage.text = "";
        }

        // Inicia ou retoma a música do menu.
        if (GameAudio.Instance != null)
        {
            GameAudio.Instance.PlayMenuMusic();
        }
        else
        {
            Debug.LogWarning("MenuUI: GameAudio não foi encontrado!");
        }
    }

    // =========================
    // NAVEGAÇÃO
    // =========================

    public void OpenCharacterCreation()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        characterCreationPanel.SetActive(true);

        if (exitMessage != null)
        {
            exitMessage.text = "";
        }
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        characterCreationPanel.SetActive(false);

        tutorialPanel.SetActive(false);
        creditsPanel.SetActive(false);

        if (exitMessage != null)
        {
            exitMessage.text = "";
        }
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        tutorialPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void BackToSettings()
    {
        tutorialPanel.SetActive(false);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // =========================
    // VOLUME
    // =========================


    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        AudioListener.volume = volume;

        Debug.Log("Volume global atualizado: " + volume);
    }


    private void ConfigureVolumeSlider()
    {
        if (volumeSlider == null)
        {
            Debug.LogError("MenuUI: VolumeSlider não foi conectado!");
            return;
        }

        // Configura o intervalo do volume.
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;

        // Define o volume inicial sem disparar o evento.
        volumeSlider.SetValueWithoutNotify(AudioListener.volume);

        // Evita registrar o mesmo evento mais de uma vez.
        volumeSlider.onValueChanged.RemoveListener(SetVolume);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }


    // =========================
    // SAIR
    // =========================

    public void ExitGame()
    {
        if (exitMessage != null)
        {
            exitMessage.text =
                "Obrigado por jogar! Você pode fechar esta aba.";
        }

        // Application.Quit não fecha a aba de um navegador.
        Application.Quit();

        Debug.Log("Solicitação para sair do jogo.");
    }
}
