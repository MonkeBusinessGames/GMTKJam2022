using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    public GameObject startTrack;
    public float startTrackDuration;
    private float timer=0;
    public GameObject middleLoop;
    // Start is called before the first frame update
    void Start()
    {
        startTrack.SetActive(true);
        timer = startTrackDuration;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            middleLoop.SetActive(true);
            startTrack.SetActive(false);
        }
    }
}
