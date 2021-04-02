﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;


// The NetworkPlayer is the class we want to write all player / client logic.
public class NetworkPlayer : Player
{
	[SerializeField] private GameObject playerObjectPrefab = null;
	[SerializeField] private GameObject shipObjectPrefab = null;

	public WeaponTypes[] Weapons = new WeaponTypes[] {
		WeaponTypes.EnergyMiniGun, 
		WeaponTypes.RocketsLauncher
	};

	public ShipType Ship = ShipType.ShipA;

	private void Update()
	{
		// Ready UP
		if (hasAuthority)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				Ready = !Ready;
				Cmd_UpdateReady(Ready);
			}
		}
	}

	#region Client

	public override void OnStartAuthority()
	{
		base.OnStartAuthority();
		//Cmd_SpawnShip();

		Cmd_SpawnPlayer();
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		if (isServer) UpdateShipBays(true);
	}

	public override void OnStopClient()
	{
		base.OnStopClient();
		if (isServer) UpdateShipBays(false);
	}

	public override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Gameplay")
		{
			Cmd_SpawnShip();
		}

		if (scene.name == "Main")
		{
			Cmd_SpawnPlayer();
		}
	}

	#region RPC Calls

	[ClientRpc]
	public void Rpc_ChangeScene(string scene)
	{
		StartCoroutine(ChangeScene(scene));
		SceneManager.LoadScene(scene);
	}

	private IEnumerator ChangeScene(string scene)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			yield return null;
		}

		if (isServer)
		{
			if (scene == "Gameplay")
			{
				Cmd_SpawnShip();
			}

			if (scene == "Main")
			{
				Cmd_SpawnPlayer();
			}
		}

	}

	#endregion

	#endregion

	#region Server

	[Server]
	public void UpdateShipBays(bool starting)
	{
		foreach (ShipBay ship in FindObjectsOfType<ShipBay>())
		{
			if (ship.ownerID == ID) {
				ship.Open = starting;
			}
		}
	}

	#region Commands

	[Command]
	public void Cmd_SpawnPlayer()
	{
		NetworkServer.Spawn(Instantiate(playerObjectPrefab), connectionToClient);
	}

	[Command]
	public void Cmd_SpawnShip()
	{
		NetworkServer.Spawn(Instantiate(shipObjectPrefab), connectionToClient);
	}

	[Command]
	public void Cmd_UpdateMissionType(string[] args)
	{
		GameManager.Instance.Settings.UpdateMissionType(args);
	}

	[Command]
	public void Cmd_UpdateWeapon(WeaponTypes weaponName, int playerWeaponSlotIndex)
	{
		if (playerWeaponSlotIndex < Weapons.Length) {
			Weapons[playerWeaponSlotIndex] = weaponName;
		}
	}	
	
	[Command]
	public void Cmd_UpdateShip(ShipType shipType)
	{
		Ship = shipType;
	}

	[Command]
	public void Cmd_ClickNode(int nodeIndex)
	{
		var nodemap = FindObjectOfType<NodeMap>();
		if (nodemap)
		{
			nodemap.SelectNewNode(nodeIndex);
		}
	}

	#endregion

	#endregion
}