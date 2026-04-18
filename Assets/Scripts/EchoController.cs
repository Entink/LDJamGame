using UnityEngine;
using System.Collections.Generic;

public class EchoController : MonoBehaviour
{
    public static EchoController instance;

    private readonly List<EchoData> activeEchoes = new();

    public IReadOnlyList<EchoData> ActiveEchoes => activeEchoes;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void CreateEcho(Vector2 origin, float speed, float maxRadius, float thickness)
    {
        EchoData echo = new EchoData(origin, speed, Time.time, maxRadius, thickness);
        activeEchoes.Add(echo);
    }
}
