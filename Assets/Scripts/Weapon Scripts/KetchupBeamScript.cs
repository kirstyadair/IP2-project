using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupBeamScript : MonoBehaviour
{
    public GameObject ketchupPrefab;
    GameData gameData;
    public Vector3 ketchupSpread;
    public Vector3 spawnPos;
    public PlayerScript playerScript;

    private void Awake()
    {
        playerScript.OnFire += Fire;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }

    void Update()
    {
        if (playerScript.up)
        {
            spawnPos = new Vector3(0, 0, 0.5f);
        }
    }

    public void Fire()
    {
        if (gameData.state != GameState.PREP)
        {
            Vector3 direction = playerScript.reticlePos - transform.position;

            if (direction.x >= 2) direction.x = 2;
            if (direction.y >= 2) direction.y = 2;
            if (direction.z >= 2) direction.z = 2;

            direction.Normalize();

            GameObject ketchup = (GameObject)Instantiate(ketchupPrefab, spawnPos, Quaternion.identity);
            ketchup.GetComponent<KetchupScript>().ketchupBeamScript = this;
            ketchup.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        }
    }
}
