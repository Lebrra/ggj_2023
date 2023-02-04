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

    [SerializeField]
    private AudioSource[] musicSourceArray;
    [SerializeField]
    
    //private LumberjackAudioMgr lumberjackAudioMgr;
    
    
    int toggle = 0;

    double nextStartTime;
    double playDuration; // duration of playing sound

    [SerializeField]
    double durationOffset = -0.07; //HACK: Don't know what the computed sample duration is too long!
   
    [SerializeField]
    private List<AudioClip> myMusicLoops;

    [SerializeField]
    private List<AudioClip> mySfxs;

    [SerializeField]
    private int currentClipNumber = 0; // Defaults to first Soundclip
    [SerializeField]
    private int nextClipNumber = 0; // Defaults to first SoundClip
    [SerializeField]
    int sfxId = 0;
    [SerializeField]
    bool startLoops;

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
            
            //lumberjackAudioMgr = gameObject.AddComponent<LumberjackAudioMgr>();            
            
            ConfigureMusicSource(toggle);
            ConfigureMusicSource(1-toggle);
            durationOffset = -0.07; // HACK
            SetMusicClip(0);
            PlayMusic();
        }
    }

    private void ConfigureMusicSource(int id)
    {
        if (id < musicSourceArray.Length)
        {
            musicSourceArray[id].loop = false;
            musicSourceArray[id].priority = 0;
            musicSourceArray[id].volume = 1;
        }
    }

    // clipNumber corresponds to the element id 0 based
    public void SetMusicClip(int clipNumber)
    {
        if ((clipNumber < myMusicLoops.Count) && (clipNumber != currentClipNumber))
        {
            nextClipNumber = clipNumber;
        }
    }
/*    
    public void PlaySFX(int id, bool loop = false)
    {
        bool didPlay = false;
        if (id < mySfxs.Count)
        {
            for (int source = 0; source < sfxSourceArray.Length; source++ )
            {
                if (!sfxSourceArray[source].isPlaying)
                {
                    if (!loop)
                    {
                        sfxSourceArray[source].loop = false;
                        sfxSourceArray[source].PlayOneShot(mySfxs[id]);
                    }
                    else
                    {
                        sfxSourceArray[source].loop = true;
                        sfxSourceArray[source].clip = mySfxs[id];
                        sfxSourceArray[source].Play();
                    }
                    didPlay = true;
                    break;
                }           
            }
            if (!didPlay)
            {
                // what happens if they're all playing?
                Debug.LogWarning("Out of sound channels");
            }            
        }
    }
    */


    public void PlayMusic()
    {
        if (!musicSourceArray[toggle].isPlaying)    
        {            
            // Schedule start and stop time of a clip
            double myDspTime = AudioSettings.dspTime;
            musicSourceArray[toggle].PlayScheduled(myDspTime+0.01);
            playDuration = (double)myMusicLoops[currentClipNumber].samples / myMusicLoops[currentClipNumber].frequency;
            nextStartTime = myDspTime + playDuration + durationOffset;
        }
    }

    public void StopMusic()
    {
        musicSourceArray[toggle].Stop();
        musicSourceArray[1-toggle].Stop();
    }

    // See if it's time to loop the current clip or a new clip
    void Update()
    {
        // if you're almost done playing current clip
        if (AudioSettings.dspTime > nextStartTime - 1) // TODO: THis could be shorter than 1 for efficiency
        {
            AudioClip clipToPlay = myMusicLoops[nextClipNumber];
            // Loads the next Clip to play and schedules when it will start
            musicSourceArray[toggle].clip = clipToPlay;
            musicSourceArray[toggle].PlayScheduled(nextStartTime);
            // Checks how long the Clip will last and updates the Next Start Time with a new value
            double duration = (double)clipToPlay.samples / clipToPlay.frequency;
            nextStartTime = nextStartTime + duration + durationOffset;
            // Switches the toggle to use the other Audio Source next
            toggle = 1 - toggle;
        }
        // TESTING ONLY
        if (sfxId > 0)
        {
            //PlaySFX(sfxId - 1, false);
        }
        if (startLoops)
        {
            //PlaySFX(3, true);
        }
    }

}
