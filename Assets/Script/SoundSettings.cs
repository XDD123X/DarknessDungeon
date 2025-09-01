using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettings : MonoBehaviour
{
    public AudioClip EquipItemSFX;
    public AudioClip UnequipItemSFX;
    public AudioClip OpenCharacter;
    public AudioClip HoverSlot;

    public AudioClip SwordSlash;
    public AudioClip BowAttack;
    public AudioClip StaffAttack;

    public AudioClip PlayerBeingAttacked;
    public AudioClip EnemyBeingAttacked;

    private AudioSource source;

    public void Start()
    {
        source = GetComponent<AudioSource>();
        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetFloat("Music", 1f);
            PlayerPrefs.Save();
        }
    }

    private void FixedUpdate()
    {
        source.volume = PlayerPrefs.GetFloat("Music");
    }

    
}
