using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private TextMeshPro damageText;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Animate(int damage, bool isCriticalHit)
    {
        damageText.text = damage.ToString();
        damageText.color = isCriticalHit ? Color.red : Color.white;

        anim.Play("Animate");
    }
}
