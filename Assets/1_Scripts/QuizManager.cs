using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    [Header("External Managers")]
    private TaskManager taskManager;
    
    [Header("Quiz Content")]
    [SerializeField] private List<Question> questions = new List<Question>();

    [Header("Button References")]
    [SerializeField] private ButtonConfigHelper[] buttonHelpers = new ButtonConfigHelper[4];
    [SerializeField] private TextMeshPro questionLabel;
    private int currentQuestionIndex = 0;

    private void Start()
    {
        taskManager = GetComponent<TaskManager>();
    }

    void ShowQuestion(int index)
    {
        if (index >= questions.Count)
        {
            foreach (var btn in buttonHelpers)
            {
                btn.MainLabelText = "";
                var interactable = btn.GetComponent<Interactable>();
                interactable.IsEnabled = false;
                interactable.OnClick.RemoveAllListeners();
            }
            questionLabel.text = "ÄûÁî Á¾·á!";
            StudyLogger.Instance.StopLogging();
            Debug.Log(taskManager);
            taskManager.DestroySceneObjects();
            return;
        }

        Question q = questions[index];
        questionLabel.text = q.questionText;

        for (int i = 0; i < buttonHelpers.Length; i++)
        {
            int choiceIndex = i;
            buttonHelpers[i].MainLabelText = q.options[i];
            var interactable = buttonHelpers[i].GetComponent<Interactable>();

            interactable.OnClick.RemoveAllListeners();
            interactable.OnClick.AddListener(() => OnAnswerSelected(choiceIndex));
        }
    }

    void OnAnswerSelected(int selectedIndex)
    {
        var correctIndex = questions[currentQuestionIndex].correctIndex;

        StudyLogger.Instance.LogQuizAnswer(currentQuestionIndex.ToString(), selectedIndex.ToString(), correctIndex == selectedIndex);

        currentQuestionIndex++;
        ShowQuestion(currentQuestionIndex);

    }

    public void InitializeQuiz(List<Question> loadedQuestions)
    {
        this.questions = loadedQuestions;
        currentQuestionIndex = 0;
        ShowQuestion(currentQuestionIndex);
    }

}
