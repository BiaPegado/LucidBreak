using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private static Player instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transform.position = SceneTransition.playerNextPosition;
        transform.rotation = SceneTransition.playerNextRotation;
    }
}
