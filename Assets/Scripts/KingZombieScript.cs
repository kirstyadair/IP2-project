using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingZombieScript : ZombieScript
{
    public delegate void KingZombieEvent();
    public event KingZombieEvent OnKingZombieDies;
    public HordeScript hordeScript;
    public Animator animator;
    public float moveAttachedBackBy = 1;
    public override bool Hit(float hp)
    {
        health -= hp;

        if (health <= 0)
        {
            Die();
            return true;
        } return false;
    }

    void Die()
    {
        dead = true;
        animator.Play("die");

        // detach all zombies
        foreach (Transform zombie in transform)
        {
            if (zombie.gameObject.tag == "Zombie") hordeScript.DetachZombie(zombie.gameObject);
        }

        if (OnKingZombieDies != null) OnKingZombieDies();

        Destroy(gameObject, 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // so we don't attach zombies while we're dying
        if (dead) return;
        if (collision.gameObject.tag == "Zombie" && collision.gameObject.transform.parent != this.transform)
        {
            collision.gameObject.transform.position = collision.gameObject.transform.position - collision.GetContact(0).normal * moveAttachedBackBy;
            hordeScript.AttachZombie(collision.gameObject);
        }
    }


    // Update is called once per frame
    public override void Update()
    {
        
    }
}
