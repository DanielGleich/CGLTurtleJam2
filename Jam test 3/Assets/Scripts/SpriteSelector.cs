using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelector : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        foreach (Transform child in transform) {
            
            if (transform.gameObject.name.Contains("Sprite") && child.TryGetComponent<SpriteRenderer>(out spriteRenderer)) {
                break;
            }
         }
        if (sprite != null && spriteRenderer != null) {
            spriteRenderer.sprite = sprite;
        }
    }
}
