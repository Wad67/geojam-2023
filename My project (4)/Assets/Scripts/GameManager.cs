using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    
    public List<string> gameScenes;
    
    public List<AudioClip> gameSounds;
    
    public List<AudioClip> musicLooping;
    
    public List<AudioClip> musicIntro;
    
    
    
    
    // public for debug purposes only
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
        } else {
            _instance = this;
        }
        
        DontDestroyOnLoad(this.gameObject);
        
        
        backingTrack.clip = musicIntro[0];
        backingTrack.Play();
        
        
    }
    
    void Update()
    {
	time += Time.deltaTime;
	Debug.Log(time);
	
	// Switch from intro backing to loop once it is done	
	if(!backingTrack.isPlaying){
		backingTrack.clip = musicLooping[0];
		backingTrack.loop = true;
		backingTrack.Play();
	
	}

    }
    
    
    
    
    public void LoadScene(int idx){
    	SceneManager.LoadScene(gameScenes[idx]);
    }
}
