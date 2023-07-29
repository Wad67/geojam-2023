using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    
    public List<string> gameScenes;
    


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        
        DontDestroyOnLoad(this.gameObject);
        
        
    }
    
    
    
    public void LoadScene(int idx){
    	SceneManager.LoadScene(gameScenes[idx]);
    }
}
