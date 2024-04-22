///////////////////////////////////////////
/////// Script Contributors:
/////// Rachel Huggins
///////
///////
///////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    public static MusicManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private AudioSource _audioSource;
    [SerializeField] public AudioClip[] songs;
    [SerializeField] public float volume;
    private float trackTimer;
    private float songsPlayed;
    private bool[] beenPlayed;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        beenPlayed = new bool[songs.Length];

        RandomizeSong();
    }

    // Update is called once per frame
    void Update()
    {
        _audioSource.volume = volume;

        if (_audioSource.isPlaying)
        {
            trackTimer += 1 * Time.deltaTime;
        }

        RandomizeSong();
        RestartShuffle();
    }   

    public void ChangeSong(int songPicked)
    {
        if (!beenPlayed[songPicked])
        {
            trackTimer = 0;
            songsPlayed++;
            beenPlayed[songPicked] = true;
            _audioSource.clip = songs[songPicked];
            _audioSource.Play();
        }
        else
        {
            _audioSource.Stop();
        }
    }

    public void RandomizeSong()
    {
        if (!_audioSource.isPlaying || trackTimer >= _audioSource.clip.length)
        {
            ChangeSong(Random.Range(0, songs.Length));
        }
    }

    void RestartShuffle()
    {
        if (songsPlayed == songs.Length)
        {
            songsPlayed = 0;

            for (int i = 0; i < songs.Length; i++)
            {
                if (i == songs.Length)
                {
                    break;
                }
                else
                {
                    beenPlayed[i] = false;
                }
            }
        }
    }
}
