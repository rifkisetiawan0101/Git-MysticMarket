using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MobilBakController : MonoBehaviour {
    public Transform targetPosition;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Canvas restokCanvas; // Referensi ke Canvas Restok
    [SerializeField] private Button buttonRestok; // Referensi ke Button Restok
    [SerializeField] private GameObject restokUI; // Referensi ke RestokUI
    [SerializeField] private Button buttonCloseRestok; // Referensi ke Button Close Restok

    private Vector3 velocity = Vector3.zero;

    private void Start() {
        buttonRestok.onClick.AddListener(OpenRestokUI);
        buttonCloseRestok.onClick.AddListener(CloseRestokUI);
        
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget() {
        while (Vector3.Distance(transform.position, targetPosition.position) > 0.1f) {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition.position, ref velocity, smoothTime, speed, Time.deltaTime);
            yield return null;
        }
    }

    private void OpenRestokUI() {
        restokUI.SetActive(true);
    }

    private void CloseRestokUI() {
        restokUI.SetActive(false);
    }
}