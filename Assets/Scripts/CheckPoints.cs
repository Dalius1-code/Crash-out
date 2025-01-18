using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    Player player;
    SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    public Sprite passive, active;
    public Transform respawnPoint;
    public AudioClip stageSound;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player.UpdateCheckPoint(respawnPoint.position);
            spriteRenderer.sprite = active;
            audioSource.PlayOneShot(stageSound);
        }
    }
}
