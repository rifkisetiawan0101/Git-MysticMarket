using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashArtButton : MonoBehaviour
{
    public string sceneName = "InGame";
    private float delayTime = 5.3f;

    private void Start()
    {
        StartCoroutine(EnableButtonAfterDelay());
    }

    private IEnumerator EnableButtonAfterDelay()
    {
        // Tunggu selama 5.3 detik
        yield return new WaitForSeconds(delayTime);

        // Button btn = GetComponent<Button>(); pake ini kalo kodenya berada didalem objek button itu sendiri
        GameObject mulai = GameObject.Find("MulaiButton");
        Button mulaiButton = mulai.GetComponent<Button>();

        GameObject pengaturan = GameObject.Find("PengaturanButton");
        Button pengaturanButton = pengaturan.GetComponent<Button>();

        GameObject exit = GameObject.Find("ExitButton");
        Button exitButton = exit.GetComponent<Button>();

        if (mulaiButton != null)
        {
            mulaiButton.onClick.AddListener(LoadScene);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void ExitGame()
    {
        // Jika sedang dalam editor, gunakan ini
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Jika dalam build, gunakan ini
        Application.Quit();
        #endif  
    }
}
