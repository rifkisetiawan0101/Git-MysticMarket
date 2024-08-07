using UnityEngine;
using Cinemachine;

public class VirtualCameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float startSize = 1185f;
    public float targetSize = 760f;
    public float smoothTime = 1f;
    
    private float velocity = 0f;

    void Start()
    {
        if (virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
        
        virtualCamera.m_Lens.OrthographicSize = startSize;
    }

    void Update()
    {
        float newSize = Mathf.SmoothDamp(virtualCamera.m_Lens.OrthographicSize, targetSize, ref velocity, smoothTime);
        virtualCamera.m_Lens.OrthographicSize = newSize;
    }
}
