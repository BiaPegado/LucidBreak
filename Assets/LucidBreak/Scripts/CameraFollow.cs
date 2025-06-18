using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Vector3 camOffset;
    private static CameraFollow instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    void Start()
    {
        if (target != null)
        {
            camOffset = transform.position - target.position;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + camOffset;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                SetTarget(player.transform);
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        camOffset = transform.position - target.position;
    }
}
