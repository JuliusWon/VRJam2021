﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidDespawner : MonoBehaviour
{
	ChunkLoading cl;
	float dist = 5;
	void Start(){
		cl = FindObjectOfType<ChunkLoading>();
	}
	void Update(){
		if(Vector3.Distance(transform.position, cl.chunk*10)>20){
		/* if(transform.position.x>(float)cl.chunk.x+dist||transform.position.x<(float)cl.chunk.x-dist||transform.position.x>(float)cl.chunk.y+dist||transform.position.x<(float)cl.chunk.y-dist||transform.position.x>(float)cl.chunk.z+dist||transform.position.x<(float)cl.chunk.z-dist){ */
			Destroy(this.gameObject);
		}
	}
}
