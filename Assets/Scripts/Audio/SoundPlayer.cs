using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

/// <summary>Support class for SoundPlayer's AudioSource</summary>
public class SoundPlayer : MonoBehaviour 
{

    public Transform transformToFollow;
    private AudioSource source;
    
	void Start () 
    {
        source = GetComponent<AudioSource>();
	}
	




    /// <summary>Starts SoundPlayer coroutine to follow a GameObject until it finishes playing sound.</summary>
    public void Follow (Transform target)
    {
        if (target != null)
            transformToFollow = target;
        else
            transform.position = new Vector3(0, 0, 0);
        StartCoroutine("FollowObject");
    }

    private IEnumerator FollowObject()
    {
        if (source.isPlaying)
        {
            if (transformToFollow != null)
            {
                if (transformToFollow.position != transform.position)
                {
                    transform.position = transformToFollow.position;
                }
            }
            yield return null;
        }
        else
            StopCoroutine("FollowObject");
    }
}
