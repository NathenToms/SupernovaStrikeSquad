﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarLobby : MonoBehaviour
{
	// Global Members
	public static HangarLobby Instance;

	// Editor References
	[SerializeField] 
	private List<Dock> ShipDocks = new List<Dock>();

	[SerializeField]
	private List<Transform> SpawnPoints = new List<Transform>();

	// Setup the Hangar Singleton
	void Awake()
	{
		if (Instance) Destroy(gameObject); Instance = this;
	}

	public Transform GetSpawnPosition(int index) => SpawnPoints[index];

	public void UpdateHangarStates()
	{
		foreach (var player in FindObjectsOfType<PlayerConnection>())
		{
			ShipDocks[player.playerIndex].StopAllCoroutines();
			OpenGate(player.playerIndex);
		}
	}

	public PlayerInfoDisplay GetInfoDisplay(int playerIndex)
	{
		return ShipDocks[playerIndex].GetInfoDisplay();
	}

	public void OpenGate(int index) => ShipDocks[index].OpenDockDoors();
	public void CloseGate(int index) => ShipDocks[index].CloseDockDoors();

	public void ChangeGameMode(int newModeID)
	{
		PlayerConnection.LocalPlayer.CmdUpdateGameMode((GameModeType)newModeID);
	}

	public void StartGame()
	{
		PlayerConnection.LocalPlayer.CmdStartGame();
	}

	private void OnDrawGizmos()
	{
		foreach (Transform spawnPoint in SpawnPoints)
		{
			Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
			Gizmos.DrawCube(spawnPoint.position, Vector3.one * 0.5f);
		}
	}
}
