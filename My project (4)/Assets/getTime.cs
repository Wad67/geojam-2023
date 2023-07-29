using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class getTime : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private string formattedTime;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        UpdateTimeText();
    }

    // Update the time text
    private void UpdateTimeText()
    {
        if (GameManager.Instance != null)
        {
            float time = GameManager.Instance.time;
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

            formattedTime = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            textMesh.text = formattedTime;
        }
    }
}

