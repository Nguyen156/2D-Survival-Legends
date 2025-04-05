using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IGameStateListener, IPlayerStatsDependency
{
    [Header(" Components ")]
    private Rigidbody2D rb;

    [Header(" Settings ")]
    [SerializeField] private float baseMoveSpeed;
    private float moveSpeed;
    private Vector2 moveDir;
    private bool canMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Movement();
    }

    private void CheckInput()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical");
    }

    private void Movement()
    {
        if (!canMove)
            return;

        rb.velocity = moveDir.normalized * moveSpeed;
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                canMove = true;
                break;

            default:
                canMove = false;
                rb.velocity = Vector2.zero;
                break;
        }
    }

    public void UpdateStats(PlayerStatsManager playerStats)
    {
        float moveSpeedPercent = playerStats.GetStatValue(Stat.MoveSpeed) / 100;
        moveSpeed = baseMoveSpeed * (1 + moveSpeedPercent);
        

    }
}
