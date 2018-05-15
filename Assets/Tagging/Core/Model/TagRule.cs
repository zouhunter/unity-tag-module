using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace Tagging
{
    [System.Serializable]
    public struct LineRule
    {
        public Material lineMat;
        public Color startColor;
        public Color endColor;
        public LineTextureMode textureMode;
        public float startWidth;
        public float endWidth;
        public float length;
    }

    [System.Serializable]
    public struct FontRule
    {
        public int fontSize;
        public Color fontColor;
    }

    [System.Serializable]
    public struct DirectionRule
    {
        public DirectionType dirType;
        public float angle;
    }

    public enum DirectionType
    {
        Left,
        Right,
        Auto
    }
}