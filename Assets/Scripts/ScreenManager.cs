using UnityEngine;
using UnityEngine.EventSystems;

public enum GameLanguage
{
    English,
    Arabic
}

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject startupScreen;
    [SerializeField] private GameObject coreGameScreen;
    [SerializeField] private GameObject englishUICanvas;
    [SerializeField] private GameObject arabicUICanvas;
    [SerializeField] private UnityEngine.UI.Button englishButton;

    public static GameLanguage ActiveLanguage { get; private set; }

    private void Awake()
    {
        startupScreen.SetActive(true);
        coreGameScreen.SetActive(false);
        englishUICanvas.SetActive(false);
        arabicUICanvas.SetActive(false);
    }

  
    public void StartEnglishGame()
    {
        ActiveLanguage = GameLanguage.English;
        TransitionToGameScreen(englishUICanvas);
    }


    public void StartArabicGame()
    {
        ActiveLanguage = GameLanguage.Arabic;
        // Pass the Arabic UI Canvas to the transition logic
        TransitionToGameScreen(arabicUICanvas);
    }

    /// <summary>
    /// Handles the actual screen transition logic.
    /// </summary>
    /// <param name="targetUICanvas">The Canvas GameObject representing the active game UI.</param>
    private void TransitionToGameScreen(GameObject targetUICanvas)
    {
        startupScreen.SetActive(false);

        coreGameScreen.SetActive(true);

        if (targetUICanvas == englishUICanvas)
        {
            englishUICanvas.SetActive(true);
            arabicUICanvas.SetActive(false);
        }
        else if (targetUICanvas == arabicUICanvas)
        {
            arabicUICanvas.SetActive(true);
            englishUICanvas.SetActive(false);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.NewGame();
        }
        else
        {
            Debug.LogError("GameManager instance not found. Ensure GameManager is initialized.");
        }
    }

    public void TransitionToStartScreen()
    {
        coreGameScreen.SetActive(false);
        englishUICanvas.SetActive(false);
        arabicUICanvas.SetActive(false);

        startupScreen.SetActive(true);

        if (englishButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(englishButton.gameObject);
        }

        // 3. Reset the GameManager score (optional, but good practice for cleanup)
        // NOTE: GameManager.Instance must be accessible.
        // if (GameManager.Instance != null)
        // {
        //     GameManager.Instance.SetScore(0); 
        // }
    }
}