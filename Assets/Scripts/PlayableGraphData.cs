using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "PlayableGraphData", menuName = "Custom/PlayableGraphData")]
public class PlayableGraphData : ScriptableObject
{
    [System.Serializable]
    public class PlayableNode
    {
        public AnimationClip animationClip;
        public AudioClip audioClip;
    }

    public List<PlayableNode> nodes = new List<PlayableNode>();

    public void BuildGraph(PlayableGraph graph, Animator animator, AudioSource audioSource)
    {
        var mixer = AnimationMixerPlayable.Create(graph, nodes.Count);

        for (int i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            if (node.animationClip != null)
            {
                var animPlayable = AnimationClipPlayable.Create(graph, node.animationClip);
                graph.Connect(animPlayable, 0, mixer, i);
            }
            if (node.audioClip != null)
            {
                var audioPlayable = AudioClipPlayable.Create(graph, node.audioClip, false);
                var audioOutput = AudioPlayableOutput.Create(graph, "AudioOutput", audioSource);
                audioOutput.SetSourcePlayable(audioPlayable);
            }
        }

        var animationOutput = AnimationPlayableOutput.Create(graph, "AnimationOutput", animator);
        animationOutput.SetSourcePlayable(mixer);
        graph.Play();
    }
}