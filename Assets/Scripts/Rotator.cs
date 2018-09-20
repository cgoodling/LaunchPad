using UnityEngine;

[DisallowMultipleComponent]
public class Rotator : MonoBehaviour {
    [SerializeField] float speed = 100f;
    [SerializeField] bool reverse = false;
	
	// Update is called once per frame
	void Update () {
        float rotationsThisFrame = Time.deltaTime * speed;

        if(reverse){
            transform.Rotate(Vector3.up, rotationsThisFrame);
        } else {
            transform.Rotate(Vector3.down, rotationsThisFrame);
        }
	}
}
