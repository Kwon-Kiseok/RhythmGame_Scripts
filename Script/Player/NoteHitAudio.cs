using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHitAudio : MonoBehaviour
{
    public AudioClip[] hitSounds;

    public AudioClip PlayHitSound()
    {
        return hitSounds[Random.Range(0, hitSounds.Length)];
    }
}
