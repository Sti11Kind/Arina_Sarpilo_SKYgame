using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip wrongSound;
    [SerializeField] private AudioClip startSound;
    [SerializeField] private AudioClip finishSound;

    [SerializeField] private AudioSource moveSource;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private PlayerControl player;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    private void OnEnable()
    {
        Obstacles.OnPlayerHit += PlayCollisionSound;
        SlalomFlag.OnCorrectPass += PlayCorrectSound;
        SlalomFlag.OnWrongPass += PlayWrongSound;

        StarpGate.StartRace += PlayStartSound;
        FinishGate.FinishRace += PlayFinishSound;
    }

    private void OnDisable()
    {
        Obstacles.OnPlayerHit -= PlayCollisionSound;
        SlalomFlag.OnCorrectPass -= PlayCorrectSound;
        SlalomFlag.OnWrongPass -= PlayWrongSound;

        StarpGate.StartRace -= PlayStartSound;
        FinishGate.FinishRace -= PlayFinishSound;
    }

    private void PlayCollisionSound()
    {
        audioSource.PlayOneShot(collisionSound);
    }

    private void PlayCorrectSound()
    {
        if (audioSource != null && correctSound != null)
            audioSource.PlayOneShot(correctSound);
    }

    private void PlayWrongSound()
    {
        if (audioSource != null && wrongSound != null)
            audioSource.PlayOneShot(wrongSound);
    }

    private void PlayStartSound()
    {
        if (audioSource != null && startSound != null)
            audioSource.PlayOneShot(startSound);
    }

    private void PlayFinishSound()
    {
        if (audioSource != null && finishSound != null)
            audioSource.PlayOneShot(finishSound);
    }

    void Update()
    {
        if (player != null)
        {
            if (player.IsGrounded() && player.IsMoving())
            {
                if (!moveSource.isPlaying)
                {
                    moveSource.clip = moveSound;
                    moveSource.loop = true;
                    moveSource.Play();
                }
            }
            else
            {
                if (moveSource.isPlaying)
                    moveSource.Stop();
            }
        }
    }
}
