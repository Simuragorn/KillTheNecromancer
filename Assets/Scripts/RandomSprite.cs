using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> sprites;
    void Start()
    {
        int spriteIndex = Random.Range(0, sprites.Count);
        spriteRenderer.sprite = sprites[spriteIndex];
    }

}
