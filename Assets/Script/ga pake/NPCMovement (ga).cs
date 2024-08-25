using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Transform waypointGroup; // Referensi ke grup waypoint

    private List<Transform> waypoints;
    private int _currentWaypointIndex = 0;
    private Vector2 _movement;
    private Vector2 _oldPosition;

    private Rigidbody2D _rb;
    private Animator _animator;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";
    private const string _speed = "Speed";

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _oldPosition = transform.position;

        // Mendapatkan semua waypoint dari grup
        waypoints = new List<Transform>();
        foreach (Transform waypoint in waypointGroup)
        {
            waypoints.Add(waypoint);
        }
    }

    private void Update()
    {
        MoveToWaypoint();

        Vector2 newPosition = transform.position;
        _movement = (newPosition - _oldPosition).normalized;
        _oldPosition = newPosition;

        _rb.velocity = _movement * _moveSpeed;
        _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_vertical, _movement.y);

        if (_movement != Vector2.zero)
        {
            _animator.SetFloat(_lastHorizontal, _movement.x);
            _animator.SetFloat(_lastVertical, _movement.y);
        }
        else
        {
            SetIdleAnimation();
        }
    }

    private void MoveToWaypoint()
    {
        if (waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[_currentWaypointIndex];
        Vector2 direction = (targetWaypoint.position - transform.position).normalized;
        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetWaypoint.position, _moveSpeed * Time.deltaTime);
        transform.position = newPosition;

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            _currentWaypointIndex++;
            if (_currentWaypointIndex >= waypoints.Count)
            {
                StartCoroutine(DestroyAfterDelay(0f)); // Menunggu 1 detik sebelum menghancurkan objek
            }
            else
            {
                _currentWaypointIndex %= waypoints.Count;
            }
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void SetIdleAnimation()
    {
        if (_movement.x > 0)
        {
            _animator.Play("IdleRight");
        }
        else if (_movement.x < 0)
        {
            _animator.Play("IdleLeft");
        }
        else if (_movement.y > 0)
        {
            _animator.Play("IdleUp");
        }
        else if (_movement.y < 0)
        {
            _animator.Play("IdleDown");
        }
    }
}
