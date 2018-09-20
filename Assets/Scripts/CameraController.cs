using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public GameObject playerInScene;

    private Vector3 offset;

	// Use this for initialization
	void Start() {
        transform.position = new Vector3(playerInScene.transform.position.x, playerInScene.transform.position.y, GlobalConstants.PLAYER_CAM_FIXED_Z);
        offset = transform.position - playerInScene.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate() {
        transform.position = playerInScene.transform.position + offset;
	}
}
