using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCollect : MonoBehaviour
{
    [SerializeField] int energyAmount = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCircleCollider") {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().energy += energyAmount;
            Destroy(this.gameObject);
        }
    }
}
