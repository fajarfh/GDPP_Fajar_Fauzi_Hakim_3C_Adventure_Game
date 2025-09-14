using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private string _mainMenuSceneName;
    private void Start()
    {
        _inputManager.onMainMenuInput += BackToMainMenu;
    }

    private void OnDestroy()
    {
        _inputManager.onMainMenuInput -= BackToMainMenu;
    }
    private void BackToMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(_mainMenuSceneName);
    }
}
