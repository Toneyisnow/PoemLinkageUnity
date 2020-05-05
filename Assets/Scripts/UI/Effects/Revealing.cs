using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class Revealing : MonoBehaviour
{
	private static int instanceCount;

	private float fadeTime = 4.0f;
	private Color color = Color.black;

	private float startTime = 0.0f;

	// Use this for initialization
	void Start()
	{
		var renderer = this.gameObject.GetComponent<SpriteRenderer>();
		renderer.color = new Color(color.r, color.g, color.b, 0.6f);

		startTime = Time.realtimeSinceStartup;
		return;
	}

	// Update is called once per frame
	void Update()
	{
		var renderer = this.gameObject.GetComponent<SpriteRenderer>();

		if (startTime == 0.0f)
		{
			return;
		}

		float diffTime = Time.realtimeSinceStartup - startTime;
		if (diffTime >= fadeTime)
		{
			renderer.material.color = new Color(color.r, color.g, color.b, 0.0f);
			Destroy(this);
		}
		else
		{
			renderer.material.color = new Color(color.r, color.g, color.b, (1 - diffTime / fadeTime) * 0.6f);
		}
	}

	private void initComponents()
	{

	}
}