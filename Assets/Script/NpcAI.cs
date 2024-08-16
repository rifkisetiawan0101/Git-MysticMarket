using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAI : MonoBehaviour
{
    public float speed = 200f;
    public float smoothTime = 0.1f; 
    public float pauseTime = 3f; // Waktu berhenti di target
    public float randomMoveTime = 5f; // Waktu untuk gerakan acak

    private MerchantManager merchantManager; 
    private Vector3 targetPosition;
    private Vector3 spawnPosition; // Posisi awal spawn NPC

    Rigidbody2D rb;
    Animator animator;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

    private Vector2 velocity;
    private Vector2 _oldPosition;

    public void SetupNPC(MerchantManager manager) {
        // Inisialisasi MerchantManager dan posisi spawn
        merchantManager = manager;
        spawnPosition = transform.position;
        SetRandomTarget();
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        _oldPosition = rb.position;
    }
    private void Start()
    {
        if (merchantManager != null) {
            SetRandomTarget();
            StartCoroutine(MoveToTarget());
        }
    }

    private void SetRandomTarget()
    {
        if (merchantManager != null && merchantManager.targetMerchantNPCList.Count > 0)
        {
            int randomIndex = Random.Range(0, merchantManager.targetMerchantNPCList.Count);
            targetPosition = merchantManager.targetMerchantNPCList[randomIndex];
        }
    }

    private void NpcAnimation() {
        Vector2 newPosition = transform.position;
        Vector2 movement = (newPosition - _oldPosition).normalized;
        _oldPosition = newPosition;

        rb.velocity = movement * speed;

        float horizontalValue = Mathf.Clamp(movement.x, -1f, 1f);
        float verticalValue = Mathf.Clamp(movement.y, -1f, 1f);

        // Set animator parameters
        animator.SetFloat(_horizontal, horizontalValue);
        animator.SetFloat(_vertical, verticalValue);

        if (movement != Vector2.zero) {
            if (movement.x > 0) {
                animator.SetFloat(_horizontal, 1);
                animator.SetFloat(_vertical, 0);
            }
            else if (movement.x < 0) {
                animator.SetFloat(_horizontal, -1);
                animator.SetFloat(_vertical, 0);
            }
            else if (movement.y > 0) {
                animator.SetFloat(_horizontal, 0);
                animator.SetFloat(_vertical, 1);
            }
            else if (movement.y < 0) {
                animator.SetFloat(_horizontal, 0);
                animator.SetFloat(_vertical, -1);
            }
        }
    }

    private IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, targetPosition, ref velocity, smoothTime, speed * Time.deltaTime);
            rb.MovePosition(targetPos);

            NpcAnimation();

            yield return null;
        }

        StartCoroutine(PauseAtTarget());
    }

    private IEnumerator MoveToTarget2()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, targetPosition, ref velocity, smoothTime, speed * Time.deltaTime);
            rb.MovePosition(targetPos);

            NpcAnimation();

            yield return null;
        }

        StartCoroutine(PauseAtTarget2());
    }

    private IEnumerator PauseAtTarget()
    {
        yield return new WaitForSeconds(pauseTime);

        StartCoroutine(MoveRandomly());
    }

    private IEnumerator PauseAtTarget2()
    {
        yield return new WaitForSeconds(pauseTime);

        StartCoroutine(MoveToSpawn());
    }

    private IEnumerator PauseAtRandom() {
        yield return new WaitForSeconds(pauseTime);

        SetRandomTarget();
        StartCoroutine(MoveToTarget2());
    }

    private IEnumerator MoveRandomly()
    {
        Vector3 randomDirection = Random.insideUnitCircle.normalized * Random.Range(1000f, 1500f);
        Vector3 randomTarget = transform.position + randomDirection;

        float timer = 0f;
        while (timer < randomMoveTime)
        {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, randomTarget, ref velocity, smoothTime, speed * Time.deltaTime);
            rb.MovePosition(targetPos);

            NpcAnimation();

            timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(PauseAtRandom());
    }

    private IEnumerator MoveToSpawn()
    {
        while (Vector3.Distance(transform.position, spawnPosition) > 0.1f)
        {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, spawnPosition, ref velocity, smoothTime, speed * Time.deltaTime);
            rb.MovePosition(targetPos);

            NpcAnimation();

            yield return null;
        }

        Destroy(gameObject); // Menghancurkan NPC setelah kembali ke posisi awal
    }

    private void OnDestroy()
    {
        // Bersihkan jika diperlukan
    }
}
