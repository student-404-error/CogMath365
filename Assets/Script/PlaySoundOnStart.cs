using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
 
    public void Start()
    {
        SoundManager.Instance.PlaySound(_clip);
    }
}
