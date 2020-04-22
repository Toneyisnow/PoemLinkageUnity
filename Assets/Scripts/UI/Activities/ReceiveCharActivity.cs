using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.UI.Activities
{
    public class ReceiveCharActivity : BaseActivity
    {
        private static Vector3 maxScale = new Vector3(2f, 2f, 1.0f);

        private static Vector3 minScale = new Vector3(1.0f, 1.0f, 1.0f);

        private GameObject rootNode;

        private GameObject characterNode;

        private float t;

        public ReceiveCharActivity(GameObject root, GameObject charNode)
        {
            this.rootNode = root;
            this.characterNode = charNode;
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
            t = 0;
            characterNode.transform.localScale = maxScale;
            var renderer = characterNode.GetComponent<SpriteRenderer>();

            Color theColorToAdjust = renderer.material.color;
            theColorToAdjust.a = 1.0f;
            renderer.material.color = theColorToAdjust;
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
