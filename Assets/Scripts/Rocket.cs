using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [Header("Level Load Delay")]
    [SerializeField] float levelLoadDelay = 2f;

    [Header("ThrustValue")]
    [SerializeField] float rcsThrust = 100f; //rcs rocket control system
    [SerializeField] float mainThrust = 10f; //rcs rocket control system

    [Header("SoundFX")]
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip successfulLanding;

    [Header("ParticleFX")]
    [SerializeField] ParticleSystem mainEngineFX;
    [SerializeField] ParticleSystem levelCompledFX;
    [SerializeField] ParticleSystem deathFX;


    enum State { Alive, Dying, Transcending}
    State state = State.Alive;
    //bool collisionsAreDisabled = false;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrust();
            RespondToRotation();
        }
        //RespondToDebugKey();

    }

   /* private void RespondToDebugKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartSuccessSequence();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsAreDisabled = !collisionsAreDisabled;
        }
    }*/

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) //|| collisionsAreDisabled)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Ok");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;

        }
    }
    
    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathFX.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(successfulLanding);
        levelCompledFX.Play();
        Invoke("LoadNextLevel", levelLoadDelay); // parameterise time
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);//allow for more than 2 levels
    }
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
    private void RespondToRotation()
    {
        rigidBody.freezeRotation = true; // take manual control of the rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;


        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame );
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; // resume physics control of rotation

    } // rotaciq

    private void RespondToThrust() //тласък, звук
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineFX.Stop();
        }
    }

    private void ApplyThrust()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineFX.Play();
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
    }
}
