using UnityEngine;

namespace MO.Utils
{
    public abstract class Singleton : MonoBehaviour
    {
        public static bool Quitting { get; protected set; }
        private void OnApplicationQuit()
        {
            Quitting = true;
        }
    }

    public class Singleton<T, T2> : Singleton<T> where T : MonoBehaviour, T2
    {
        public new static T2 Instance
        {
            get
            {
                if (Quitting)
                {
                    return default(T2);
                }

                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    DontDestroyOnLoad(instance.gameObject);
                }

                return instance;
            }
        }
    }


    public class DestructibleSingleton<T> : Singleton where T : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        protected static T instance;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {

                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        var newGO = new GameObject(typeof(T).Name, typeof(T));
                        instance = newGO.GetComponent<T>();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Awake the singleton -> add instance to the same gameobject than this class
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = Instance;
            }
            else
            {
                // We have already an instance, we destroy this one
                if (this != instance)
                    Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (this == instance)
            {
                Quitting = true;
            }
        }
    }

    public class Singleton<T> : Singleton where T : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        protected static T instance;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (Quitting)
                {
                    return null;
                }

                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        var newGO = new GameObject(typeof(T).Name, typeof(T));
                        instance = newGO.GetComponent<T>();
                    }

                    DontDestroyOnLoad(instance.gameObject);
                }

                return instance;
            }
        }

        /// <summary>
        /// Awake the singleton -> add instance to the same gameobject than this class
        /// </summary>
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = Instance;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                // We have already an instance, we destroy this one
                if (this != instance)
                    Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (this == instance)
            {
                Quitting = true;
            }
        }
    }
}