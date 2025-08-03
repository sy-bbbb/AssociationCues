using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    [Header("External Managers")]
    [SerializeField] private TaskManager taskManager;
    
    [Header("Quiz Content")]
    [SerializeField] private List<Question> questions = new List<Question>();

    [Header("Button References")]
    [SerializeField] private ButtonConfigHelper[] buttonHelpers = new ButtonConfigHelper[4];
    [SerializeField] private TextMeshPro questionLabel;
    private int currentQuestionIndex = 0;

    void ShowQuestion(int index)
    {
        if (index >= questions.Count)
        {
            questionLabel.text = "ÄûÁî Á¾·á!";
            StudyLogger.Instance.StopLogging();
            taskManager.DestroySceneObjects();
            foreach (var btn in buttonHelpers)
            {
                btn.MainLabelText = "";
                var interactable = btn.GetComponent<Interactable>();
                interactable.IsEnabled = false;
                interactable.OnClick.RemoveAllListeners();
            }
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
