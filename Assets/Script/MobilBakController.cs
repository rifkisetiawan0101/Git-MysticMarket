using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MobilBakController : MonoBehaviour {
    public Transform targetPosition;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Button buttonOpenRestok;
    [SerializeField] private GameObject windowRestok;
    [SerializeField] private GameObject overlay;

    private Vector3 velocity = Vector3.zero;

    private void Start() {
        buttonOpenRestok.onClick.AddListener(() => {
            windowRestok.SetActive(true);
            overlay.SetActive(false);

            FindObjectOfType<PlayerMovementNew>().StopPlayer();
        });
        
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget() {
        while (Vector3.Distance(transform.position, targetPosition.position) > 0.1f) {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition.position, ref velocity, smoothTime, speed, Time.deltaTime);
            yield return null;
        }
    }
}