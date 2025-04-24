using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Optional: Restore saved music state
        if (PlayerPrefs.GetInt("MusicOn", 1) == 0)
        {
            musicSource.Pause();
        }
    }

    public void ToggleMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
            PlayerPrefs.SetInt("MusicOn", 0);
        }
        else
        {
            musicSource.Play();
            PlayerPrefs.SetInt("MusicOn", 1);
        }

        PlayerPrefs.Save();
    }

    public bool IsMusicPlaying()
    {
        return musicSource.isPlaying;
    }
}
