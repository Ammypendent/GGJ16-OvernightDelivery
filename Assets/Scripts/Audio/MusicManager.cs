using UnityEngine;
using UnityEngine.Audio; //Unity 5 Audio API
using System.Collections;

public class MusicManager : MonoBehaviour 
{
    /// <summary>
    /// Songlist: --
    /// </summary>
    [Tooltip("Make this the soundtrack database")]
    public AudioClip[] songDB;
    [Tooltip("Connect Music Players to Music Mixer's AudioGroups")]
    public AudioSource[] musicPlayers;
    public AudioMixer MusicMixer;
    [Tooltip("Order ChannelNumber snapshots with MusicPlayers by AudioGroup for correct song switching")]
    public AudioMixerSnapshot[] snapshots;
    [Tooltip("Which snapshots are active [1,0]. Array needs to be same length as snapshots above")]
    public float[] weights;
    private AudioMixerGroup[] _musicChannels;
    private float[] defaultSnapshotWeights;
    private int previousSongSnapshot;
	
	void Awake () 
    {
        DontDestroyOnLoad(transform.gameObject);    //in case this is in diff GameObject as SoundMaster
        
        /* //If Music Players are not children of MusicManager, uncomment this
        for (int i = 0; i < musicPlayers.Length; i++ )
        {
            DontDestroyOnLoad(musicPlayers[i].transform.gameObject);
        }*/

        if (MusicMixer == null) MusicMixer = Resources.Load("Music") as AudioMixer;
        _musicChannels = MusicMixer.FindMatchingGroups("Master/");
        defaultSnapshotWeights = weights;

        if (musicPlayers[0].outputAudioMixerGroup == null)
        {
            Debug.Log("Default Music Player have no audiomixergroups attached, attaching one automatically. " + 
                "If you have other music players active, attach them to their respective audiogroups.");    
            musicPlayers[0].outputAudioMixerGroup = _musicChannels[0];
        }
	}

    void Start ()
    {
        MusicMixer.TransitionToSnapshots(snapshots, weights, 0.01f);
        previousSongSnapshot = SnapshotLocation("Muted");
    }
	
	//Used as testing, comment out Update when things are set up correctly
	void Update () 
    {
	    
	}

    /// <summary>Looks for name match in songDB and returns index location, else 0</summary>
    public int Song(string name)
    {
        for (int i = 0; i < songDB.Length; i++ )
        {
            if (name == songDB[i].name)
                return i;
        }
        Debug.Log("Song Name doesn't match any clips in songDB[], did you mispell it?");
        return 0;
    }

    public void ChangeSong (int songNumber, float transitionTime)
    {
        AudioSource player = musicPlayers[0];   //default reference
        bool playerFound = false;
        bool alreadyPlaying = false;
        int correspondingSnapshot = 0;
        //Look to see if a musicplayer has the corresponding audioclip loaded already
        for (int i = 0; i < musicPlayers.Length; i++)
        {
            if (musicPlayers[i].clip == songDB[songNumber])
            {
                player = musicPlayers[i];
                correspondingSnapshot = i;
                playerFound = true;
                if (musicPlayers[i].isPlaying)
                    alreadyPlaying = true;
                break;
            }
        }
        if (!playerFound)
        {
            //Find player with current snapshot weight of 0
            //Because first X of snapshots[] should correspond to channels
            for (int i = 0; i < musicPlayers.Length; i++)
            {
                if (weights[i] <= 0)
                {
                    player = musicPlayers[i];
                    correspondingSnapshot = i;
                    playerFound = true;
                    break;
                }
            }
            if (!playerFound)
            {
                Debug.LogError("All MusicPlayers are in use according to snapshot weights. Cannot change song! \n" + 
                    "Check if you got snapshots[] & weights[] correctly set up with MusicPlayers[] or add more MusicPlayers." +
                    " Using MusicPlayers[0] for song change now.");
            }  
        }
        player.clip = songDB[songNumber];
        if (!alreadyPlaying)
        {
            player.Play();
            player.loop = true;
        }
        weights[previousSongSnapshot] = 0f;
        SetSnapshot(snapshots[correspondingSnapshot], 1f, transitionTime);
        previousSongSnapshot = correspondingSnapshot;
    }



    /// <summary>Finds matching snapshot in snapshots[] and performs multi-snapshot transition</summary>
    public void SetSnapshot (AudioMixerSnapshot snapshot, float weight, float transitionTime)
    {
        for (int i = 0; i < snapshots.Length; i++)
        {
            if (snapshot == snapshots[i])
            {
                weights[i] = weight;
                MusicMixer.TransitionToSnapshots(snapshots, weights, transitionTime);
                return;
            }
        }
        Debug.LogError("Snapshot Doesn't match anything in Snapshots[]!");
    }
    /// <summary>Finds matching snapshot by name in snapshots[] and performs multi-snapshot transition</summary>
    public void SetSnapshot (string snapshotName, float weight, float transitionTime)
    {
        for (int i = 0; i < snapshots.Length; i++)
        {
            if (snapshots[i].name == snapshotName)
            {
                weights[i] = weight;
                MusicMixer.TransitionToSnapshots(snapshots, weights, transitionTime);
                return;
            }
        }
        Debug.LogError("Snapshot Name doesn't match anything in Snapshots[]");
    }

    public int SnapshotLocation (string snapshotName)
    {
        for (int i = 0; i < snapshots.Length; i++)
        {
            if (snapshots[i].name == snapshotName)
            {
                return i;
            }
        }
        Debug.Log("Snapshot " + snapshotName.ToString() + " is not found in Snapshots[]");
        return 0;
    }

    public void ResetSnapshots()
    {
        weights = defaultSnapshotWeights;
        MusicMixer.TransitionToSnapshots(snapshots, weights, 0.1f);
    }

}
