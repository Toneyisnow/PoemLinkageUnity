﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class FadeIn : MonoBehaviour
{
	private static int instanceCount;

	private float fadeTime = 1.3f;
	private Color color = Color.black;

	private float startTime;

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		var renderer = this.gameObject.GetComponent<SpriteRenderer>();
		if (startTime == 0.0f)
		{
			startTime = Time.realtimeSinceStartup;
			return;
		}

		float diffTime = Time.realtimeSinceStartup - startTime;
		if (diffTime >= fadeTime)
		{
			renderer.color = new Color(color.r, color.g, color.b, 0.6f);
			Destroy(this);
		}
		else
		{
			renderer.color = new Color(color.r, color.g, color.b, diffTime / fadeTime * 0.6f);
		}
	}

	private void initComponents()
	{

	}
}