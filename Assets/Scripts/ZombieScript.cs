using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public bool dead = false;
    public SpriteRenderer sprt;
    public SpriteRenderer defBubble;
    public SpriteRenderer offBubble;
    HordeScript hordeScript;
    GameData gameData;
    public PlayerStats stats;
    public bool isAttachedToKing = false;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Animator>().speed = UnityEngine.Random.Range(0.5f, 1.5f);
        hordeScript = GameObject.Find("Horde").GetComponent<HordeScript>();
        gameData = GameObject.Find("GameData").GetComponent<GameData>();

        // store the playerstats for the fat chef if it's in the game
        GameObject fatChef = GameObject.Find("FAT player");
        if (fatChef != null) stats = fatChef.GetComponent<PlayerScript>().stats;
    }

    // Update is called once per frame
    public virtual void Update ()
    {
        // make it more red the deader it is
        Color clr = sprt.color;
        clr.g = health / maxHealth;
        clr.b = health / maxHealth;
        sprt.color = clr;

        if (dead) return;

        if (hordeScript.state == HordeState.DEFENSIVE)
        {
            offBubble.gameObject.SetActive(false);
            defBubble.gameObject.SetActive(true);
        }

        if (hordeScript.state == HordeState.OFFENSIVE)
        {
            offBubble.gameObject.SetActive(true);
            defBubble.gameObject.SetActive(false);
        }

        if (hordeScript.state == HordeState.NEUTRAL)
        {
            offBubble.gameObject.SetActive(false);
            defBubble.gameObject.SetActive(false);
        }

        if (health < maxHealth) health += hordeScript.regenerationSpeed;
    }

    public virtual bool Hit(float hp)
    {
        if (dead) return false;
        if (this == null) return false;

        health -= hp * hordeScript.defensiveStat;
        if (health <= 0) {
            Kill();
            return true;
        }

        return false;
    }

    public void Kill()
    {
        offBubble.gameObject.SetActive(false);
        defBubble.gameObject.SetActive(false);
        dead = true;
        GetComponent<Animator>().SetTrigger("death");
        GetComponent<ParticleSystem>().Emit(10);
        Destroy(gameObject, 2f);

        hordeScript.ZombieDied(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameData == null) return;
        /*
        // if we touch another zombie while attached to the king, recruit that one too
        if (other.tag == "Zombie" && isAttachedToKing && !hordeScript.kingZombie.GetComponent<KingZombieScript>().dead)
        {
           // hordeScript.AttachZombie(other.gameObject);
        }
        */
        if (other.tag == "KetchupBlob")
        {
            if (Hit(gameData.ketchupDamage)) stats.kills++;
        }
    }
}
