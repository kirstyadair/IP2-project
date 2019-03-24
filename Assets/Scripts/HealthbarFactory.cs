using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarFactory : MonoBehaviour
{
    public GameObject fatHealthPrefab;
    public GameObject crazyHealthPrefab;
    public GameObject thinHealthPrefab;

    float currentY = 0;
    public float distanceBetweenBars;

    public void CreateHealthbar(PlayerScript player)
    {
        Debug.Log("current y: " + currentY);
        GameObject prefab;

        switch (player.chefType)
        {
            case PlayerType.CRAZY:
                prefab = crazyHealthPrefab;
                break;
            case PlayerType.FAT:
                prefab = fatHealthPrefab;
                break;
            case PlayerType.THIN:
                prefab = thinHealthPrefab;
                break;
            default:
                prefab = crazyHealthPrefab;
                break;
        }

        GameObject healthbar = Instantiate(prefab, this.transform);
        healthbar.transform.localPosition = new Vector3(0, currentY, 0);
        currentY -= distanceBetweenBars;

        healthbar.GetComponent<HealthbarScript>().Initialize(player);
    }
}
