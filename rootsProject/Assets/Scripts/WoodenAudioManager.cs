using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    Looped Sample Sequencer - 
    - specify a SoundClip to loop. 
    - new Soundclips will loop when current SoundClip completes
*/
public class WoodenAudioManager : MonoBehaviour
{
    public static WoodenAudioManager instance;

    private AudioSource[] audioSourceArray;
    int toggle = 0;

    double nextStartTime;
    double playDuration; // duration of playing sound

    [SerializeField]
    double durationOffset = -0.07; //HACK: Don't know what the computed sample duration is too long!

    [SerializeField]
    private List<AudioClip> mySoundClips;

    [SerializeField]
    private int currentClipNumber = 0; // Defaults to first Soundclip
    [SerializeField]
    private int nextClipNumber = 0; // Defaults to first SoundClip

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake() {
        if (instance != null) 
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //currentAudioSource = GetComponent<AudioSource>();
            audioSourceArray = GetComponents<AudioSource>();
            ConfigureAudioSources(toggle);
            ConfigureAudioSources(1-toggle);
            durationOffset = -0.07; // HACK
            PlayMusic();
        }
    }

    private void ConfigureAudioSources(int id)
    {
        audioSourceArray[id].loop = false;
        audioSourceArray[id].priority = 0;
        audioSourceArray[id].volume = 1;
    }

    // clipNumber corresponds to the element id 0 based
    public void SetClip(int clipNumber)
    {
        if ((clipNumber < mySoundClips.Count) && (clipNumber != currentClipNumber))
        {
            nextClipNumber = clipNumber;
        }
    }
    
    public void PlayMusic()
    {
        if (!audioSourceArray[toggle].isPlaying)    
        {            
            // Schedule start and stop time of a clip
            double myDspTime = AudioSettings.dspTime;
            audioSourceArray[toggle].PlayScheduled(myDspTime);
            playDuration = (double)mySoundClips[currentClipNumber].samples / mySoundClips[currentClipNumber].frequency;
            nextStartTime = myDspTime + playDuration + durationOffset;
        }
    }

    public void StopMusic()
    {
        audioSourceArray[toggle].Stop();
        audioSourceArray[1-toggle].Stop();
    }

    // See if it's time to loop the current clip or a new clip
    void Update()
    {
        // if you're almost done playing current clip
        if (AudioSettings.dspTime > nextStartTime - 1) // TODO: THis could be shorter than 1 for efficiency
        {
            AudioClip clipToPlay = mySoundClips[nextClipNumber];
            // Loads the next Clip to play and schedules when it will start
            audioSourceArray[toggle].clip = clipToPlay;
            audioSourceArray[toggle].PlayScheduled(nextStartTime);
            // Checks how long the Clip will last and updates the Next Start Time with a new value
            double duration = (double)clipToPlay.samples / clipToPlay.frequency;
            nextStartTime = nextStartTime + duration + durationOffset;
            // Switches the toggle to use the other Audio Source next
            toggle = 1 - toggle;
        }
    }

}
