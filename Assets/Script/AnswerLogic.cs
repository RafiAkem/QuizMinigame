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
    [SerializeField] private AudioClip answerClickSound;
    [SerializeField] private float questionTime = 10f;
    [SerializeField] private TextMeshProUGUI timerText;

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

        // Ambil Image dan text dari prefab
        TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>(true);
        Image answerImage = answerButtons[i].transform.Find("AnswerImage").GetComponent<Image>();

        btnText.gameObject.SetActive(false);
        answerImage.gameObject.SetActive(false);

    // Cek apakah question pakai gambar
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
        audioSource.PlayOneShot(answerClickSound);
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
        }
        else
        {
            Debug.Log("Wrong!");
        }

        currentQuestionIndex++;
        DisplayQuestion();
    }

    void EndQuiz()
    {
        timerText.gameObject.SetActive(false);
        questionImage.gameObject.SetActive(false);

        Debug.Log("Quiz Finished! Score: " + score + "/" + questions.Count);
        questionText.text = $"Finished! Score: {score}/{questions.Count}";

        foreach (var btn in answerButtons)
        {
            btn.SetActive(false);
        }
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
