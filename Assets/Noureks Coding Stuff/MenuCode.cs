﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuCode : MonoBehaviour
{
	public void LoadScene(int index){
		SceneManager.LoadScene(index);
	}
	public void Quit(int index){
		Application.Quit();
	}

}
