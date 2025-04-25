using UnityEngine;

public class GlobalManager : MonoBehaviour{
    public static GlobalManager Instance {get; private set;} //Singleton instance
    [SerializeField] private StateManager stateManager; //Reference to the StateManager script
    [SerializeField] private GameManager gameManager; //Reference to the GameManager script
    [SerializeField] private UIManager uiManager; //Reference to the UIManager script
    [SerializeField] private AudioManager audioManager; //Reference to the AudioManager script
    
    void Awake(){ //Should run on script/object load

        Debug.Log("Hello World!");

        if(Instance == null){
            Instance = this; //Set the instance to this object
            DontDestroyOnLoad(gameObject); //Don't destroy this object when loading a new scene
        } else {
            Destroy(gameObject); //Destroy this object if another instance already exists
            Debug.Log("Duplicate GlobalManager destroyed.");
        }
        
        Debug.Log("Game Initialized.");
    }
    void Start(){ //Runs the moment the object is active?
        Debug.Log("Game Started.");
    }

    void Update(){ //Runs every frame
        Debug.Log("Game Updating.");
    }

    void OnDestroy(){ //Runs when the object is destroyed
        Debug.Log("Game Destroyed.");
        Debug.Log("Goodbye World!");
    }
}