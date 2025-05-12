using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof(PlayerLevel))]
public class Player : MonoBehaviour
{
    public static Player instance;

    [Header(" Components ")]
    private SpriteRenderer sr;
    private PlayerHealth playerHealth;
    private PlayerLevel playerLevel;

    public bool FacingRight { get; private set; } = true;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        sr = GetComponentInChildren<SpriteRenderer>();

        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();

        CharacterSelectionManager.OnCharacterSelected += CharacterSelectedCallback;
    }

    private void OnDestroy()
    {
        CharacterSelectionManager.OnCharacterSelected -= CharacterSelectedCallback;
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);

        AudioManager.instance.PlaySFX(11);
        CameraManager.instance.ScreenShake();
    }

    public bool HasLevelUp()
    {
        return playerLevel.HasLevelUp();
    }

    private void CharacterSelectedCallback(CharacterDataSO characterData)
    {
        sr.sprite = characterData.Sprite;
    }

    public void HandleFlip(float xValue)
    {
        if (xValue < 0 && Player.instance.FacingRight || xValue > 0 && !FacingRight)
            Flip();
    }

    private void Flip()
    {
        FacingRight = !FacingRight;
        sr.flipX = !FacingRight;
    }
}
