﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptAudioManager : MonoBehaviour
{
    public GameObject earth;
    public GameObject[] enemyPacks;
    public AudioClip[] musicLoops;

    public enum vibes
    {
        Chill,
        EnemyTerritory,
        Alerted,
        Dead
    }

    private vibes currentVibe;
    private vibes prevVibe;
    private bool vibeChange = false;

    //Coroutines
    private IEnumerator playLoopRoutine;

    // Start is called before the first frame update
    void Start()
    {
        SetCurrentVibe(vibes.Chill);
    }

    // Update is called once per frame
    void Update()
    {
        //Chill vibe
        if (currentVibe == vibes.Chill)
            HandleChill();
        //Enemy territory
        else if (currentVibe == vibes.EnemyTerritory)
            HandleEnemyTerritory();
        else if (currentVibe == vibes.Alerted)
            HandleAlerted();

        CheckEnemyPacks();
    }

    //sets the curent vibe and  caches the previous vibe
    public void SetCurrentVibe(vibes newVibe)
    {
        if (newVibe != currentVibe)
        {
            prevVibe = currentVibe; //cache the previous vibe
            currentVibe = newVibe;

            vibeChange = true;
        }
		else
		{
            vibeChange = false;
		}
    }

    void HandleChill()
	{
        if (vibeChange)
        {
            TransitionMusic();
        }

        if (playLoopRoutine == null)
		{
            playLoopRoutine = PlayLoopRoutine(earth.GetComponent<AudioSource>(), vibes.Chill);
            StartCoroutine(playLoopRoutine);
        }
	}

    void HandleEnemyTerritory()
	{
        if (vibeChange)
		{
            TransitionMusic();
		}
            
        if (playLoopRoutine == null)
		{
            playLoopRoutine = PlayLoopRoutine(earth.GetComponent<AudioSource>(), vibes.EnemyTerritory);
            StartCoroutine(playLoopRoutine);
        }
	}

    void HandleAlerted()
	{
        if (vibeChange)
        {
            TransitionMusic();
        }

        if (playLoopRoutine == null)
		{
            playLoopRoutine = PlayLoopRoutine(earth.GetComponent<AudioSource>(), vibes.Alerted);
            StartCoroutine(playLoopRoutine);
        }
	}

    void CheckEnemyPacks()
    {
        vibes vibe = vibes.Chill;

        foreach (GameObject pack in enemyPacks)
        {
            var packScript = pack.GetComponent<scriptEnemyPack>();

            if (packScript.aware == scriptEnemyPack.awareness.Alerted)
            {
                vibe = vibes.Alerted;
                break; //break out cause we're full force in this case
            }
            else if (packScript.aware == scriptEnemyPack.awareness.InTerritory)
            {
                vibe = vibes.EnemyTerritory;
            }
        }

        SetCurrentVibe(vibe);
    }

	void TransitionMusic()
	{
        StopCoroutine(playLoopRoutine);
        playLoopRoutine = null;
    }

    //Coroutines
    private IEnumerator PlayLoopRoutine(AudioSource anASrc, vibes vibe)
    {
        AudioClip audClip = musicLoops[(int)vibe];

        anASrc.clip = audClip;
        anASrc.Play();

        yield return new WaitForSeconds(audClip.length); //give the enemies time to run their dislodge scripts

        playLoopRoutine = null;
    }
}