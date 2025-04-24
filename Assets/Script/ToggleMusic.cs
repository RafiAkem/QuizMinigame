using UnityEngine;
using UnityEngine.UI;  // Make sure this is included for UI elements like Image and Button

public class ToggleMusic : MonoBehaviour
{
    [SerializeField] private Button musicToggleButton; // Button to toggle music
    [SerializeField] private Sprite musicOnIcon;      // Icon for when music is on
    [SerializeField] private Sprite musicOffIcon;     // Icon for when music is off
    [SerializeField] private Image buttonImage;       // Image component for the button

    private void Start()
    {
        if (musicToggleButton == null)
            Debug.LogError("Toggle button is not assigned!");
        if (buttonImage == null)
            Debug.LogError("Button image is not assigned!");

        // Set up the button click listener
        musicToggleButton.onClick.AddListener(() =>
        {
            Debug.Log("Toggle button clicked");
            MusicManager.Instance.ToggleMusic();  // Toggle the music
            UpdateIcon();  // Update the button image after toggling music
        });

        // Initialize the button icon
        UpdateIcon();
    }

    // Update the button's image based on the music state
    void UpdateIcon()
    {
        if (buttonImage == null)
        {
            Debug.LogError("Button image is null!");
            return;
        }

        // Update the sprite based on whether music is playing or not
        if (MusicManager.Instance.IsMusicPlaying())
        {
            buttonImage.sprite = musicOnIcon;  // Set the "on" sprite
        }
        else
        {
            buttonImage.sprite = musicOffIcon;  // Set the "off" sprite
        }
    }
}
