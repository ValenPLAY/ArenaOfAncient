using UnityEngine;

public class SingletonAuto<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    var singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + "- SingletonAuto";

                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }
}
