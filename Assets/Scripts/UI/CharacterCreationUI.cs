
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCreationUI : MonoBehaviour
{
    [Header("Name")]
    [SerializeField] private TMP_InputField nameInput;

    [Header("Skin Buttons")]
    [SerializeField] private Button[] skinButtons;

    [Header("Continue")]
    [SerializeField] private Button continueButton;

    [Header("Button Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.green;

    [Header("Game")]
    [SerializeField] private GameManager gameManager;

    private int selectedSkin = -1;

    private void Awake()
    {
        // Conecta os quatro botões de skin
        for (int i = 0; i < skinButtons.Length; i++)
        {
            int skinIndex = i;

            skinButtons[i].onClick.AddListener(
                () => SelectSkin(skinIndex)
            );
        }

        // Atualiza a validação quando o nome mudar
        nameInput.onValueChanged.AddListener(
            delegate { UpdateValidation(); }
        );

        // Botão continuar
        continueButton.onClick.AddListener(ContinueCreation);
    }

    private void Start()
    {
        UpdateSkinVisuals();
        UpdateValidation();
    }

    private void SelectSkin(int skinIndex)
    {
        selectedSkin = skinIndex;

        UpdateSkinVisuals();
        UpdateValidation();
    }

    private void UpdateSkinVisuals()
    {
        for (int i = 0; i < skinButtons.Length; i++)
        {
            Image buttonImage = skinButtons[i].GetComponent<Image>();

            if (buttonImage != null)
            {
                if (i == selectedSkin)
                {
                    buttonImage.color = selectedColor;
                }
                else
                {
                    buttonImage.color = normalColor;
                }
            }
        }
    }

    private void UpdateValidation()
    {
        bool hasValidName =
            !string.IsNullOrWhiteSpace(nameInput.text);

        bool hasSelectedSkin =
            selectedSkin >= 0 && selectedSkin < skinButtons.Length;

        continueButton.interactable =
            hasValidName && hasSelectedSkin;
    }

    private void ContinueCreation()
    {
        UpdateValidation();

        if (!continueButton.interactable)
        {
            return;
        }

        PresidentData.SavePresident(
            nameInput.text,
            selectedSkin
        );

        if (gameManager == null)
        {
            Debug.LogError("GameManager não foi configurado!");
            return;
        }

        gameManager.StartGame();
    }
}
