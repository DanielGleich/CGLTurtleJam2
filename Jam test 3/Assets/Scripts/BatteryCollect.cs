using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCollect : MonoBehaviour
{
    [SerializeField] int energyAmount = 10;
    [SerializeField] SoundManager soundManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCircleCollider") {
            Movement temp = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
            temp.energy += energyAmount;
            Destroy(this.gameObject);
        }
    }
}
