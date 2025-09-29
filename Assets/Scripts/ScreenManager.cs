using UnityEngine;

// Enum to easily track the active language
public enum GameLanguage
{
    English,
    Arabic
}

public class ScreenManager : MonoBehaviour
{
    // The main screens to manage visibility
    [SerializeField] private GameObject startupScreen;

    // The core game components (maze, characters) which stays active regardless of language
    [SerializeField] private GameObject coreGameScreen;

    // ONLY the UI Canvases change based on language
    [SerializeField] private GameObject englishUICanvas;
    [SerializeField] private GameObject arabicUICanvas;

    // Static property to allow other scripts (like localization) to check the active language
    public static GameLanguage ActiveLanguage { get; private set; }

    private void Awake()
    {
        // Ensure the application starts with only the language selection screen visible
        startupScreen.SetActive(true);
        coreGameScreen.SetActive(false); // Game core starts disabled
        englishUICanvas.SetActive(false);
        arabicUICanvas.SetActive(false);
    }

    /// <summary>
    /// Called when the English button is clicked. Activates the English UI.
    /// </summary>
    public void StartEnglishGame()
    {
        ActiveLanguage = GameLanguage.English;
        // Pass the English UI Canvas to the transition logic
        TransitionToGameScreen(englishUICanvas);
    }

    /// <summary>
    /// Called when the Arabic button is clicked. Activates the Arabic UI.
    /// </summary>
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
        // 1. Hide the startup screen
        startupScreen.SetActive(false);

        // 2. Activate the core game components (maze, characters)
        coreGameScreen.SetActive(true);

        // 3. Show the selected language UI Canvas and hide the other
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

        // 4. Kick off the game logic (Assuming GameManager.NewGame is public)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NewGame();
        }
        else
        {
            Debug.LogError("GameManager instance not found. Ensure GameManager is initialized.");
        }
    }
}