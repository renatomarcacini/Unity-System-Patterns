using UnityEngine;

public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this as T;
    }

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}


public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        base.Awake();
    }
}

public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}


public abstract class SingletonAutoInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (Instantiate(Resources.Load(typeof(T).Name)) as GameObject).GetComponent<T>();
            }
            return instance;
        }
    }
}

public abstract class PersistentSingletonAutoInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (Instantiate(Resources.Load(typeof(T).Name)) as GameObject).GetComponent<T>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
}






