using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FearGameController : MonoBehaviour
{
    public GameObject player;
    public float paranoiaDuration = 60f;
    public AudioClip[] originalSounds;
    public AudioClip[] muffledSounds;
    public AudioSource audioSource;

    private bool isParanoid = false;
    private float timeElapsed = 0f;
    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        // Add event listener to play button
        GameObject.Find("PlayButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PlayGame);

        // Add event listener to exit button
        GameObject.Find("ExitButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ExitGame);

        // Set player to inactive at start
        player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If game is playing and not paranoid, start paranoia mode
        if (isPlaying && !isParanoid)
        {
            StartCoroutine(ParanoiaMode());
        }

        // If in paranoia mode, muffle sounds and darken environment
        if (isParanoid)
        {
            audioSource.clip = muffledSounds[Random.Range(0, muffledSounds.Length)];
            RenderSettings.ambientLight = new Color(0.2f, 0.2f, 0.2f, 1f);
        }
        else
        {
            audioSource.clip = originalSounds[Random.Range(0, originalSounds.Length)];
            RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
        audioSource.Play();

        // If paranoia mode has ended, return environment to normal
        if (timeElapsed >= paranoiaDuration)
        {
            isParanoid = false;
            timeElapsed = 0f;
            RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
    }

    void PlayGame()
    {
        // Set player to active
        player.SetActive(true);

        // Set game as playing
        isPlaying = true;
    }

    void ExitGame()
    {
        // Quit the game
        Application.Quit();
    }

    IEnumerator ParanoiaMode()
    {
        // Set paranoia to true and stop player movement
        isParanoid = true;
        player.GetComponent<UnityEngine.XR.InputDevice>().Disable();

        // Wait for the specified duration
        yield return new WaitForSeconds(paranoiaDuration);

        // Set paranoia to false and re-enable player movement
        isParanoid = false;
        player.GetComponent<UnityEngine.XR.InputDevice>().Enable();
    }
}
