using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SplashArtButton : MonoBehaviour
{
    public string sceneName = "FirstCutScene";
    private float delayTime = 5.3f;
    private Button mulaiButton;
    [SerializeField] private GameObject blackFadeIn;

    private void Start()
    {
        StartCoroutine(EnableButtonAfterDelay());
    }

    private IEnumerator EnableButtonAfterDelay()
    {
        // Tunggu selama 5.3 detik
        yield return new WaitForSeconds(delayTime);

        // Button btn = GetComponent<Button>(); pake ini kalo kodenya berada didalem objek button itu sendiri
        mulaiButton = GameObject.Find("MulaiButton").GetComponent<Button>();

        GameObject pengaturan = GameObject.Find("PengaturanButton");
        Button pengaturanButton = pengaturan.GetComponent<Button>();

        GameObject exit = GameObject.Find("ExitButton");
        Button exitButton = exit.GetComponent<Button>();

        if (mulaiButton != null)
        {
            mulaiButton.onClick.AddListener(OnMulaiButtonClick);

            // Menambahkan event trigger untuk hover
            AddEventTrigger(mulaiButton.gameObject, EventTriggerType.PointerEnter, eventData => OnPointerEnter(eventData));
        }

        if (pengaturanButton != null)
        {
            pengaturanButton.onClick.AddListener(OnPengaturanButtonClick);
            AddEventTrigger(pengaturanButton.gameObject, EventTriggerType.PointerEnter, eventData => OnPointerEnter(eventData));
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
            AddEventTrigger(exitButton.gameObject, EventTriggerType.PointerEnter, eventData => OnPointerEnter(eventData));
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadSceneAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        LoadScene();
    }

    private void ActivateIntroOverlay()
    {
        blackFadeIn.SetActive(true);
    }

    private void OnMulaiButtonClick()
    {   
        ActivateIntroOverlay();
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick);
        AudioManager.audioManager.FadeOutMusic(0.5f); // Durasi fade out 0.5 detik
        StartCoroutine(LoadSceneAfterDelay(0.5f));
    }

    private void OnPengaturanButtonClick()
    {
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonClick); 
    }

    // Implementasi IPointerEnterHandler untuk menangani event hover
    private void OnPointerEnter(BaseEventData eventData)
    {
        Debug.Log("Pointer masuk ke MulaiButton.");
        AudioManager.audioManager.PlaySFX(AudioManager.audioManager.buttonHover);
    }

    // Implementasi IPointerExitHandler untuk menangani saat pointer keluar dari button
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter == mulaiButton.gameObject)
        {
            // Contoh: kembalikan efek visual ke keadaan semula
        }
    }

    // event trigger
    private void AddEventTrigger(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>() ?? obj.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
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
