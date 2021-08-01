﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An enemy pack is a collection of enemies, when one becomes aware of the player, they can transmit that to other units in the pack.
public class scriptEnemyPack : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject earth;

    public enum types
    {
        AllDriller,
        AllHunter,
        RandomMix
    }

    public types type;

    public enum awareness
    {
        Unknown,
        InTerritory,
        Alerted
    }

    public awareness aware;

    public Transform packStartPoint;
    public int packSize = 5;
    public float patrolAreaSize = 5;
    public float enemyTerritoryRange = 5;
    public bool packAlerted = true;

    private List<GameObject> packUnits = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnPack(type, packStartPoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Check earth proximity
        CheckProximity();
    }

    public void SpawnPack(types type, Vector3 position)
	{
		switch (type)
		{
            case types.AllDriller:
                GameObject drillerPf = enemyPrefabs[(int)types.AllDriller];
                
                for (int i = 0; i < packSize; i++)
                {
                    var unit = Instantiate(drillerPf);

                    unit.transform.position = Random.insideUnitSphere * patrolAreaSize + position;

                    var script = unit.GetComponent<scriptEnemyDriller>();
                    script.pack = this;

                    packUnits.Add(unit);
                }
                break;
            case types.AllHunter:
                GameObject hunterPf = enemyPrefabs[(int)types.AllHunter];

                for (int i = 0; i < packSize; i++)
                {
                    var unit = Instantiate(hunterPf);

                    unit.transform.position = Random.insideUnitSphere * patrolAreaSize + position;

                    var script = unit.GetComponent<scriptEnemyHunter>();
                    script.pack = this;

                    packUnits.Add(unit);
                }
                break;
            default:
                Debug.LogWarning($"Have not implemented pack type {type}.");
                break;
        }
	}

    void CheckProximity()
	{
        if (earth != null)
		{
            var dist = Vector3.Distance(packStartPoint.position, earth.transform.position);

            if (dist < enemyTerritoryRange)
            {
                if (!packAlerted)
                    aware = awareness.InTerritory;
                else
                    aware = awareness.Alerted;
            }
            else
            {
                aware = awareness.Unknown;
                RetreatPackMembers();
            }
		}
	}

    public void AlertPackMembers(Transform target)
	{
        packAlerted = true;
		
        foreach (GameObject unit in packUnits)
        {
            unit.SendMessage("AlertUnit", target, SendMessageOptions.DontRequireReceiver);
        }
	}

    public void RetreatPackMembers()
	{
        packAlerted = false;

        foreach (GameObject unit in packUnits)
        {
            unit.SendMessage("RetreatUnit", SendMessageOptions.DontRequireReceiver);
        }
    }
}
