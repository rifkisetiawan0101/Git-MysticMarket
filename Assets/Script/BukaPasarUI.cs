using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BukaPasarUI : MonoBehaviour
{
    [SerializeField] private Button buttonMenujuMalam;
    [SerializeField] private TimerPagi timerPagi; // Referensi ke TimerPagi
    [SerializeField] private Transform returnPosition;

    private void Start() {
        buttonMenujuMalam.onClick.AddListener(OnMenujuMalamClicked);
    }

    public void OnMenujuMalamClicked() {
        // GameObject mobilBak = timerPagi.GetMobilBakInstance();
        // if (mobilBak != null) {
        //     MobilBakController controller = mobilBak.GetComponent<MobilBakController>();
        //     if (controller != null) {
        //         controller.targetPosition = returnPosition; // Set targetPosition ke posisi pulang
        //     }
        // }
        
        Time.timeScale = 1;
        SceneManager.LoadScene("InGame");
        gameObject.SetActive(false);
        // StartCoroutine(LoadSceneWithDelay());
    }

    public void ShowBukaPasarUI() {
        gameObject.SetActive(true);
    }

    // private IEnumerator LoadSceneWithDelay() {
    //     // Wait for a while before changing scene
    //     yield return new WaitForSeconds(1f); // Delay sebelum load scene

    //     Time.timeScale = 1;
    //     SceneManager.LoadScene("InGame");
    //     gameObject.SetActive(false);
    // }
}