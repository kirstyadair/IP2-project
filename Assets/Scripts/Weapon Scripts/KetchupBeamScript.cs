using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupBeamScript : MonoBehaviour
{
    public GameObject ketchupPrefab;
    GameObject ketchup;
    GameData gameData;
    AudioSource audioSource;
    public Vector3 ketchupSpread;
    Vector3 direction;
    public PlayerScript playerScript;
    bool hasMovedUp = false;
    bool hasMovedDown = false;
    bool hasMovedLeft = false;
    bool hasMovedRight = false;
    bool ketchupActive = false;
    float ketchupTimeToDestroy = 3.0f;

    private void Awake()
    {
        playerScript.OnFire += Fire;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        audioSource = GetComponent<AudioSource>();
        ketchupActive = false;
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
        ketchupActive = true;
        // Don't fire before play has started
        if (gameData.state != GameState.PREP)
        {
            // Play the ketchup sound effect 
            if (!audioSource.isPlaying) audioSource.Play();

            // Find the direction of firing
            direction = playerScript.reticlePos - transform.position;

            // Limit how far the ketchup can travel on each axis
            if (direction.x >= 2) direction.x = 2;
            if (direction.y >= 2) direction.y = 2;
            if (direction.z >= 2) direction.z = 2;
            direction.Normalize();

            // Instantiate the ketchup blob
            ketchup = (GameObject)Instantiate(ketchupPrefab, transform.position, Quaternion.identity);
            ketchup.tag = "KetchupBlob";
            ketchup.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
            StartCoroutine(StartKetchupDestroy(ketchup));
        }
    }

    // This is applied to every ketchup blob as soon as it is instantiated
    IEnumerator StartKetchupDestroy(GameObject ketchup)
    {
        // If the blob does not hit the floor or a wall, it has three seconds before it is destroyed
        float timeToDestroy = 3.0f;
        do
        {
            // Reduce the time left until the blob is destroyed
            timeToDestroy -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            // Don't try to change the scale of the ketchup blob if it has already been destroyed by a wall or floor
            if (ketchup.gameObject != null)
            {
                // ketchupSpread is a public variable with its value assigned in the inspector
                ketchup.transform.localScale += ketchupSpread;
            }
        } while (timeToDestroy > 0);

        // Once the three seconds are done, destroy the blob
        Destroy(ketchup);
    }
}
