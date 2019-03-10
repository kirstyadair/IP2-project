using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnivesScript : MonoBehaviour
{
    public GameObject knifePrefab;
    public float fireCooldown = 0.5f;
    float fireTimeout = 0;
    public GameObject reticle;
    GameData gameData;
    PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        playerScript.OnFire += Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimeout -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (fireTimeout > 0) return;
        if (gameData.state == GameState.PREP) return; // Don't shoot whilst we're preparing
        Vector3 bulletDirection = playerScript.reticlePos - transform.position;
        bulletDirection.Normalize();
        bulletDirection.y = 0;

        GameObject knife = (GameObject)Instantiate(knifePrefab, transform.position, Quaternion.identity);

        knife.GetComponent<Rigidbody>().AddForce(bulletDirection * 2, ForceMode.Impulse);
        knife.transform.up = bulletDirection;
        fireTimeout = fireCooldown;
    }


}
