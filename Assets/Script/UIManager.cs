using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{   
    public static UIManager Instance;
    [SerializeField] private GameObject BlackFadeOut;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject invoiceUI;
    [SerializeField] private GameObject timerUI;
    [SerializeField] private GameObject blokirUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        BlackFadeOut.SetActive(true);
    }

    private void Start()
    {   
        // Set visibilitas image(2) sesuai dengan scene saat ini
        UpdateImageVisibility();

        StartCoroutine(DisableOutroOverlay(1.1f));
    }

    private IEnumerator DisableOutroOverlay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        BlackFadeOut.SetActive(false);
    }


    ///---------------------


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update visibilitas image(2) ketika scene baru dimuat
        UpdateImageVisibility();
    }

    private void UpdateImageVisibility()
    {
        if (SceneManager.GetActiveScene().name == "InGamePagi")
        {
            shopUI.SetActive(false);
            invoiceUI.SetActive(false);
            timerUI.SetActive(false);
            Timer.elapsedTime = 0;
            PersistentManager.Instance.isInvoiceShown = false;
        }
        else
        {
            shopUI.SetActive(true);
            timerUI.SetActive(true);
        }
    }

    public void DeactivateUI() {
        blokirUI.SetActive(true);
        PersistentManager.Instance.isActivateUI = false;
    }

    public void ActivateUI() {
        blokirUI.SetActive(false);
        PersistentManager.Instance.isActivateUI = true;
    }
}
