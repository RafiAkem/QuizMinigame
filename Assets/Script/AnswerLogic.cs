using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Question
{
    public string questionText;
    public string[] options;
    public int correctAnswerIndex;
    public Sprite questionImage;
    public Sprite[] answerImages;
    public bool usesImageAnswers;
}

public class AnswerLogic : MonoBehaviour
{
    [SerializeField] private Transform buttonField;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Image questionImage;
    [SerializeField] private List<Sprite> answerImages;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip wrongSound;

    [SerializeField] private float questionTime = 10f;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] public GameObject ScoreCanvas;
    [SerializeField] public TextMeshProUGUI ScoreText;
    [SerializeField] private int currentLevelIndex = 1;

    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button retryButton;

    [SerializeField] private GameObject selectLevelBtn1; // Button for incorrect answers
    [SerializeField] private GameObject selectLevelBtn2; // Button for correct answers

    [SerializeField] private float answerFontSize = 24f; // Tambahan untuk atur ukuran font

    private float timer;
    private bool isCountingDown = false;

    public List<Question> questions = new List<Question>();
    private List<GameObject> answerButtons = new List<GameObject>();
    private int currentQuestionIndex = 0;
    private int score = 0;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject button = Instantiate(buttonPrefab, buttonField);
            button.name = "AnswerButton" + i;
            answerButtons.Add(button);
        }
    }

    private void Start()
    {
        DisplayQuestion();
        Time.timeScale = 1f;
        ScoreCanvas.SetActive(false);

        // Ensure both buttons are hidden at the start
        if (selectLevelBtn1 != null) selectLevelBtn1.SetActive(false);
        if (selectLevelBtn2 != null) selectLevelBtn2.SetActive(false);
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex >= questions.Count)
        {
            EndQuiz();
            return;
        }

        Question q = questions[currentQuestionIndex];
        questionText.text = q.questionText;
        questionImage.sprite = q.questionImage;

        timerText.gameObject.SetActive(true);
        questionImage.gameObject.SetActive(true);

        for (int i = 0; i < answerButtons.Count; i++)
        {
            Button btn = answerButtons[i].GetComponent<Button>();
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>(true);
            Image answerImage = answerButtons[i].transform.Find("AnswerImage").GetComponent<Image>();

            btnText.gameObject.SetActive(false);
            answerImage.gameObject.SetActive(false);

            if (q.usesImageAnswers)
            {
                if (i < q.answerImages.Length && q.answerImages[i] != null)
                {
                    answerImage.sprite = q.answerImages[i];
                    answerImage.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning($"Missing image for answer {i}");
                }
            }
            else
            {
                if (i < q.options.Length)
                {
                    btnText.text = q.options[i];
                    btnText.fontSize = answerFontSize; // Set ukuran font
                    btnText.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning($"Missing text for answer {i}");
                }
            }

            btn.onClick.RemoveAllListeners();
            int index = i;
            btn.onClick.AddListener(() =>
            {
                CheckAnswer(index);
            });
        }

        timer = questionTime;
        isCountingDown = true;
    }

    void CheckAnswer(int index)
    {
        isCountingDown = false;

        if (index == questions[currentQuestionIndex].correctAnswerIndex)
        {
            score++;
            Debug.Log("Correct!");
            if (correctSound != null)
                audioSource.PlayOneShot(correctSound);
        }
        else
        {
            Debug.Log("Wrong!");
            if (wrongSound != null)
                audioSource.PlayOneShot(wrongSound);
        }

        currentQuestionIndex++;
        DisplayQuestion();
    }

    void EndQuiz()
    {
        timerText.gameObject.SetActive(false);
        questionImage.gameObject.SetActive(false);
        questionText.gameObject.SetActive(false);

        Debug.Log("Quiz Finished! Score: " + score + "/" + questions.Count);

        ScoreText.text = $"{score}/{questions.Count}";
        ScoreText.gameObject.SetActive(true);

        // Check if the current level is level 3
        if (currentLevelIndex == 3)
        {
            if (score == questions.Count)
            {
                int nextLevel = currentLevelIndex + 1;
                Debug.Log($"Player aced the quiz. Unlocking next level: {nextLevel}");
                LevelProgress.UnlockLevel(nextLevel);

                selectLevelBtn2.SetActive(true);
                nextLevelButton.gameObject.SetActive(false);
                retryButton.gameObject.SetActive(false);
            }
            else
            {
                selectLevelBtn1.SetActive(true);
                nextLevelButton.gameObject.SetActive(false);
                retryButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (score == questions.Count)
            {
                int nextLevel = currentLevelIndex + 1;
                Debug.Log($"Player aced the quiz. Unlocking next level: {nextLevel}");
                LevelProgress.UnlockLevel(nextLevel);

                selectLevelBtn2.SetActive(true);
                nextLevelButton.gameObject.SetActive(true);
                retryButton.gameObject.SetActive(false);
            }
            else
            {
                selectLevelBtn1.SetActive(true);
                nextLevelButton.gameObject.SetActive(false);
                retryButton.gameObject.SetActive(true);
            }
        }

        foreach (var btn in answerButtons)
        {
            btn.SetActive(false);
        }

        ScoreCanvas.SetActive(true);
    }

    private void Update()
    {
        if (!isCountingDown) return;

        timer -= Time.deltaTime;
        timerText.text = Mathf.CeilToInt(timer).ToString();

        if (timer <= 0f)
        {
            isCountingDown = false;
            CheckAnswer(-1); // Time's up
        }
    }
}
