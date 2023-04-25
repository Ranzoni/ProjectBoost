using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip deathExplosionAudio;
    [SerializeField] AudioClip successAudio;

    [SerializeField] ParticleSystem deathExplosionParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool isCollisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
            LoadNextLevel();
        else if (Input.GetKeyDown(KeyCode.C))
            DisableCollisions();
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || isCollisionDisabled)
            return;

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Nothing");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StopMovement()
    {
        var movementScript = GetComponent<Movement>();
        movementScript.enabled = false;
        isTransitioning = true;
        audioSource.Stop();
    }

    void RestartMovement()
    {
        var movementScript = GetComponent<Movement>();
        movementScript.enabled = true;
    }

    void LoadNextLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;

        SceneManager.LoadScene(nextSceneIndex);

        RestartMovement();
    }

    void StartSuccessSequence()
    {
        StopMovement();
        audioSource.PlayOneShot(successAudio);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void ReloadLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void StartCrashSequence()
    {
        StopMovement();
        audioSource.PlayOneShot(deathExplosionAudio);
        deathExplosionParticles.Play();
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void DisableCollisions()
    {
        isCollisionDisabled = !isCollisionDisabled;
    }
}