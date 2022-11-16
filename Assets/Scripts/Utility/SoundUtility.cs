using UnityEngine;
public static class SoundUtility
{
    public static void PlayAudio(AudioClip clip, Vector3? position = null, bool global = false)
    {
        position ??= Vector3.zero;
        var audioSource = new GameObject($"OneShot - {clip.name}", typeof(AudioSource)).GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        if(global) audioSource.minDistance = audioSource.maxDistance = float.MaxValue;
        GameObject.Destroy(audioSource.gameObject, clip.length);
    }
}
