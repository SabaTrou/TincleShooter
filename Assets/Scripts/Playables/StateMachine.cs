using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "StateMachine", menuName = "Custom/StateMachine")]
public class StateMachine : ScriptableObject
{
    public State[] states;
    public string initialState; // 初期ステート名
}