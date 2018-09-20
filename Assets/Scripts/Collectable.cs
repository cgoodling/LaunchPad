using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectable : MonoBehaviour {
    private bool collected = false;

    Collider c_Collider;
    Renderer c_Renderer;
    Rigidbody c_RigidBody;

    public static Collectable instance = null;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        c_Collider = GetComponent<Collider>();
        c_Renderer = GetComponent<Renderer>();
        c_RigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {;
        print("collected: " + collected);
	}


    private void OnTriggerEnter(Collider other)
    {
        collected = true;

        c_Collider.enabled = false;
        c_Renderer.enabled = false;
        c_RigidBody.freezeRotation = true;
        c_RigidBody.useGravity = false;
    }

    void OnEnable()
    {
        //Start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        //Stop listening for a scene change as soon as this script is disabled.
        //Remember to always have an unsubscription for every delegate you
        //subscribe to!
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (collected)
        {
            Debug.Log("SHOULD DESTROY ME");
            Destroy(gameObject);
        }
    }


}
