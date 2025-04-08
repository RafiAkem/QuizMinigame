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
}

public class AnswerLogic : MonoBehaviour
{
    [SerializeField] private Transform buttonField;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Image questionImage;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip answerClickSound;

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

        for (int i = 0; i < answerButtons.Count; i++)
        {
            Button btn = answerButtons[i].GetComponent<Button>();
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();

            btnText.text = q.options[i];
            btn.onClick.RemoveAllListeners();

            int index = i;
            btn.onClick.AddListener(() => {
            audioSource.PlayOneShot(answerClickSound); 
            CheckAnswer(index); 
});
        }
    }

    void CheckAnswer(int index)
    {
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
        Debug.Log("Quiz Finished! Score: " + score + "/" + questions.Count);
        questionText.text = $"Finished! Score: {score}/{questions.Count}";

        foreach (var btn in answerButtons)
        {
            btn.SetActive(false);
        }
    }
}
