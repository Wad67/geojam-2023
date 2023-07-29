using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public List<string> gameScenes;
    public List<AudioClip> gameSounds;
    public List<AudioClip> musicLooping;
    public List<AudioClip> musicIntro;
    public List<GameObject> items;
    public GameObject inventoryPanel;

    public float time;

    public AudioSource gameSoundsSource;
    public AudioSource backingTrack;
    public AudioSource topTrack;

    bool backingPlaying = false;
    bool topPlaying = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        backingTrack.clip = musicIntro[0];
        backingTrack.Play();

        foreach (GameObject itemPrefab in items)
        {
            GameObject newItem = Instantiate(itemPrefab, inventoryPanel.transform);
            AddItem(newItem);
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        if (!backingTrack.isPlaying)
        {
            backingTrack.clip = musicLooping[0];
            backingTrack.loop = true;
            backingTrack.Play();
        }
    }

    public void AddItem(GameObject item)
    {
        // Make sure the item has a RectTransform component
        if (item.GetComponent<RectTransform>() == null)
        {
            item.AddComponent<RectTransform>();
        }

        // Add the item as a child of the inventoryPanel
        item.transform.SetParent(inventoryPanel.transform, false);


        // Add an Image component to display the sprite as UI image
        Image imageComponent = item.AddComponent<Image>();
        
        // Get the sprite from the original SpriteRenderer (assuming it has one)
        SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            imageComponent.sprite = spriteRenderer.sprite;
        }
        
        
        // Remove the original SpriteRenderer component (since it's no longer needed)
        Destroy(item.GetComponent<SpriteRenderer>());
    }

    public void LoadScene(int idx)
    {
        SceneManager.LoadScene(gameScenes[idx]);
    }
}

