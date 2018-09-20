using UnityEngine;
using UnityEngine.UI;


[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour {
    
    // GameObject rigid body
    Rigidbody rigidBody;

    // GameObject starting position
    Vector3 startPosition;
    Quaternion startRotation;

    [SerializeField] float respawnDelay = GlobalConstants.PLAYER_SPAWN_DELAY;

    //AudioSources For Player
    AudioSource[] audioSource;

    // Sounds
    [SerializeField] AudioClip mainThruster;
    [SerializeField] AudioClip shipExplosion;
    [SerializeField] AudioClip levelComplete;
    [SerializeField] AudioClip collectCrystal;
    [SerializeField] AudioClip collectLife;

    // Thrust Control
    [SerializeField] float RCSThrust = GlobalConstants.PLAYER_RCS_THRUST;
    [SerializeField] float MainThrust = GlobalConstants.PLAYER_MAIN_THRUST;

    // Particle Systems
    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem explosionParticles;

    // Debug
    bool collisionsDisabled = false;

    // Game State
    bool isTransitioning = false;

    // Player Stats
    int playerLives;
    int playerScore;

    // Player stats text objects
    public Text playerLivesText;
    public Text playerScoreText;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
        audioSource = GetComponents<AudioSource>();
		UpdatePlayerLives(); //Set player lives from game manager
        UpdatePlayerScore(); //Set the player score
	}

	// Update is called once per frame
	void Update () {
        if (!isTransitioning) {
            ManageThrust();
            ManageRotation();
        }
        //only in debug mode
        if (Debug.isDebugBuild){
			RespondToDebugKeys();
        }
	}

    private void RespondToDebugKeys() {
        if (Input.GetKeyDown(KeyCode.L)){
            GameManager.instance.NextLevel();
        }
        if (Input.GetKeyDown(KeyCode.C)){
            // toggle collision
            collisionsDisabled = !collisionsDisabled;
        }
    }

    // For collision with static objects
	private void OnCollisionEnter(Collision collision) {

        if (isTransitioning || collisionsDisabled) return;

        switch (collision.gameObject.tag){
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    // For collision with dynamic objects
    private void OnTriggerEnter(Collider other) {
        if(!isTransitioning) {
            if(other.CompareTag("Bonus100")) {
                CollectBonus(GlobalConstants.BLUE_CRYSTAL_BONUS);
                other.gameObject.SetActive(false);
            } else if (other.CompareTag("Bonus500")){
                CollectBonus(GlobalConstants.YELLOW_CRYSTAL_BONUS);
                other.gameObject.SetActive(false);
            } else if(other.CompareTag("Bonus1000")) {
                CollectBonus(GlobalConstants.RED_CRYSTAL_BONUS);
                other.gameObject.SetActive(false);
            } else if (other.CompareTag("Bonus1UP")) {
                CollectLife();
                other.gameObject.SetActive(false);
            }
        }
    }

    private void StartDeathSequence() {
        isTransitioning = true;
        audioSource[0].Stop();
        audioSource[0].PlayOneShot(shipExplosion);
        explosionParticles.Play();

        if(GameManager.instance.GetLives() == 0) {
            GameManager.instance.GameOver();
        } else {
            GameManager.instance.DecreaseLives();
            UpdatePlayerLives();
            Invoke("RespawnPlayer", respawnDelay);
        }
    }

    private void StartSuccessSequence() {
        isTransitioning = true;
        audioSource[0].Stop();
        audioSource[0].PlayOneShot(levelComplete);
        successParticles.Play();
        GameManager.instance.IncreaseScore(GlobalConstants.COMPLETE_LEVEL_BONUS);
        GameManager.instance.NextLevel();
    }

	private void ManageThrust() {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) {
            ApplyThrust();
        } else {
            StopThrust();
        }
    }

    private void StopThrust() {
        if (audioSource[0].isPlaying) {
            //stop the audio if playing and thrust not pressed
            audioSource[0].Stop();
        }
        engineParticles.Stop();
    }

    private void ApplyThrust() {
        rigidBody.AddRelativeForce(Vector3.up * MainThrust * Time.deltaTime);

        if (!audioSource[0].isPlaying) {
            audioSource[0].PlayOneShot(mainThruster);
        }
        engineParticles.Play();
    }

    private void ManageRotation() {

        //remove rotation due to physics
		rigidBody.angularVelocity = Vector3.zero;

		float rotationThisFrame = Time.deltaTime * RCSThrust;

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-(Vector3.forward * rotationThisFrame));
        }
    }

	void UpdatePlayerLives() {
        playerLives = GameManager.instance.GetLives();
        playerLivesText.text = playerLives.ToString();
    }

    void UpdatePlayerScore() {
        playerScore = GameManager.instance.GetScore();
        playerScoreText.text = "SCORE: " + playerScore.ToString();
    }

    private void CollectLife() {
        GameManager.instance.IncreaseLives();
        UpdatePlayerLives();

        GameManager.instance.IncreaseScore(GlobalConstants.EXTRA_LIFE_BONUS);
        UpdatePlayerScore();

        audioSource[1].Stop();
        audioSource[1].PlayOneShot(collectLife);
    }

    void CollectBonus(int score) {
        //Save score in game manager
        GameManager.instance.IncreaseScore(score);

        //Update local score and UI
        UpdatePlayerScore();

        audioSource[1].Stop();
        audioSource[1].PlayOneShot(collectCrystal);
    }

    void RespawnPlayer(){
        isTransitioning = false;
        transform.position = startPosition;
        transform.rotation = startRotation;
        rigidBody.velocity = new Vector3(0f, 0f, 0f);
        rigidBody.angularVelocity = new Vector3(0f, 0f, 0f);
    }

}
