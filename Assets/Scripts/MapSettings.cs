using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SushiType
{
    EYES, TENTACLES
}

[System.Serializable]
public struct Wave
{
    public SushiType sushiType;
    public int sushiCount;
    public int secondsBeforeSpawning;
    public float timeBetweenSpawns;
}

public class MapSettings : MonoBehaviour
{
    public float cameraZoomPrepMode;
    public float cameraZoomPlayMode;
    public Transform centerPoint;

    public Wave[] waves;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
