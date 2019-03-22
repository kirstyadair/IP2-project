using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupBeamScript : MonoBehaviour
{
    public GameObject ketchupPrefab;
    GameData gameData;
    public Vector3 ketchupSpread;
    public PlayerScript playerScript;
    bool hasMovedUp = false;
    bool hasMovedDown = false;
    bool hasMovedLeft = false;
    bool hasMovedRight = false;

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
            if (!hasMovedUp)
            {
                transform.localPosition = new Vector3(0, 0, 0);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 0.5f);
                hasMovedUp = true;
                hasMovedDown = false;
                hasMovedLeft = false;
                hasMovedRight = false;
            }
        }

        if (playerScript.firingDown)
        {
            if (!hasMovedDown)
            {
                transform.localPosition = new Vector3(0, 0, 0);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.75f);
                hasMovedUp = false;
                hasMovedDown = true;
                hasMovedLeft = false;
                hasMovedRight = false;
            }
        }

        if (playerScript.firingLeft)
        {
            if (!hasMovedLeft)
            {
                transform.localPosition = new Vector3(0, 0, 0);
                transform.localPosition = new Vector3(transform.localPosition.x - 0.5f, transform.localPosition.y, transform.localPosition.z - 0.3f);
                hasMovedUp = false;
                hasMovedDown = false;
                hasMovedLeft = true;
                hasMovedRight = false;
            }
        }

        if (playerScript.firingRight)
        {
            if (!hasMovedRight)
            {
                transform.localPosition = new Vector3(0, 0, 0);
                transform.localPosition = new Vector3(transform.localPosition.x + 0.5f, transform.localPosition.y, transform.localPosition.z - 0.3f);
                hasMovedUp = false;
                hasMovedDown = false;
                hasMovedLeft = false;
                hasMovedRight = true;
            }
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
