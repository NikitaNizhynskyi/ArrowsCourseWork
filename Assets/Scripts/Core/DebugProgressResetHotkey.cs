using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DebugProgressResetHotkey : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            SaveManager.ResetCompletedLevels();

            Debug.Log("Progress reset by F2");

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}