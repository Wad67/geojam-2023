using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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

    public GameObject slidingPanel;
    bool slidingPanelToggle = true;
    private Vector3 panelStartPosition;
    private Vector3 panelEndPosition;
    public Slider volumeSlider;

    public int questCount = 3;

    public TextMeshProUGUI counterValue;

    private bool isFirstLevelLoaded = false;

    void Awake()
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

        // Store the start and end anchored positions for slidingPanel animation
        panelStartPosition = slidingPanel.GetComponent<RectTransform>().anchoredPosition;
        panelEndPosition = panelStartPosition + new Vector3(0, 160, 0);

        foreach (GameObject itemPrefab in items)
        {
            GameObject newItem = Instantiate(itemPrefab, inventoryPanel.transform);
            AddItem(newItem);
        }
        PreloadAudioClips();
    }

    private void PreloadAudioClips()
    {
        foreach (AudioClip clip in musicIntro)
        {
            clip.LoadAudioData();
        }

        foreach (AudioClip clip in musicLooping)
        {
            clip.LoadAudioData();
        }

        foreach (AudioClip clip in gameSounds)
        {
            clip.LoadAudioData();
        }
    }

    void Start()
    {
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    void Update()
    {
        counterValue.text = questCount.ToString();
        if (isFirstLevelLoaded)
        {
            time += Time.deltaTime;
        }

        if (!backingTrack.isPlaying)
        {
            backingTrack.clip = musicLooping[0];
            backingTrack.loop = true;
            backingTrack.Play();
        }
        if (questCount <= 0)
        {
            LoadScene(2);
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
        items.Add(item);
    }

    public void LoadScene(int idx)
    {
        if (idx == 1 && !isFirstLevelLoaded)
        {
            isFirstLevelLoaded = true;
        }

        SceneManager.LoadScene(gameScenes[idx]);
    }

    public void ToggleMenu()
    {
        if (slidingPanelToggle)
        {
            StartCoroutine(SlidePanel(panelStartPosition, panelEndPosition, 0.5f));
        }
        else
        {
            StartCoroutine(SlidePanel(panelEndPosition, panelStartPosition, 0.5f));
        }

        // Toggle the slidingPanelToggle value
        slidingPanelToggle = !slidingPanelToggle;
    }

    private IEnumerator SlidePanel(Vector2 startPos, Vector2 endPos, float duration)
    {
        RectTransform rectTransform = slidingPanel.GetComponent<RectTransform>();

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the current position based on interpolation
            float t = Mathf.Clamp01(elapsedTime / duration);
            Vector2 newPos = Vector2.Lerp(startPos, endPos, t);

            // Apply the new anchored position to the slidingPanel
            rectTransform.anchoredPosition = newPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exact
        rectTransform.anchoredPosition = endPos;
    }

    private void ChangeVolume(float value)
    {
        // Ensure the value is clamped between 0 and 1
        value = Mathf.Clamp01(value);

        // Adjust the volume of the AudioSources based on the slider value
        gameSoundsSource.volume = value;
        backingTrack.volume = value;
    }

    // Check if the player has the specified number of items
    public bool HasItems(string itemName, int count)
    {
        // Find all items in the items list that have the specified name
        List<GameObject> matchingItems = items.FindAll(item => item.name.Contains(itemName));

        // If the number of matching items is greater than or equal to the required count, return true
        return matchingItems.Count >= count;
    }

    // Remove the specified number of items from the inventory
    public void RemoveItems(string itemName, int count)
    {
        // Find all items in the items list that have the specified name
        List<GameObject> matchingItems = items.FindAll(item => item.name.Contains(itemName));

        // Check if there are enough items to remove
        if (matchingItems.Count >= count)
        {
            // Remove the required number of items from the inventory
            for (int i = 0; i < count; i++)
            {
                GameObject itemToRemove = matchingItems[i];

                // Call the method to handle the removal and update the serialized properties
                HandleItemRemoval(itemToRemove);

                // Destroy the game object after removing it from the list
                Destroy(itemToRemove);
            }
        }
        else
        {
            Debug.LogError("Insufficient items to remove.");
        }
    }

    // Method to handle item removal and update serialized properties
    private void HandleItemRemoval(GameObject itemToRemove)
    {
        int index = items.IndexOf(itemToRemove);

        if (index >= 0 && index < items.Count)
        {
            // Remove the item from the items list
            items.RemoveAt(index);

            // Handle updating the serialized properties here
            // For example, if using a SerializedObject, update the property array size or use DeleteArrayElementAtIndex to remove the element at the specified index.
        }
    }

    public void ChangeMusicSequence(int introIndex, int loopIndex)
    {
        // Make sure the provided indices are within the bounds of the musicIntro and musicLooping lists
        if (introIndex < 0 || introIndex >= musicIntro.Count || loopIndex < 0 || loopIndex >= musicLooping.Count)
        {
            Debug.LogError("Invalid intro or loop index provided.");
            return;
        }

        // Stop the current music
        backingTrack.Stop();

        // Set the intro track and start playing it
        backingTrack.clip = musicIntro[introIndex];
        backingTrack.loop = false;
        backingTrack.Play();

        // Start the loop track after the intro track finishes
        StartCoroutine(StartLoopAfterIntroFinishes(musicIntro[introIndex].length, loopIndex));
    }

    private IEnumerator StartLoopAfterIntroFinishes(float delay, int loopIndex)
    {
        yield return new WaitForSeconds(delay);

        // Set the loop track and start playing it
        backingTrack.clip = musicLooping[loopIndex];
        backingTrack.loop = true;
        backingTrack.Play();
    }
}

