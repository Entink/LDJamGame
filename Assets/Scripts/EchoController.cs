using UnityEngine;

public class EchoController : MonoBehaviour
{
    public static EchoController instance;

    publi

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Update()
    {
        
    }

    public void CreateEcho(Vector2 origin, float speed, float maxRadius, float thickness)
    {
        EchoData echo = new EchoData(origin, speed, Time.time, maxRadius, thickness);

    }
}
