using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SushiType
{
    EYES, TEETH, SQUID, TENTACLES
}

[System.Serializable]
public struct Wave
{
    public SushiType sushiType;
    public int sushiCount;
    public int secondsBeforeSpawning;
    public float timeBetweenSpawns;
    public int hitpoints;
}

public class MapSettings : MonoBehaviour
{
    public Transform hordeSpawnA;
    public Transform hordeSpawnB;
    public Wave[] waves;
}
