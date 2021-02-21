﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerConnection_ObjectManager : NetworkBehaviour
{
	[Header("References")]

	[SerializeField]
	public GameObject shipPrefab = null;

	[SerializeField]
	public GameObject characterPrefab = null;

	[Header("Player Object")]

	public GameObject PlayerObject;

	[Command]
	public void CmdSpawnShipIntoGames()
	{
		PlayerObject = Instantiate(shipPrefab);
		NetworkServer.Spawn(PlayerObject, connectionToClient);
	}

	[Command]
	public void CmdSpawnCharacterIntoGames()
	{
		PlayerObject = Instantiate(characterPrefab);
		NetworkServer.Spawn(PlayerObject, connectionToClient);
	}

	public void PausePlayerObject()
	{
		if (!hasAuthority) return;

		if (PlayerObject.TryGetComponent<PlayerShipController>(out PlayerShipController shipController))
		{
			shipController.Pause(true);
		}
	}
	public void UnpausePlayerObject()
	{
		if (!hasAuthority) return; 
		
		if (PlayerObject.TryGetComponent<PlayerShipController>(out PlayerShipController shipController))
		{
			shipController.Pause(false);
		}
	}

	public void PlayerEnterLevelAnimation()
	{
		if (PlayerObject.TryGetComponent<PlayerShipController>(out PlayerShipController shipController))
		{
			shipController.PlayEnterLevel();
		}
	}	
	public void PlayerExitLevelAnimation()
	{
		if (PlayerObject.TryGetComponent<PlayerShipController>(out PlayerShipController shipController))
		{
			shipController.PlayExitLevel();
		}
	}
}
