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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Animate(int damage)
    {
        damageText.text = damage.ToString();
        anim.Play("Animate");
    }
}
