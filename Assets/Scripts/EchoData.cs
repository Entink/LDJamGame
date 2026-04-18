using UnityEngine;

[System.Serializable]
public class EchoData
{
    public Vector2 origin;
    public Vector2 direction;
    public float speed;
    public float startTime;
    public float maxRadius;
    public float thickness;

    public EchoData(Vector2 origin, float speed, float startTime, float maxRadius, float thickness)
    {
        this.origin = origin;
        this.speed = speed;
        this.startTime = startTime;
        this.maxRadius = maxRadius;
        this.thickness = thickness;
    }

}