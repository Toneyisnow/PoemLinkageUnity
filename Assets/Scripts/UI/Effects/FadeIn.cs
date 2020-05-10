using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class FadeIn : MonoBehaviour
{
	private static int instanceCount;

	private float fadeTime = 3.0f;
	private Color color = Color.black;

	private float startTime;

	public void SetFadeTime(float value)
	{
		fadeTime = value;
	}

	// Use this for initialization
	void Start()
	{
		startTime = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	void Update()
	{
		var renderer = this.gameObject.GetComponent<SpriteRenderer>();

		float diffTime = Time.realtimeSinceStartup - startTime;
		if (diffTime >= fadeTime)
		{
			renderer.color = new Color(color.r, color.g, color.b, 1.0f);
			//renderer.color = new Color(color.r, color.g, color.b, 0.6f);
			Destroy(this);
		}
		else
		{
			renderer.color = new Color(color.r, color.g, color.b, diffTime / fadeTime * 1.0f);
		}
	}

	private void initComponents()
	{

	}
}