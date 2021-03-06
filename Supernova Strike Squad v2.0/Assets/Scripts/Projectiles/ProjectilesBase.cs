﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

// NOTE: Each projectile is spawned by the local clients weapon
// NOTE: Each projectile is synced by its owner not the server

// This is so we can get the smoothest gameplay. This could change

public class ProjectilesBase : NetworkBehaviour, IDamageSource
{
	// Public Members
	// The forward speed of the projectile
	public float Speed = 175.0f;

	public int Damage = 1;

	// Private Members
	private Rigidbody myRigidbody;

	// Start is called before the first frame update
	void Start()
	{
		myRigidbody = GetComponent<Rigidbody>();

		// Get this projectiles myRigidbody
		if (!isServer) Destroy(myRigidbody);

		// Destroy this projectile after 2s
		if (isServer) Destroy(gameObject, 2);
	}

	#region Server

	void FixedUpdate()
	{
		if (isServer)
		{
			myRigidbody.MovePosition(myRigidbody.position + transform.forward * Speed * Time.fixedDeltaTime);
		}
	}

	public int GetDamage()
	{
		return Damage;
	}

	// When we destroy this projectile we want to destroy it on all clients
	void OnDestroy()
	{
		if (isServer) NetworkServer.Destroy(gameObject);
	}

	#endregion
}
