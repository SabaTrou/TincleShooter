using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Audio;

public class CustomAnimator : MonoBehaviour
{
    public StateMachine stateMachine; // ステートマシン
    public Animator animator; // アニメーター
    public AudioSource audioSource; // オーディオソース

    private PlayableGraph graph; // PlayableGraph
    private Dictionary<string, State> stateDictionary; // ステートの辞書
    private Dictionary<string, float> parameters = new Dictionary<string, float>(); // パラメータ辞書

    private string currentState; // 現在のステート

    void Start()
    {
        InitializeStateMachine();
        PlayInitialState();
    }

    void Update()
    {
        EvaluateTransitions();
    }

    void InitializeStateMachine()
    {
        stateDictionary = new Dictionary<string, State>();
        foreach (var state in stateMachine.states)
        {
            stateDictionary[state.name] = state;
        }
        currentState = stateMachine.initialState;
    }

    void PlayInitialState()
    {
        graph = PlayableGraph.Create("CustomAnimatorGraph");
        PlayState(currentState);
    }

    void PlayState(string stateName)
    {
        if (!stateDictionary.ContainsKey(stateName)) return;

        var state = stateDictionary[stateName];
        graph.Destroy();

        graph = PlayableGraph.Create($"State_{stateName}");
        var mixer = AnimationMixerPlayable.Create(graph, 2);

        if (state.animationClip != null)
        {
            var animationPlayable = AnimationClipPlayable.Create(graph, state.animationClip);
            graph.Connect(animationPlayable, 0, mixer, 0);
        }

        if (state.audioClip != null)
        {
            var audioPlayable = AudioClipPlayable.Create(graph, state.audioClip, false);
            graph.Connect(audioPlayable, 0, mixer, 1);
        }

        var animationOutput = AnimationPlayableOutput.Create(graph, "AnimationOutput", animator);
        animationOutput.SetSourcePlayable(mixer);

        var audioOutput = AudioPlayableOutput.Create(graph, "AudioOutput", audioSource);
        audioOutput.SetSourcePlayable(mixer);

        graph.Play();
        currentState = stateName;
    }

    void EvaluateTransitions()
    {
        var state = stateDictionary[currentState];
        foreach (var transition in state.transitions)
        {
            if (EvaluateCondition(transition.condition))
            {
                PlayState(transition.targetState);
                break;
            }
        }
    }

    bool EvaluateCondition(Condition condition)
    {
        if (!parameters.ContainsKey(condition.parameterName)) return false;

        float value = parameters[condition.parameterName];
        switch (condition.conditionType)
        {
            case ConditionType.Greater: return value > condition.threshold;
            case ConditionType.Less: return value < condition.threshold;
            case ConditionType.Equals: return Mathf.Approximately(value, condition.threshold);
            case ConditionType.NotEquals: return !Mathf.Approximately(value, condition.threshold);
            default: return false;
        }
    }

    public void SetParameter(string name, float value)
    {
        parameters[name] = value;
    }
}