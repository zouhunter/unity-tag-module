using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Tagging
{
    public class TextBody : MonoBehaviour
    {
        private Text uiText;
        private TextMesh threedText;
        private FontRule rule;
        private Vector3[] worldConer = new Vector3[4];
        private RectTransform recttransform;
        private void Awake()
        {
            if(transform is RectTransform)
            {
                recttransform = transform as RectTransform;
                uiText = GetComponentInChildren<Text>();
            }
            else
            {
                threedText = GetComponentInChildren<TextMesh>();
            }
        }
        internal void SetText(string name)
        {
            //var text ="  " + name.TrimStart(' ').TrimEnd(' ') + " ";
            if(uiText != null)
            {
                uiText.text = name;
            }
            else if(threedText != null)
            {
                threedText.text = name;
            }
        }

        internal void SetFontRule(FontRule fontRule)
        {
            this.rule = fontRule;
            if (uiText != null)
            {
                uiText.fontSize = fontRule.fontSize;
                uiText.color = fontRule.fontColor;
            }
            else if (threedText != null)
            {
                threedText.fontSize = fontRule.fontSize;
                threedText.color = fontRule.fontColor;
            }
        }

        internal float GetWidth()
        {
            if(uiText != null)
            {
                recttransform.GetWorldCorners(worldConer);
                return Vector3.Distance( worldConer[2],worldConer[1]);
            }
            else
            {
                return (name.Length) * rule.fontSize / 10;
            }
        }
        internal float GetHeight()
        {
            if (uiText != null)
            {
                recttransform.GetWorldCorners(worldConer);
                return Vector3.Distance(worldConer[1], worldConer[0]);
            }
            else
            {
                return 0;
            }
        }

        internal void SetPosition(Vector3 vector3)
        {
            transform.position = vector3 + Vector3.up * GetHeight() * 0.5f;
        }
    }
}