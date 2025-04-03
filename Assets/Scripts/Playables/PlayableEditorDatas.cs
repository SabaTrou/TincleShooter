using UnityEngine;

[System.Serializable]
public class Condition
{
    public string parameterName; // パラメータ名
    public ConditionType conditionType; // 条件タイプ
    public float threshold; // しきい値
}

public enum ConditionType { Greater, Less, Equals, NotEquals }

[System.Serializable]
public class Transition
{
    public string targetState; // 遷移先のステート名
    public Condition condition; // 遷移条件
}

[System.Serializable]
public class State
{
    public string name; // ステート名
    public AnimationClip animationClip; // アニメーションクリップ
    public AudioClip audioClip; // オーディオクリップ
    public Transition[] transitions; // 遷移のリスト
    public Vector2 position; // ノードの位置
}

