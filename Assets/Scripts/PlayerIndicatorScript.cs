using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerIndicatorScript : MonoBehaviour
{
    public Animator animator;
    public TextMeshPro text;
    public SpriteRenderer sprite;

    public void Show(string text, Color color, float seconds)
    {
        this.text.text = text;
        sprite.color = color;
        animator.Play("show");
        StartCoroutine(HideAfter(seconds));
    }

    private IEnumerator HideAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        animator.Play("hide");
    }
}
