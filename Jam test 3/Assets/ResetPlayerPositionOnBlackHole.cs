using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResetPlayerPositionOnBlackHole : MonoBehaviour
{
    Vector3 startRombaPosition;
    GameObject player;
    GameObject circle;
    Movement movcir;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        circle = GameObject.FindGameObjectWithTag("PlayerCircleCollider");
        movcir = player.GetComponent<Movement>();
        startRombaPosition = player.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCircleCollider")
        {
            player.transform.position = startRombaPosition;
            movcir.nextStepTarget = startRombaPosition + new Vector3(movcir.moveDirection.x*0.66f, movcir.moveDirection.y*0.66f, 0);
            //movcir.StartCoroutine(movcir.PrepNextStep());
        }
    }
}
