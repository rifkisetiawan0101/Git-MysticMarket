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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        _oldPosition = rb.position;

        if (merchantManager != null) {
            SetRandomTarget();
            StartCoroutine(MoveToTarget());
        } else {
            Debug.LogWarning("MerchantManager belum di-set.");
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

    private IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector2 direction = (targetPosition - transform.position).normalized;
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, targetPosition, ref velocity, smoothTime, speed * Time.deltaTime);
            rb.MovePosition(targetPos);

            Vector2 movement = (rb.position - _oldPosition).normalized;
            _oldPosition = rb.position;

            animator.SetFloat(_horizontal, movement.x);
            animator.SetFloat(_vertical, movement.y);

            if (movement != Vector2.zero)
            {
                animator.SetFloat(_lastHorizontal, movement.x);
                animator.SetFloat(_lastVertical, movement.y);
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            yield return null;
        }

        StartCoroutine(PauseAtTarget());
    }

    private IEnumerator MoveToTarget2()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector2 direction = (targetPosition - transform.position).normalized;
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, targetPosition, ref velocity, smoothTime, speed * Time.deltaTime);
            rb.MovePosition(targetPos);

            Vector2 movement = (rb.position - _oldPosition).normalized;
            _oldPosition = rb.position;

            animator.SetFloat(_horizontal, movement.x);
            animator.SetFloat(_vertical, movement.y);

            if (movement != Vector2.zero)
            {
                animator.SetFloat(_lastHorizontal, movement.x);
                animator.SetFloat(_lastVertical, movement.y);
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            yield return null;
        }

        StartCoroutine(PauseAtTarget2());
    }

    private IEnumerator PauseAtTarget()
    {
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(pauseTime);

        StartCoroutine(MoveRandomly());
    }

    private IEnumerator PauseAtTarget2()
    {
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(pauseTime);

        ReturnToSpawn();
    }

    private IEnumerator PauseAtRandom() {
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(pauseTime);

        SetRandomTarget();
        StartCoroutine(MoveToTarget2());

    }

    private IEnumerator MoveRandomly()
    {
        Vector3 randomDirection = Random.insideUnitCircle.normalized * Random.Range(500f, 1000f);
        Vector3 randomTarget = transform.position + randomDirection;

        float timer = 0f;
        while (timer < randomMoveTime)
        {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, randomTarget, ref velocity, smoothTime, speed * Time.deltaTime);
            rb.MovePosition(targetPos);

            timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(PauseAtRandom());
    }

    private void ReturnToSpawn()
    {
        StartCoroutine(MoveToSpawn());
    }

    private IEnumerator MoveToSpawn()
    {
        while (Vector3.Distance(transform.position, spawnPosition) > 0.1f)
        {
            Vector2 targetPos = Vector2.SmoothDamp(rb.position, spawnPosition, ref velocity, smoothTime, speed * Time.deltaTime);
            rb.MovePosition(targetPos);

            yield return null;
        }

        Destroy(gameObject); // Menghancurkan NPC setelah kembali ke posisi awal
    }

    private void OnDestroy()
    {
        // Bersihkan jika diperlukan
    }
}
