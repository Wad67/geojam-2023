using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public int musicIntroSequence; // The intro sequence index to play
    public int musicLoopSequence; // The loop sequence index to play

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the music change request in the GameManager
            GameManager.Instance.ChangeMusicSequence(musicIntroSequence, musicLoopSequence);
        }
    }
}

