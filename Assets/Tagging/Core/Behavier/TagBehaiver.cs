using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Tagging
{

    public class TagBehaiver : MonoBehaviour, ITagBehaiver
    {
        [SerializeField]
        private string _id;
        [SerializeField]
        private LineRule _lineRule = new LineRule() { endColor = Color.green, startColor = Color.red, startWidth = 0.04f, endWidth = 0.04f, length = 2, lineMat = null, textureMode = LineTextureMode.Stretch };
        [SerializeField]
        private FontRule _fontRule = new FontRule() { fontSize = 4, fontColor = Color.black };
        [SerializeField]
        private DirectionRule _directionRule = new DirectionRule() { dirType = DirectionType.Auto, angle = 45 };

        [SerializeField]
        public string ID { get { if (string.IsNullOrEmpty(_id)) _id = name; return _id; } }
        private LineRenderer _line;
        private LineRenderer line
        {
            get
            {
                if (_line == null)
                {
                    var go = new GameObject("Line");
                    go.transform.SetParent(transform);
                    _line = go.AddComponent<LineRenderer>();
                    _line.positionCount = 3;
                }
                return _line;
            }
        }

        internal void Hide()
        {
            line.enabled = false;
            text.gameObject.SetActive(false);
        }

        internal void Show()
        {
            line.enabled = true;
            text.gameObject.SetActive(true);
        }

        public TextBody textPrefab { get; set; }
        private TextBody _text;
        private TextBody text
        {
            get
            {
                if (_text == null)
                {
                    _text = Instantiate(textPrefab);
                    _text.transform.SetParent(transform);
                }
                return _text;
            }
        }

        internal void Init(TextBody textPrefab, Material lineMaterial)
        {
            this.textPrefab = textPrefab;
            if (lineMaterial != null && _lineRule.lineMat == null) this._lineRule.lineMat = lineMaterial;
        }

        #region Public Props
        public LineRule lineRule
        {
            get
            {
                return _lineRule;
            }
            set
            {
                _lineRule = value;
                OnSetLineRule();
            }
        }
        public FontRule fontRule
        {
            set
            {
                _fontRule = value;
                text.SetFontRule(_fontRule);
            }
        }
        public DirectionRule directionRule
        {
            get
            {
                return _directionRule;
            }
            set
            {
                _directionRule = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnNameChanged();
            }
        }
        #endregion

        private void Awake()
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = name;
            }
            else
            {
                name = _id;
            }
        }
        private void Start()
        {
            OnSetLineRule();
            text.SetFontRule(_fontRule);
            text.SetText(name);
        }

        /// <summary>
        /// 重置坐标指向
        /// </summary>
        /// <param name="position"></param>
        internal void ResetTagPos(Vector3 position)
        {
            Vector3 direction1 = Vector3.up;
            Vector3 direction2 = ViewRight;

            //float angle = Vector3.Angle(Vector3.right, ViewRight);

            DirectionType dirType = _directionRule.dirType;
            if (dirType == DirectionType.Auto)
            {
                bool normal = Vector3.Dot(ViewRight, Vector3.right) >= 0;

                if (transform.position.x >= 0)
                {
                    dirType = normal ? DirectionType.Right : DirectionType.Left;
                }
                else
                {
                    dirType = normal ? DirectionType.Left : DirectionType.Right;
                }
            }
            if (dirType == DirectionType.Right)
            {
                direction1 = (Mathf.Tan(Mathf.Deg2Rad * _directionRule.angle) * Vector3.up + ViewRight).normalized;
                direction2 = ViewRight;
            }
            else if (dirType == DirectionType.Left)
            {
                direction1 = (Mathf.Tan(Mathf.Deg2Rad * _directionRule.angle) * Vector3.up - ViewRight).normalized;
                direction2 = -ViewRight;
            }

            //节点一
            var endPos1 = position + direction1.normalized * _lineRule.length;
            //节点二
            var endPos2 = endPos1 + (direction2 * text.GetWidth());

            line.SetPositions(new Vector3[] { position, endPos1, endPos2 });
            text.SetPosition((endPos1 + endPos2) * 0.5f);
            text.transform.forward = ViewForward;
        }

        private Vector3 ViewForward
        {

            get
            {
                if (Camera.main == null)
                {
                    return Vector3.forward;
                }
                else
                {
                    return Quaternion.Euler(0, -90, 0) * ViewRight;
                }
            }
        }

        private Vector3 ViewRight
        {
            get
            {
                if (Camera.main == null)
                {
                    return Vector3.right;
                }
                else
                {
                    return Camera.main.transform.right;
                }
            }
        }

        /// <summary>
        /// 生成线
        /// </summary>
        private void OnSetLineRule()
        {
            if (line.material == null && _lineRule.lineMat == null){
                _lineRule.lineMat = new Material(Shader.Find("Default-Particle"));
            }

            line.material = _lineRule.lineMat;
            line.startWidth = _lineRule.startWidth;
            line.endWidth = _lineRule.endWidth;
            line.startColor = _lineRule.startColor;
            line.endColor = _lineRule.endColor;
            line.textureMode = _lineRule.textureMode;
        }

        /// <summary>
        /// 显示文字
        /// </summary>
        private void OnNameChanged()
        {
            text.SetText(Name);
        }

    }
}
