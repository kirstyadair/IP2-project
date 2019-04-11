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

    // called in order of player creation
    public void CreateHealthbar(PlayerScript player)
    {
        GameObject healthbarPrefab;

        // pick the appropriate prefab variant for the health bar
        switch (player.chefType)
        {
            case PlayerType.CRAZY:
                healthbarPrefab = crazyHealthPrefab;
                break;
            case PlayerType.FAT:
                healthbarPrefab = fatHealthPrefab;
                break;
            case PlayerType.THIN:
                healthbarPrefab = thinHealthPrefab;
                break;
            default:
                healthbarPrefab = crazyHealthPrefab;
                break;
        }


        // place & initialize each health bar in position
        GameObject healthbar = Instantiate(healthbarPrefab, this.transform);
        healthbar.transform.localPosition = new Vector3(0, currentY, 0);
        currentY -= distanceBetweenBars;

        healthbar.GetComponent<HealthbarScript>().Initialize(player);
    }
}
