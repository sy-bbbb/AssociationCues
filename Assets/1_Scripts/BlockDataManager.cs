using UnityEngine;
using System.Collections.Generic;

public struct ObjectTransform
{
    public Vector3 Position;
    public Quaternion Rotation;
}

public struct RootTransform
{
    public Vector3 Position;
    public Vector3 Scale;
}

public class TaskData
{
    public List<List<string>> BlockPrefabNames;
    public RootTransform TaskRootTransform;
}


public class BlockDataManager : MonoBehaviour
{
    public static BlockDataManager Instance { get; private set; }

    private Dictionary<StudySettings.Task, TaskData> allTaskData;
    private List<ObjectTransform> prefabTransforms;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            InitializeTaskData();
            InitializePrefabTransforms();
        }
    }

    public RootTransform GetRootTransformForTask(StudySettings.Task task)
    {
        if (allTaskData.TryGetValue(task, out TaskData data))
            return data.TaskRootTransform;
        return new RootTransform { Position = Vector3.zero, Scale = Vector3.one }; // Return default
    }

    public List<string> GetPrefabNamesForBlock(StudySettings.Task task, int blockId)
    {
        if (allTaskData.TryGetValue(task, out TaskData data))
        {
            int blockIndex = blockId - 1;
            if (blockIndex >= 0 && blockIndex < data.BlockPrefabNames.Count)
                return data.BlockPrefabNames[blockIndex];
        }
        return null;
    }

    public List<ObjectTransform> GetPrefabTransforms() => prefabTransforms;

    private void InitializeTaskData()
    {
        allTaskData = new Dictionary<StudySettings.Task, TaskData>
        {
            {
                StudySettings.Task.task1, new TaskData
                {
                    TaskRootTransform = new RootTransform { Position = new Vector3 (0f, -0.2f, 2f), Scale = Vector3.one },
                    BlockPrefabNames = new List<List<string>>
                    {
                        new List<string> { "OrangeClownfish", "PurpleTang", "FlameAngelfish", "NapoleonWrasse", "Boxfish", "Bannerfish" },
                        new List<string> { "YellowTang", "LinedSurgeon", "AngelfishBlueface", "Threadfin", "BlackDurgon", "SunfishMolaMola" },
                        new List<string> { "ClownTriggerfish", "BlueTang", "AngelfishMagestic", "BanggaiCardinalfish", "ClownfishBlack", "Discus" },
                        new List<string> { "EmperorAngelfish", "RoyalAngelfish", "ScrawledFilefish", "AngelfishMultibarred", "AngelfishQueen", "GobyNemateleotris" },
                    },

                }
            },
            {
                StudySettings.Task.task2, new TaskData
                {
                    TaskRootTransform = new RootTransform { Position =  new Vector3 (0f, -0.2f, 2f), Scale = Vector3.one * 0.5f },
                    BlockPrefabNames = new List<List<string>>
                    {
                        new List<string> { "BlackDurgon", "FlameAngelfish", "AngelfishMultibarred", "Threadfin", "Bannerfish", "ClownfishBlack" },
                        new List<string> { "SunfishMolaMola", "YellowTang", "BlueTang", "EmperorAngelfish", "Boxfish", "RoyalAngelfish" },
                        new List<string> { "OrangeClownfish", "PurpleTang", "GobyNemateleotris", "BanggaiCardinalfish", "LinedSurgeon", "Discus" },
                        new List<string> { "ScrawledFilefish", "NapoleonWrasse", "AngelfishBlueface", "AngelfishMagestic", "ClownTriggerfish", "AngelfishQueen" },
                    },
                }
            }
        };
    }

    private void InitializePrefabTransforms()
    {
        prefabTransforms = new List<ObjectTransform>
        {
            new ObjectTransform { Position = new Vector3(-0.655f, 0.02f, -0.962f), Rotation = new Quaternion(0.0f, -0.1305f, 0.0f, 0.9914f) },
            new ObjectTransform { Position = new Vector3(0.009f, 0.0f, -0.510f), Rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f) },
            new ObjectTransform { Position = new Vector3(1.058f, 0.0f, -0.642f), Rotation = new Quaternion(0.0f, 0.2816f, 0.0f, 0.9595f) },
            new ObjectTransform { Position = new Vector3(0.849f, 0.0f, -1.747f), Rotation = new Quaternion(0.0f, 0.983f, 0.0f, 0.185f) },
            new ObjectTransform { Position = new Vector3(0.0f, -0.059f, -2.126f), Rotation = new Quaternion(0.0f, 1.0f, 0.0f, 0.0f) },
            new ObjectTransform { Position = new Vector3(-0.692f, 0.0f, -1.745f), Rotation = new Quaternion(0.0f, 0.9786f, 0.0f, -0.2058f) }
        };
    }
}

