using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupBeamScript : MonoBehaviour
{
    public GameObject ketchupPrefab;
    GameData gameData;
    public Vector3 ketchupSpread;
    //public Vector3 ketchupPuddle;
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

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (playerScript.controller == null && Input.GetButton("Fire1")) Fire(); // Fire with mouse click when in keyboard/mouse control
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
            //ketchup.AddComponent<KetchupScript>();
            ketchup.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        }
    }

    void Update()
    {
        
    }
}
