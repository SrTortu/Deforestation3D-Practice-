using UnityEngine;
using UnityEngine.SceneManagement;

namespace Deforestation
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                // Si la instancia ya existe, la devolvemos.
                if (_instance != null)
                {
                    return _instance;
                }

                // Si no existe, la buscamos en la escena.
                _instance = FindObjectOfType<T>();
             
                // Si aún es null, significa que no hay un objeto en la escena.
                // En lugar de crear uno, simplemente devolvemos null y generamos un error
                if (_instance == null)
                {
                    Debug.LogError("No se encontró una instancia de " + typeof(T).Name + " en la escena.");
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}