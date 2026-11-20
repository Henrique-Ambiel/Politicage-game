
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject characterCreationPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject endingPanel;

    [Header("President")]
    [SerializeField] private TMP_Text presidentNameText;

    [Header("Indicators")]
    [SerializeField] private TMP_Text sovereigntyText;
    [SerializeField] private TMP_Text budgetText;
    [SerializeField] private TMP_Text popularityText;

    [Header("Crisis UI")]
    [SerializeField] private TMP_Text crisisTitleText;
    [SerializeField] private TMP_Text crisisDescriptionText;

    [SerializeField] private Button optionAButton;
    [SerializeField] private TMP_Text optionAButtonText;

    [SerializeField] private Button optionBButton;
    [SerializeField] private TMP_Text optionBButtonText;

    [Header("Victory UI")]
    [SerializeField] private TMP_Text victoryTitleText;
    [SerializeField] private TMP_Text victoryDescriptionText;

    [Header("Defeat UI")]
    [SerializeField] private TMP_Text defeatTitleText;
    [SerializeField] private TMP_Text defeatDescriptionText;

    [Header("Ending Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    [Header("Crisis Data")]
    [SerializeField] private CrisisData[] crises;

    [Header("President Skin")]
    [SerializeField] private Image presidentImage;
    [SerializeField] private Sprite[] presidentSkins;

    [Header("Ending Panels")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

    [Header("Crisis Counter")]
    [SerializeField] private TMP_Text crisisCounterText;

    // Indicadores do país
    private int sovereignty;
    private int budget;
    private int popularity;

    // Controle da partida
    private int currentCrisisIndex;
    private bool decisionResolved;

    private void Awake()
    {
        optionAButton.onClick.AddListener(
            () => MakeDecision(true)
        );

        optionBButton.onClick.AddListener(
            () => MakeDecision(false)
        );

        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(ReturnToMenu);
    }

    // =========================
    // INICIAR PARTIDA
    // =========================

    public void StartGame()
    {
        sovereignty = 50;
        budget = 60;
        popularity = 60;

        currentCrisisIndex = 0;
        decisionResolved = false;

        // Aplica a skin escolhida.
        int skinIndex = PresidentData.SkinIndex;

        if (presidentSkins != null &&
            skinIndex >= 0 &&
            skinIndex < presidentSkins.Length)
        {
            presidentImage.sprite = presidentSkins[skinIndex];
            presidentImage.enabled = true;
        }
        else
        {
            Debug.LogError("GameManager: Skin do presidente inválida!");
        }

        PresidentData.PresidentName =
            PresidentData.PresidentName.Trim();

        // Exibe gameplay e inicia sua música.
        ShowGamePanel();

        presidentNameText.text =
            PresidentData.PresidentName;

        UpdateIndicators();

        // Verifica se existem crises.
        if (crises == null || crises.Length == 0)
        {
            Debug.LogError("GameManager: Nenhuma crise foi cadastrada!");
            return;
        }

        ShowCrisis();
    }

    // =========================
    // EXIBIR GAMEPLAY
    // =========================

    private void ShowGamePanel()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        characterCreationPanel.SetActive(false);

        endingPanel.SetActive(false);
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);

        gamePanel.SetActive(true);

        // Troca para a música de gameplay.
        if (GameAudio.Instance != null)
        {
            GameAudio.Instance.PlayGameplayMusic();
        }
    }

    // =========================
    // EXIBIR CRISE
    // =========================

    private void ShowCrisis()
    {
        if (currentCrisisIndex >= crises.Length)
        {
            ShowSuccessfulEnding();
            return;
        }

        CrisisData crisis = crises[currentCrisisIndex];

        decisionResolved = false;

        crisisCounterText.text =
            $"Crise: {currentCrisisIndex + 1}/{crises.Length}";

        crisisTitleText.text = crisis.crisisTitle;
        crisisDescriptionText.text = crisis.crisisDescription;

        optionAButtonText.text = crisis.optionA.optionText;
        optionBButtonText.text = crisis.optionB.optionText;

        optionAButton.interactable = true;
        optionBButton.interactable = true;
    }

    // =========================
    // TOMAR DECISÃO
    // =========================


    private void MakeDecision(bool chooseA)
    {
        // Evita processar dois cliques na mesma crise.
        if (decisionResolved)
        {
            return;
        }

        decisionResolved = true;

        // Novo som dos botões de decisão.
        if (GameAudio.Instance != null)
        {
            GameAudio.Instance.PlayGameplayClick();
        }

        optionAButton.interactable = false;
        optionBButton.interactable = false;

        CrisisData crisis = crises[currentCrisisIndex];

        CrisisOption option = chooseA
            ? crisis.optionA
            : crisis.optionB;

        ApplyDecision(option);

        UpdateIndicators();

        if (CheckDefeat())
        {
            return;
        }

        currentCrisisIndex++;

        ShowCrisis();
    }


    private void ApplyDecision(CrisisOption option)
    {
        sovereignty += option.sovereigntyChange;
        budget += option.budgetChange;
        popularity += option.popularityChange;

        sovereignty = Mathf.Clamp(sovereignty, 0, 100);
        budget = Mathf.Clamp(budget, 0, 100);
        popularity = Mathf.Clamp(popularity, 0, 100);
    }

    // =========================
    // ATUALIZAR INDICADORES
    // =========================

    private void UpdateIndicators()
    {
        sovereigntyText.text = "Soberania: " + sovereignty;
        budgetText.text = "Orçamento: " + budget;
        popularityText.text = "Popularidade: " + popularity;
    }

    // =========================
    // VERIFICAR DERROTA
    // =========================

    private bool CheckDefeat()
    {
        if (sovereignty <= 0)
        {
            ShowDefeatEnding(
                "Perda da soberania",
                "O Brasil perdeu o controle de suas decisões."
            );

            return true;
        }

        if (budget <= 0)
        {
            ShowDefeatEnding(
                "Colapso financeiro",
                "O orçamento chegou a zero e o governo entrou em colapso."
            );

            return true;
        }

        if (popularity <= 0)
        {
            ShowDefeatEnding(
                "Revolta popular",
                "A população perdeu a confiança no presidente."
            );

            return true;
        }

        return false;
    }

    // =========================
    // FINAL DE VITÓRIA
    // =========================


    private void ShowSuccessfulEnding()
    {
        // Para a música de gameplay.
        if (GameAudio.Instance != null)
        {
            GameAudio.Instance.StopMusic();
        }

        gamePanel.SetActive(false);
        endingPanel.SetActive(false);

        victoryPanel.SetActive(true);
        defeatPanel.SetActive(false);

        victoryTitleText.text = "Mandato concluído";

        victoryDescriptionText.text =
            "Você superou todas as crises e concluiu seu mandato.";

        // Toca apenas o efeito sonoro de vitória.
        if (GameAudio.Instance != null)
        {
            GameAudio.Instance.PlayVictory();
        }
    }

    // =========================
    // FINAL DE DERROTA
    // =========================

    private void ShowDefeatEnding(string title, string description)
    {
        // Para a música de gameplay.
        if (GameAudio.Instance != null)
        {
            GameAudio.Instance.StopMusic();
        }

        gamePanel.SetActive(false);
        endingPanel.SetActive(false);

        victoryPanel.SetActive(false);
        defeatPanel.SetActive(true);

        defeatTitleText.text = title;
        defeatDescriptionText.text = description;

        // Toca apenas o efeito sonoro de derrota.
        if (GameAudio.Instance != null)
        {
            GameAudio.Instance.PlayDefeat();
        }
    }

    // =========================
    // REINICIAR
    // =========================

    public void RestartGame()
    {
        if (GameAudio.Instance != null)
        {
            GameAudio.Instance.PlayClick();
        }

        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);

        // StartGame troca novamente para a música de gameplay.
        StartGame();
    }

    // =========================
    // VOLTAR AO MENU
    // =========================

    public void ReturnToMenu()
    {
        if (GameAudio.Instance != null)
        {
            GameAudio.Instance.PlayClick();
        }

        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        endingPanel.SetActive(false);
        gamePanel.SetActive(false);

        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        characterCreationPanel.SetActive(false);

        // IMPORTANTE:
        // Ao voltar, troca a música de gameplay pela música do menu.
        if (GameAudio.Instance != null)
        {
            GameAudio.Instance.PlayMenuMusic();
        }
    }
}
