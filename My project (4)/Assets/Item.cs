using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Get references to the BoxCollider2D and GameManager
        boxCollider = GetComponent<BoxCollider2D>();
        gameManager = GameManager.Instance;
    }

    // Called when another collider enters this object's trigger area
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to an entity in the "player" group
        if (other.CompareTag("Player"))
        {
            // Call GameManager's AddItem method and pass this item as the parameter
            gameManager.AddItem(gameObject);
        }
    }
}

