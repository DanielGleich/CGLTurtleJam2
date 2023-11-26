using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashDetect : MonoBehaviour
{
    AudioSource soundManager;

    private void Start()
    {
        soundManager = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCircleCollider") { 
            GameObject.FindAnyObjectByType<TrashManager>().RemoveOneTrash(gameObject);
            Destroy(this.gameObject);
            soundManager.Play();
        }
    }
}
