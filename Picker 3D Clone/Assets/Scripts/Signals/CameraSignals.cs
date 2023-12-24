
using UnityEngine;
using UnityEngine.Events;

public class CameraSignals : MonoBehaviour
{
    public static CameraSignals instance;

    public UnityAction onSetCameraTarger =delegate { }; 

    private void Awake()
    {
        if (instance != null && instance != this)
        {

            Destroy(gameObject);
            return;
        }

        instance = this;
    }

}
