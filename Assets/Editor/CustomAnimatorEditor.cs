using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomAnimator))]
public class CustomAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CustomAnimator animator = (CustomAnimator)target;

        if (GUILayout.Button("Set Parameter (Example)"))
        {
            animator.SetParameter("Speed", 1.0f);
        }
    }
}