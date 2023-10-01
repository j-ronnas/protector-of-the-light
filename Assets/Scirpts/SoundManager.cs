using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    [SerializeField]
    SoundEffect[] effects;

    Dictionary<string, GameObject> soundEffects;

    public static SoundManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        soundEffects = new Dictionary<string, GameObject>();
        foreach (SoundEffect se in effects)
        {
            soundEffects.Add(se.name, se.soundPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Play(string soundName)
    {
        if(soundEffects.ContainsKey(soundName))
        {
            Instantiate(soundEffects[soundName], transform);
        }
    }
}

[Serializable]
public struct SoundEffect
{
    public string name;
    public GameObject soundPrefab;
}
