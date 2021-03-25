﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipProfileScreen : MonoBehaviour
{
	[SerializeField] private ShipSelectScreen shipSelectScreen = null;

	public void SelectNewShip()
	{
		Instantiate(shipSelectScreen, GetComponentInParent<Canvas>().transform).Open(OnSelectNewShip);
	}

	public void Return()
	{
		Debug.Log("Return");
	}

	public void OnSelectNewShip(string shipName)
	{
		Debug.Log(shipName);
	}
}
