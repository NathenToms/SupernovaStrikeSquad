﻿using System.Collections;
using UnityEngine;

public class Dock : MonoBehaviour
{
	[Header("References")]

	[SerializeField]
	private Transform dockDoorL = null;

	[SerializeField]
	private Transform dockDoorR = null;

	[SerializeField]
	private Transform shipModel = null;

	[Header("States")]

	public DockStates State = DockStates.Closed;

	// Start is called before the first frame update
	void Start()
	{
		OpenDockDoors();
	}

	[ContextMenu("Open Dock")]
	public void OpenDockDoors()
	{
		StartCoroutine(coOpenDockDoors());
	}

	[ContextMenu("Close Dock")]
	public void CloseDockDoors()
	{
		StartCoroutine(coCloseDockDoors());
	}

	private IEnumerator coOpenDockDoors()
	{
		if (State == DockStates.Transitioning || State == DockStates.Open) yield return coCloseDockDoors();

		if (State == DockStates.Closed)
		{
			State = DockStates.Transitioning;

			bool waiting = true;

			Tween.Instance.EaseOut_Scale_BounceX(dockDoorL, 1, 0.25f, 3, 0);
			Tween.Instance.EaseOut_Scale_BounceX(dockDoorR, 1, 0.25f, 3, 0, () => {
				waiting = false;
			});

			while (waiting) yield return null;

			waiting = true;

			Tween.Instance.EaseOut_Transform_ElasticY(shipModel, 0, 3, 1.5f, 0, () => {
				waiting = false;
			});

			while (waiting) yield return null;

			State = DockStates.Open;
		}
	}
	public IEnumerator coCloseDockDoors()
	{
		State = DockStates.Transitioning;

		bool waiting = true;

		Tween.Instance.EaseOut_Transform_ElasticY(shipModel, shipModel.localPosition.y, 0, 2.5f, 0, () => {
			waiting = false;
		});

		while (waiting) yield return null;

		waiting = true;

		Tween.Instance.EaseOut_Scale_BounceX(dockDoorL, 0.25f, 1, 2.5f, 0);
		Tween.Instance.EaseOut_Scale_BounceX(dockDoorR, 0.25f, 1, 2.5f, 0, () => {
			waiting = false;
		});

		while (waiting) yield return null;

		State = DockStates.Closed;
	}
}
