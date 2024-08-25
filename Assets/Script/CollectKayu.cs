using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class CollectKayu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField] private GameObject kayuPrefab;
    [SerializeField] private float holdTime = 1f;
    [SerializeField] private Button buttonTapHold;
    [SerializeField] private GameObject tapHold;
    private Animator tapHoldAnimator;
    public static float holdTimer = 0f;
    private bool isHolding = false;
    private bool hasCollected = false;

    [SerializeField] private GameObject collectFeedback;
    [SerializeField] private Image collectFeedbackImage;
    [SerializeField] private Sprite[] collectFrames;
    private float collectDelay = 0.055f;

    private void Start() {
        tapHold.SetActive(false);
        tapHoldAnimator = tapHold.GetComponent<Animator>();    
    }

    private void Update() {
        if (isHolding && ColTriggerKayu.isPlayerInRange && !hasCollected) {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdTime) {
                hasCollected = true;
                tapHold.SetActive(false);
                PersistentManager.Instance.UpdateCollectable(1, "Kayu");
                StartCoroutine(PlayCollectKayu());
            }
        }
    }

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;

    public void OnPointerDown(PointerEventData eventData) {
        if (ColTriggerKayu.isPlayerInRange) {
            isHolding = true;  // Mulai hold
            buttonTapHold.image.sprite = highlightedSprite;
            tapHold.SetActive(true);
            tapHoldAnimator.Play("TapHoldAnim");

            FindObjectOfType<PlayerMovementNew>().StopPlayer();
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        isHolding = false;  // Berhenti hold
        holdTimer = 0f;
        buttonTapHold.image.sprite = normalSprite;
        tapHold.SetActive(false);
    }

    private IEnumerator PlayCollectKayu() {
        collectFeedback.SetActive(true);
        for (int i = 0; i < collectFrames.Length; i++) {
            collectFeedbackImage.sprite = collectFrames[i];
            yield return new WaitForSeconds(collectDelay);
        }
        collectFeedback.SetActive(false);
        Destroy(kayuPrefab);
    }
}
