
using UnityEngine;
using UnityEngine.Events;

public class UISignals : MonoBehaviour
{
    public static UISignals instance;

    public UnityAction<byte> onSetStageColor = delegate { };
    public UnityAction<byte> onSetLevelValue = delegate { };
    public UnityAction onPlay = delegate { };


    private void Awake()
    {

        if(instance !=null && instance !=this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
