﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	[SerializeField] 
	private MenuTransition OpenTransition = null;

	[SerializeField] 
	private MenuTransition CloseTransition = null;

	[SerializeField]
	private Text MenuTitle = null;


	// Global Members
	public static Menu CurrentMenu;


	// Public Members
	public string MenuName;


	// Private Members
	private bool transitioning;


	public void Open()
	{
		if (transitioning == false)
		{
			gameObject.SetActive(true);

			StartCoroutine(TransitionMenu());
		};
	}

	private IEnumerator TransitionMenu()
	{
		transitioning = true;     
		
		// If we have a menu open
		if (CurrentMenu)
		{
			yield return CurrentMenu.CloseLastMenu();
		}

		CurrentMenu = this;

		if (CurrentMenu.OpenTransition)  yield return CurrentMenu.OpenTransition.Play();

		transitioning = false;
	}

	private IEnumerator CloseLastMenu()
	{
		if (CloseTransition) {
			yield return CloseTransition.Play();
			CurrentMenu.gameObject.SetActive(false);
		} 
	}

	private void OnValidate()
	{
		if (MenuTitle) MenuTitle.text = MenuName;
	}
}