using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class CustomAnimatorWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private StateMachine currentStateMachine; // 編集対象のステートマシン
    private Vector2 mousePosition; // マウス位置
    private State selectedState; // 選択されたステート
    private Transition selectedTransition; // 選択された遷移

    [MenuItem("Window/Custom Animator Window")]
    public static void OpenWindow()
    {
        GetWindow<CustomAnimatorWindow>("Custom Animator");
    }

    private void OnGUI()
    {
        DrawToolbar();

        if (currentStateMachine == null)
        {
            EditorGUILayout.HelpBox("No StateMachine selected. Please select one in the toolbar.", MessageType.Info);
            return;
        }

        // スクロール可能エリア
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        DrawNodes();
        DrawConnections();

        EditorGUILayout.EndScrollView();

        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            ShowContextMenu();
        }

        ProcessNodeDragging();
    }

    private void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        // ScriptableObjectを選択するObjectField
        currentStateMachine = (StateMachine)EditorGUILayout.ObjectField(
            "StateMachine", currentStateMachine, typeof(StateMachine), false);

        EditorGUILayout.EndHorizontal();
    }

    private void DrawNodes()
    {
        if (currentStateMachine == null) return;

        foreach (var state in currentStateMachine.states)
        {
            Rect nodeRect = new Rect(state.position, new Vector2(150, 50));
            GUI.Box(nodeRect, state.name);

            if (Event.current.type == EventType.MouseDown && nodeRect.Contains(Event.current.mousePosition))
            {
                selectedState = state;
                Event.current.Use();
            }
        }
    }

    private void DrawConnections()
    {
        if (currentStateMachine == null) return;

        foreach (var state in currentStateMachine.states)
        {
            if(state.transitions==null)
            {
                continue;
            }
            foreach (var transition in state.transitions)
            {
                var targetState = FindStateByName(transition.targetState);
                if (targetState != null)
                {
                    Handles.DrawLine(state.position + new Vector2(75, 25), targetState.position + new Vector2(75, 25));
                }
            }
        }
    }

    private void ShowContextMenu()
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add State"), false, AddState);
        if (selectedState != null)
        {
            menu.AddItem(new GUIContent("Delete State"), false, DeleteState);
        }
        menu.ShowAsContext();
    }

    private void AddState()
    {
        if (currentStateMachine == null) return;

        State newState = new State
        {
            name = "New State",
            position = mousePosition
        };

        ArrayUtility.Add(ref currentStateMachine.states, newState);
        EditorUtility.SetDirty(currentStateMachine);
    }

    private void DeleteState()
    {
        if (currentStateMachine == null || selectedState == null) return;

        ArrayUtility.Remove(ref currentStateMachine.states, selectedState);
        selectedState = null;
        EditorUtility.SetDirty(currentStateMachine);
    }

    private void ProcessNodeDragging()
    {
        if (selectedState == null || Event.current.type != EventType.MouseDrag) return;

        selectedState.position += Event.current.delta;
        Event.current.Use();
        EditorUtility.SetDirty(currentStateMachine);
    }

    private State FindStateByName(string name)
    {
        foreach (var state in currentStateMachine.states)
        {
            if (state.name == name) return state;
        }
        return null;
    }
}
#endif