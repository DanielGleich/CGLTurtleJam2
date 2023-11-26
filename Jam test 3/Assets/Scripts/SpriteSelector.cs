using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelector : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    [SerializeField] SpriteRenderer spriteRenderer;

    void Start()
    {
        if (sprite != null && spriteRenderer != null) {
            spriteRenderer.sprite = sprite;
        }
    }
}
