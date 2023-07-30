using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public SpriteRenderer doorSpriteRenderer;
    public Sprite openSprite;
    public Sprite closedSprite;

    private bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isOpen)
            {
                // Play door open sound
                GameManager.Instance.gameSoundsSource.PlayOneShot(GameManager.Instance.gameSounds[1]);
                // Change sprite to open
                doorSpriteRenderer.sprite = openSprite;
                isOpen = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isOpen)
            {
                // Play door close sound
                GameManager.Instance.gameSoundsSource.PlayOneShot(GameManager.Instance.gameSounds[2]);
                // Change sprite to closed
                doorSpriteRenderer.sprite = closedSprite;
                isOpen = false;
            }
        }
    }
}

