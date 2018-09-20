using UnityEngine;

[DisallowMultipleComponent]
public class BackAndForthMovement : MonoBehaviour {

    [SerializeField] float speed = 1f;
    [SerializeField] float delta = 5f;
    [SerializeField] bool flipAxis = false;

    private Vector3 startPos;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
	}

    void Update() {
        Vector3 vector = startPos;

        if(flipAxis) {
            vector.y += delta * (Mathf.Sin(Time.time * speed) / 2f);
        } else {
            vector.x += delta * (Mathf.Sin(Time.time * speed) / 2f);
        }
        transform.position = vector;
    }
}


// Algorithm for moving distance (delta) from position of gameobject placement
// vector.y += delta * ((Mathf.Sin(Time.time * speed) + 1) / 2f);