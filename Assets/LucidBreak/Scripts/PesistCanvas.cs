using UnityEngine;

public class PersistentCanvas : MonoBehaviour
{
    private static PersistentCanvas instance;

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
}
