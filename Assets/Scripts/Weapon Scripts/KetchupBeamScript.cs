using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupBeamScript : MonoBehaviour
{
    public GameObject ketchupPrefab;
    Vector3 direction = new Vector3(0, 0, 1);
    GameData gameData;
    public Vector3 ketchupSpread;
    public Vector3 ketchupPuddle;

    // Start is called before the first frame update
    void Start()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameData.state != GameState.PREP)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                GameObject ketchup = (GameObject)Instantiate(ketchupPrefab, transform.position, Quaternion.identity);
                ketchup.AddComponent<KetchupScript>();
                ketchup.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
            }
        }
    }
}
