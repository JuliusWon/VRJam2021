﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptStunGun : MonoBehaviour
{
	#region Properties
	public float slashDamage = 25f;
	#endregion

	#region MonoBehaviour Stuff
	private void OnCollisionEnter(Collision collision)
	{
		collision.collider.SendMessage("HandleDamage", slashDamage, SendMessageOptions.DontRequireReceiver);
	}
	#endregion
}