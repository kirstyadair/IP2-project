using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public SpriteRenderer sprt;
    HordeScript hordeScript;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Animator>().speed = UnityEngine.Random.Range(0.5f, 1.5f);
        hordeScript = GameObject.Find("Horde").GetComponent<HordeScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // make it more red the deader it is
        Color clr = sprt.color;
        clr.g = health / maxHealth;
        clr.b = health / maxHealth;
        sprt.color = clr;
    }

    public void Hit(float hp)
    {
        health -= hp * hordeScript.defensiveStat;
        if (health <= 0) Kill();
    }

    public void Kill()
    {
        GetComponent<Animator>().SetTrigger("death");
        GetComponent<ParticleSystem>().Emit(10);
        Destroy(gameObject, 2f);
    }
}
