using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class TaskManager : MonoBehaviour
{
    [Header("Study Settings")]
    [SerializeField] private string participantID = "P01";
    [SerializeField] private StudySettings.Task currentTask = StudySettings.Task.task1;
    [SerializeField] private int blockID = 1;
    [SerializeField] private StudySettings.Condition currentCondition = StudySettings.Condition.Proximity;

    [Header("Component References")]
    [SerializeField] private ConditionManager conditionManager;
    [SerializeField] private PhoneLabelHandler phoneLabelHandler;
    [SerializeField] private QuizRemoteLoader quizRemoteLoader;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject quizPanel;

    [Header("Scene Setup")]
    [SerializeField] private Transform sceneObjectRoot;
    [SerializeField] private string prefabsResourcePath = "SceneObjects";

    // --- Public properties ---
    public StudySettings.Task CurrentTask => currentTask;
    public int BlockID => blockID;
    public List<string> CurrentBlockPrefabNames => BlockDataManager.Instance.GetPrefabNamesForBlock(currentTask, blockID);


    // --- Private State ---
    private List<GameObject> sceneObjects = new List<GameObject>();
    private bool isPhoneConnected = false;
    private bool areLabelsLoaded = false;
    private bool areQuizzesLoaded = false;

    void Start()
    {
        InitialisePointers();
        InitialiseUI();
        BeginPreloading();
    }

    private void InitialisePointers()
    {
        PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);
        PointerUtils.SetGazePointerBehavior(PointerBehavior.AlwaysOff);
    }
    
    private void InitialiseUI()
    {
        if (startButton != null)
            startButton.GetComponent<Interactable>().IsEnabled = false;
        if (quizPanel != null)
            quizPanel.SetActive(false);
    }

    private void BeginPreloading()
    {
        phoneLabelHandler.StartFetchingLabels();
        quizRemoteLoader.StartFetchingQuizzes();
    }

    public void ReportPhoneConnected()
    {
        isPhoneConnected = true;
        Debug.Log("Phone connection ready.");
        CheckAllReady();
    }
    public void ReportLabelsLoaded()
    {
        areLabelsLoaded = true;
        Debug.Log("Label contents ready.");
        CheckAllReady();
    }

    public void ReportQuizzesLoaded()
    {
        areQuizzesLoaded = true;
        Debug.Log("Quiz questions ready.");
        CheckAllReady();
    }

    private void CheckAllReady()
    {
        bool allSystemsReady = isPhoneConnected && areLabelsLoaded && areQuizzesLoaded;
        if (allSystemsReady && startButton != null)
            startButton.GetComponent<Interactable>().IsEnabled = true;
    }

    public void StartExperiment()
    {
        StudyLogger.Instance.StartLogging(participantID, currentTask.ToString(), currentCondition.ToString(), blockID.ToString());
        AssignSceneObjects();
        conditionManager.Initialise(currentCondition, currentTask, blockID, sceneObjects);
        if (startButton != null)
            startButton.SetActive(false);
        if (quizPanel != null)
            quizPanel.SetActive(true);
    }

    private void AssignSceneObjects()
    {
        sceneObjects.Clear();

        RootTransform rootTransform = BlockDataManager.Instance.GetRootTransformForTask(currentTask);
        sceneObjectRoot.localPosition = rootTransform.Position;
        sceneObjectRoot.localScale = rootTransform.Scale;

        List<string> requiredNames = BlockDataManager.Instance.GetPrefabNamesForBlock(currentTask, blockID);
        List<ObjectTransform> prefabTransforms = BlockDataManager.Instance.GetPrefabTransforms();

        if (requiredNames == null || prefabTransforms == null)
        {
            Debug.LogError($"Data for Task '{currentTask}' and Block ID '{blockID}' not defined in BlockDataManager.");
            return;
        }

        var prefabMap = Resources.LoadAll<GameObject>(prefabsResourcePath)
                                 .ToDictionary(prefab => prefab.name, prefab => prefab);

        for (int i = 0; i < requiredNames.Count; i++)
        {
            string nameToFind = requiredNames[i];
            ObjectTransform objectTransform = prefabTransforms[i];

            if (prefabMap.TryGetValue(nameToFind, out GameObject prefabToSpawn))
            {
                GameObject newObject = Instantiate(prefabToSpawn, sceneObjectRoot);
                newObject.transform.localPosition = objectTransform.Position;
                newObject.transform.localRotation = objectTransform.Rotation;
                newObject.name = prefabToSpawn.name;
                sceneObjects.Add(newObject);
            }
            else
                Debug.LogWarning($"'{nameToFind}' not found in 'Resources/{prefabsResourcePath}'.");
        }
    }

    public void DestroySceneObjects() => Destroy(sceneObjectRoot.gameObject);
}