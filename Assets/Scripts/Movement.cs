using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainBoosterParticles;
    [SerializeField] ParticleSystem leftBoosterParticles;
    [SerializeField] ParticleSystem rightBoosterParticles;

    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
            StartThrusting();
        else
            StopThursting();
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);

        if (!mainBoosterParticles.isPlaying)
            mainBoosterParticles.Play();
    }

    void StopThursting()
    {
        audioSource.Stop();
        mainBoosterParticles.Stop();
    }
    
    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
            StartLeftRotation();
        else if (Input.GetKey(KeyCode.D))
            StartRightRotation();
        else
            StopRotation();
    }

    void StartLeftRotation()
    {
        ApplyRotation(rotationThrust);

        if (!rightBoosterParticles.isPlaying)
            rightBoosterParticles.Play();
    }
    
    void StartRightRotation()
    {
        ApplyRotation(-rotationThrust);

        if (!leftBoosterParticles.isPlaying)
            leftBoosterParticles.Play();
    }

    void StopRotation()
    {
        rightBoosterParticles.Stop();
        leftBoosterParticles.Stop();
    }
    
    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
