using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;    //Unity5 new audio API

/// <summary>
/// New audio manager class. Common Sound Database and Music Player.
/// Handles the playing of sounds at a GameObject or a point in space.
/// 
/// </summary>
public class SoundMaster : MonoBehaviour
{

    public AudioMixer soundMixer;
    private AudioMixerGroup[] _soundChannels;
    /// <summary>
    /// Sound List: --
    /// </summary>
    [Tooltip("Common Sounds such as UI and effects")]
    public AudioClip[] soundDB;
    [Tooltip("Pool of AudioSources.")]
    public AudioSource[] soundPlayers;
    /// <summary>Index of SoundPlayer[]</summary>
    private int _soundPlayer;

    public AudioMixerSnapshot[] snapshots;
    [Tooltip("Which snapshots are active [1,0]. Array needs to be same length as snapshots above")]
    public float[] weights;
    private float[] _defaultSnapshotWeights;

    private AudioListener _listener;
    /// <summary>
    /// Where the sounds are heard, Usually on Main Camera. 
    /// When new scene loads, it should be able to find and cache the AudioListener. 
    /// Probably most common use will be to find position of player to play 3D sounds at or nearby.
    /// </summary>
    public AudioListener Listener
    {
        get
        {
            if (_listener != null) 
                return _listener;
            else
            {
                Debug.Log("Looking For Main Camera to get AudioListener. TODO: Make this look for AudioListener through multiple objects.");
                GameObject newListener = GameObject.Find("Main Camera");
                _listener = newListener.GetComponent<AudioListener>();
                if (_listener == null)
                    Debug.Log("Main Camera doesn't have AudioListener component! Errors incoming");
                return _listener;
            }
        }
    }
    
    public enum SoundType
    {
        noType,
        _3dSound,
        _2dSound,
        _UISound
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (soundMixer == null) soundMixer = Resources.Load("Sounds") as AudioMixer;
        _soundChannels = soundMixer.FindMatchingGroups("Master/");
        _defaultSnapshotWeights = weights;
    }

    void Update()
    {
        if (Input.GetKeyUp("j"))
        {
            //print("There is " + musicChannels.Length.ToString() + " music channels");
            //print("Channel 0 has " + musicChannels[0].ToString());
        }  
    }

    public void Play2DSound(string soundName)
    {
        int matchingNumber = GetIndex(soundName);
        
        if (matchingNumber < 9000)
            Play2DSound(matchingNumber);
        else
            Debug.Log("Sound name doesn't match any in SoundDB, check spelling.");
    }
    public void Play2DSound(int indexNumber)
    {
        PlaySound(indexNumber, 0, SoundType._2dSound, null);
    }
    public void PlayUISound(string soundName)
    {
        int matchingNumber = GetIndex(soundName);
        if (matchingNumber < 9000)
            PlayUISound(matchingNumber);
        else
            Debug.Log("Sound name doesn't match any in SoundDB, check spelling.");
    }
    public void PlayUISound(int indexNumber)
    {
        PlaySound(indexNumber, 0, SoundType._UISound, null);
    }
    /// <summary>Plays 3D Sound at AudioListener's location</summary>
    public void Play3DSound(string soundName)
    {
        int matchingNumber = GetIndex(soundName);
        if (matchingNumber < 9000)
            Play3DSound(matchingNumber);
        else
            Debug.Log("Sound name doesn't match any in SoundDB, check spelling.");
    }
    public void Play3DSound(int indexNumber)
    {
        PlaySound(indexNumber, 1, SoundType._3dSound, Listener.gameObject.transform);
    }
    /// <summary>Plays 3D Sound at target location</summary>
    public void Play3DSound(string soundName, Transform target)
    {
        int matchingNumber = GetIndex(soundName);
        if (matchingNumber < 9000)
            Play3DSound(matchingNumber, target);
        else
            Debug.Log("Sound name doesn't match any in SoundDB, check spelling.");
    }
    public void Play3DSound(int indexNumber, Transform target)
    {
        PlaySound(indexNumber, 1, SoundType._3dSound, target);
    }

    /// <summary>Checks SoundPlayer[] if any AudioClip by soundName is playing</summary>
    public bool SoundIsPlaying(string soundName)
    {
        int matchingNumber = GetIndex(soundName);
        if (matchingNumber < 9000)
            return SoundIsPlaying(matchingNumber);
        else
            Debug.Log("Sound name doesn't match any in SoundDB, check spelling.");
        return false;
    }
    /// <summary>Checks SoundPlayer[] if any AudioClip of SoundDB[indexNumber] is playing</summary>
    public bool SoundIsPlaying(int indexNumber)
    {
        for (int i = 0; i < soundPlayers.Length; i++)
        {
            if (soundPlayers[i].clip == soundDB[indexNumber])
            {
                if (soundPlayers[i].isPlaying)
                    return true;
            }
        }
        return false;
    }

    /// <summary>Stops all sounds matching the name</summary>
    public void StopSound(string soundName)
    {
        int matchingNumber = GetIndex(soundName);
        if (matchingNumber < 9000)
            StopSound(matchingNumber);    
        else
            Debug.Log("Sound name doesn't match SoundDB, check spelling.");
    }
    
    /// <summary>Stops all sounds matching the index number</summary>
    public void StopSound(int indexNumber)
    {
        for (int i = 0; i < soundPlayers.Length; i++)
        {
            if (soundPlayers[i].clip == soundDB[indexNumber])
            {
                if (soundPlayers[i].isPlaying) soundPlayers[i].Stop();
            }
        }
    }

    public void ClearAllSounds()
    { }

    #region InternalMethods
    private void PlaySound(int sound, float spatialBlend, SoundType type, Transform location)
    {
        if (sound >= soundDB.Length)
        {
            Debug.Log("ERROR: ID Number is larger than Sound Database! Nothing will be played.");
            return;
        }
        AudioSource source = GetNextAvailableAudioSource();
        source.clip = soundDB[sound];
        source.spatialBlend = spatialBlend;
        
        //Assign Audiomixer that audiosource points to
        switch (type)
        {
            case SoundType._2dSound:
                source.outputAudioMixerGroup = SoundChannel("GameWorld");
                break;
            case SoundType._3dSound:
                source.outputAudioMixerGroup = SoundChannel("GameWorld");
                break;
            case SoundType._UISound:
                source.outputAudioMixerGroup = SoundChannel("UI");
                break;
            case SoundType.noType:
                source.outputAudioMixerGroup = SoundChannel("other");
                break;
            default:
                Debug.Log("SoundMaster.PlaySound doesn't have SoundType" + type.ToString() + " tied to an AudioMixerGroup");
                break;
        }

        source.Play();
        
        if(source.gameObject.name == "BackupSoundPlayer")
        {
            Destroy(source.gameObject, source.clip.length + 1f);
        }

        //If there's a SoundPlayer class attached to audiosource's gameobject, have it follow
        SoundPlayer supportClass = source.gameObject.GetComponent<SoundPlayer>();
        if (supportClass != null)
            supportClass.Follow(location);
    }
    
    /// <summary>Goes through SoundPlayer[] pool, returns one that isn't doing anything</summary>
    private AudioSource GetNextAvailableAudioSource()
    {
        if (_soundPlayer < soundPlayers.Length)
        {
            for (int i = _soundPlayer; i < soundPlayers.Length; i++ ) //Go through rest of pool
            {
                if (!soundPlayers[i].isPlaying)
                {
                    _soundPlayer = i + 1;
                    return soundPlayers[i];
                }
            }
            Debug.Log("Reached end of SoundPlayer, everything after position " + _soundPlayer.ToString() + 
                " seems to be busy with sounds. Starting back at beginning");
            _soundPlayer = soundPlayers.Length; //Setting index to end so if statement below catches it
        }
        if(_soundPlayer >= soundPlayers.Length) 
        {
            for (int i = 0; i < soundPlayers.Length; i++) //Go through entire pool for unused AudioSource
            {
                if (!soundPlayers[i].isPlaying)
                {
                    _soundPlayer = i + 1;
                    return soundPlayers[i];
                }
            }
            Debug.Log("SoundPlayer[] is all used up! Creating backup AudioSource. Increase size of pool or check for bugs.");
        }
        _soundPlayer = 0;
        GameObject backupSoundPlayer = new GameObject("BackupSoundPlayer");
        backupSoundPlayer.AddComponent<AudioSource>();
        backupSoundPlayer.AddComponent<SoundPlayer>();
        return backupSoundPlayer.GetComponent<AudioSource>();
    }

    /// <summary>Finds the Group in _SoundChannels[] by name</summary>
    private AudioMixerGroup SoundChannel(string name)
    {
        for (int i = 0; i < _soundChannels.Length; i++)
        {
            if (_soundChannels[i].name == name)
            {
                return _soundChannels[i];
            }
        }
        Debug.Log("Didn't find the Channel, using last one which should be called 'other'.");
        return _soundChannels[_soundChannels.Length - 1];
    }

    private int GetIndex(string name)
    {
        int noMatch = 9000;
        for (int i = 0; i < soundDB.Length; i++)
        {
            if (soundDB[i].name == name)
            {
                return i;
            }
        }
        return noMatch;
    }

    #endregion
    /*
    
    public static bool SoundIsPlaying(int sound)
    {
        int amountPlaying = 0;
        AudioSource[] sounds = _instance.GetComponents<AudioSource>();
        for (int i =0; i < sounds.Length; i++)
        {
            //for particular sound look for the matching clip
            if (sounds[i].clip == _instance.sounds[sound])
                amountPlaying++;
        }
        if (amountPlaying > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void ClearLoopingSounds()
    {
        AudioSource[] sounds = _instance.GetComponents<AudioSource>();
        for (int i =0; i < sounds.Length; i++)
        {
            if (sounds[i].loop == true)
                Destroy(sounds[i]);
        }
    }

    public static void StopClip(int sound)
    {
        AudioSource[] sounds = _instance.GetComponents<AudioSource>();
        for (int i =0; i < sounds.Length; i++)
        {
            //for particular sound look for the matching clip
            if (sounds[i].clip == _instance.sounds[sound])
                Destroy(sounds[i]);
        }
    }
    */
}
