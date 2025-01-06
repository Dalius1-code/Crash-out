using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    Player player;
    SpriteRenderer spriteRenderer;
    public Sprite passive, active;
    public Transform respawnPoint;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player.UpdateCheckPoint(respawnPoint.position);
            spriteRenderer.sprite = active;
        }
    }
}
