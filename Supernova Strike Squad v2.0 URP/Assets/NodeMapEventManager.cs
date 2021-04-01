﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[RequireComponent(typeof(NodeMap))]

public class NodeMapEventManager : NetworkBehaviour
{
	NodeMap nodeMap;

	public override void OnStartServer()
	{
		nodeMap = GetComponent<NodeMap>();
	}

	public void StartNewEvent()
	{
		// TEMP
		nodeMap.CompleteNode();
	}
}
