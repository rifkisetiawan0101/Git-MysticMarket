using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcUtoAI : MonoBehaviour {
    public float speed = 40000f;
    public float smoothTime = 0.1f; 
    public float idleTime = 3f; // Waktu berhenti di target

    private MerchantManager merchantManager; 
    private Vector3 targetPosition;
    private Vector3 spawnPosition;

    Rigidbody2D rb;
    Animator animator;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

    private Vector2 velocity;
    private Vector2 oldPosition;

    public void SetupUto(MerchantManager manager) {
        // Inisialisasi MerchantManager dan posisi spawn
        merchantManager = manager;
        spawnPosition = transform.position;
        SetRandomTarget();
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        oldPosition = rb.position;
    }
    
    private void Start() {
        if (merchantManager != null) {
            SetRandomTarget();
            StartCoroutine(MoveToTarget());
        }
    }

    private void Update() {
        NpcAnimation();
    }

    private void SetRandomTarget() {
        if (PersistentManager.Instance.dataMerchantList.Count > 0) {
            int randomIndex = Random.Range(0, PersistentManager.Instance.dataMerchantList.Count);
            targetPosition = PersistentManager.Instance.dataMerchantList[randomIndex].merchantPosition;
        }
    }

    private void NpcAnimation() {
        Vector2 newPosition = transform.position;
        Vector2 movement = (newPosition - oldPosition).normalized;
        oldPosition = newPosition;

        if (movement != Vector2.zero) {
            animator.SetFloat(_horizontal, movement.x);
            animator.SetFloat(_vertical, movement.y);
            animator.SetFloat(_lastHorizontal, movement.x);
            animator.SetFloat(_lastVertical, movement.y);
        } else {
            animator.SetFloat(_horizontal, 0);
            animator.SetFloat(_vertical, 0);
        }
    }

    private IEnumerator MoveToTarget() {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, targetPosition, ref velocity, smoothTime, speed * Time.deltaTime);
            rb.MovePosition(targetPos);

            NpcAnimation();

            yield return null;
        }

        StartCoroutine(idleAtTarget());
    }

    private IEnumerator idleAtTarget() {
        yield return new WaitForSeconds(idleTime);
        StartCoroutine(MoveToTarget());
    }
}
