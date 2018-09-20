using UnityEngine;

public class CameraManager : MonoBehaviour {

    public GameObject cameraOne;
    public GameObject cameraTwo;

    AudioListener cameraOneAudioLis;
    AudioListener cameraTwoAudioLis;

    // Use this for initialization
    void Start() {
        //Get Camera Listeners
        cameraOneAudioLis = cameraOne.GetComponent<AudioListener>();
        cameraTwoAudioLis = cameraTwo.GetComponent<AudioListener>();

        //Camera Position Set
        CameraPositionChange(GameManager.instance.GetCameraPosition());
    }

    // Update is called once per frame
    void Update() {
        SwitchCamera();
    }

    //Change Camera Keyboard
    void SwitchCamera() {
        if (Input.GetKeyDown(KeyCode.P)) {
            CameraPositionChange(GameManager.instance.ChangeCameraPosition());
        }
    }


    //Camera change Logic
    void CameraPositionChange(int camPosition) {

        //Set camera position 1
        if (camPosition == GlobalConstants.CAMERA_MAIN) {
            cameraOne.SetActive(true);
            cameraOneAudioLis.enabled = true;

            cameraTwoAudioLis.enabled = false;
            cameraTwo.SetActive(false);
        }

        //Set camera position 2
        if (camPosition == GlobalConstants.CAMERA_FOLLOW) {
            cameraTwo.SetActive(true);
            cameraTwoAudioLis.enabled = true;

            cameraOneAudioLis.enabled = false;
            cameraOne.SetActive(false);
        }
    }
}
