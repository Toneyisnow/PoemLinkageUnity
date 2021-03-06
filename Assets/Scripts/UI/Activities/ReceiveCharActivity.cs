﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.UI.Activities
{
    public class ReceiveCharActivity : BaseActivity
    {
        private Vector3 maxScale = new Vector3(2f, 2f, 1.0f);

        private Vector3 minScale = new Vector3(1.0f, 1.0f, 1.0f);

        private GameObject rootNode;

        private GameObject characterNode;

        private float t;

        public ReceiveCharActivity(GameObject root, GameObject charNode)
        {
            this.rootNode = root;
            this.characterNode = charNode;
        }

        public void SetScales(float min, float max)
        {
            minScale = new Vector3(min, min, 1.0f);
            maxScale = new Vector3(max, max, 1.0f);
        }

        public override bool HasFinished()
        {
            if (characterNode == null)
            {
                return true;
            }

            if (characterNode.transform.localScale == minScale)
            {
                return true;
            }

            return false;
        }

        public override void OnBeginning()
        {
            characterNode.SetActive(true);

            t = 0;
            characterNode.transform.localScale = maxScale;
            var renderer = characterNode.GetComponent<SpriteRenderer>();

            Color theColorToAdjust = renderer.color;
            theColorToAdjust.a = 1.0f;
            renderer.color = theColorToAdjust;
        }

        public override void OnFinished()
        {
            
        }

        public override void Update()
        {
            t += Time.deltaTime * 1.0f;
            characterNode.transform.localScale = Vector3.Lerp(maxScale, minScale, Mathf.SmoothStep(0.0f, 1.0f, t));
        }
    }
}
