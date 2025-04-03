using UnityEngine;

[System.Serializable]
public class Condition
{
    public string parameterName; // �p�����[�^��
    public ConditionType conditionType; // �����^�C�v
    public float threshold; // �������l
}

public enum ConditionType { Greater, Less, Equals, NotEquals }

[System.Serializable]
public class Transition
{
    public string targetState; // �J�ڐ�̃X�e�[�g��
    public Condition condition; // �J�ڏ���
}

[System.Serializable]
public class State
{
    public string name; // �X�e�[�g��
    public AnimationClip animationClip; // �A�j���[�V�����N���b�v
    public AudioClip audioClip; // �I�[�f�B�I�N���b�v
    public Transition[] transitions; // �J�ڂ̃��X�g
    public Vector2 position; // �m�[�h�̈ʒu
}

