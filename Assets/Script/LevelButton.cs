using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite unlockedSprite;

    private Button button;
    private Image buttonImage;

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        if (LevelProgress.IsLevelUnlocked(levelNumber))
        {
            button.interactable = true;
            buttonImage.sprite = unlockedSprite;
            button.onClick.AddListener(() => SceneManager.LoadScene(sceneToLoad));
        }
        else
        {
            button.interactable = false;
            buttonImage.sprite = lockedSprite;
        }
    }

}
