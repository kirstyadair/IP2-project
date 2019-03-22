using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupBeamScript : MonoBehaviour
{
    public GameObject ketchupPrefab;
    GameData gameData;
    public Vector3 ketchupSpread;
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
        if (playerScript.firingUp)
        {
            Debug.Log("up");
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 0.5f);
        }

        if (playerScript.firingDown)
        {
            Debug.Log("down");
        }

        if (playerScript.firingLeft)
        {
            Debug.Log("left");
        }

        if (playerScript.firingRight)
        {
            Debug.Log("right");
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

            GameObject ketchup = (GameObject)Instantiate(ketchupPrefab, transform.position, Quaternion.identity);
            ketchup.GetComponent<KetchupScript>().ketchupBeamScript = this;
            ketchup.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        }
    }
}
