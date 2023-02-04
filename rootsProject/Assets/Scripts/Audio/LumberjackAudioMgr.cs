using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberjackAudioMgr : MonoBehaviour
{
    public const int MAX_AUDIO_SOURCES = 16;
    public static LumberjackAudioMgr instance;

    public enum LumbejackState {idling, walking, chopping, dead};
    public enum Sounds {chopping = 0, walking = 1};
    [SerializeField]
    private List<AudioClip> mySfxs;
    Dictionary<int, AudioSource> Sources;

    Dictionary<int, Lumberjack> LJStates = new Dictionary<int, Lumberjack>();

    private void Awake() {
        if (instance != null) 
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Allocate AudioSource
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void PlaySound(AudioSource source, Sounds soundIndex)
    {
        source.clip = mySfxs[(int)soundIndex];
        source.loop = true;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // scan list
        if (LJStates.Count == 0) return;

        foreach (KeyValuePair<int,Lumberjack> entry in LJStates)
        {
            LumberjackAudioMgr.LumbejackState _curr = entry.Value.current;
            LumberjackAudioMgr.LumbejackState _next = entry.Value.next;
            AudioSource _source = Sources[entry.Key];

            if (_curr != _next)
            {
                if (_curr == LumbejackState.idling)
                {
                    if (_next == LumbejackState.chopping)
                    { 
                        PlaySound(_source, Sounds.chopping);
                    }
                    else if (_next == LumbejackState.walking)
                    {
                        PlaySound(_source, Sounds.walking);
                    }
                }
                else if (_curr == LumbejackState.walking)
                {
                    if (_next == LumbejackState.chopping)
                    {
                        PlaySound(_source, Sounds.chopping);
                    }
                    else if (_next == LumbejackState.idling)
                    {
                        _source.Stop();
                    }
                }
                else if (_curr == LumbejackState.chopping)
                {
                    if (_next == LumbejackState.walking)
                    {
                        PlaySound(_source, Sounds.walking);
                    }
                    else if (_next == LumbejackState.idling)
                    {
                        _source.Stop();
                    }
                }
                _curr = _next;
            }
        }
    }
    public int RegisterLumberjack(LumbejackState initialState = LumbejackState.idling)
    {
        int _tempId = LJStates.Count + 1;
        Lumberjack _lj = new Lumberjack(_tempId);
        if (!LJStates.ContainsKey(_tempId))
            LJStates.Add(_tempId, _lj);

        //Add an AudioSource to this object
        Sources.Add(_tempId, gameObject.AddComponent<AudioSource>());

        return _tempId;
    }

    public void SetLumberjackState(int id, LumberjackAudioMgr.LumbejackState state)
    {
        if(LJStates.ContainsKey(id))
        {
           Lumberjack _lj = LJStates[id];
           if (_lj.current != _lj.next)
           {
                _lj.next = state;
           }
        }
    }
}

public class Lumberjack 
{
    public Lumberjack( int id, LumberjackAudioMgr.LumbejackState initialState = LumberjackAudioMgr.LumbejackState.idling  ) 
    {
        current = next = initialState;        
    }
    public int id;
    public LumberjackAudioMgr.LumbejackState current;
    public LumberjackAudioMgr.LumbejackState next;
}