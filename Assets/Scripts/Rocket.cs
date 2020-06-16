
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
//test 2

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine = default;
    [SerializeField] AudioClip deathSound = default;
    [SerializeField] AudioClip levelEndSound = default;

    [SerializeField] ParticleSystem mainEngineParticlesRear = default;
    [SerializeField] ParticleSystem mainEngineParticlesFront = default;
    [SerializeField] ParticleSystem RCSParticlesRear = default;
    [SerializeField] ParticleSystem RCSParticlesFront = default;
    [SerializeField] ParticleSystem deathParticles = default;
    [SerializeField] ParticleSystem levelEndParticles = default;
    [SerializeField] float levelLoadDelay = 2f;

    bool CollisionsOn = true;

    Rigidbody rigidbody;
    AudioSource audioSource;
    enum State { Alive , Dying, Trancending }
    State state = State.Alive;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive) {
            RespondToRotateInput();
            RespondToThrustInput();
  //          if (Debug.isDebugBuild)   // re-add for dev build
    //        {
             CheckForDebugKeys();
      //      }
        }
       
    }

   
    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) {
            return;
        }
        if (CollisionsOn)
        {
            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    break;
                case "Finish":
                    PlayerSuccess();
                    break;
                default:
                    PlayerDied();
                    break;
            }
        }
    }

    private void PlayerSuccess()
    {
        state = State.Trancending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelEndSound);
        levelEndParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    private void PlayerDied()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
        state = State.Alive;
    }

    private void LoadNextLevel()
    {
        int currenctSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currenctSceneIndex+1;
        int totalSceneIndex = SceneManager.sceneCountInBuildSettings;


        if (currenctSceneIndex < (totalSceneIndex - 1)) 
        {
            SceneManager.LoadScene(nextSceneIndex);
            state = State.Alive;
        }
        else
        {
            LoadFirstLevel();
        }

    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticlesRear.Stop();
            mainEngineParticlesFront.Stop();
        }
    }
   

    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
            mainEngineParticlesRear.Play();
            mainEngineParticlesFront.Play();
        }
        
    }

    private void RespondToRotateInput()
    {
        rigidbody.angularVelocity = Vector3.zero; // Take control of rotation, freeze physics rotations
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            RCSParticlesRear.Play();
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RCSParticlesFront.Play();
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        } 
    }
    private void CheckForDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            Invoke("LoadNextLevel", 0);
        }
        if (Input.GetKey(KeyCode.C))
        {
            CollisionsOn = !CollisionsOn;
        }
    }
}
