using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{

    private float wind = 20f;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * wind, ForceMode2D.Impulse);
            }
        }
    
}
