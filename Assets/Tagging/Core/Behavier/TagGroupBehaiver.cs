using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Tagging
{

    public class TagGroupBehaiver : MonoBehaviour, ITagGroupBehaiver
    {
        [SerializeField]
        private string key;
        [SerializeField]
        private TextBody textPrefab;
        [SerializeField]
        private Material lineMaterial;
        [SerializeField]
        private List<TagBehaiver> tagbehaviers;

        private ITagPosCtrl posCtrl;

        private Dictionary<string, TagBehaiver> tagbehavierDic = new Dictionary<string, TagBehaiver>();

        public string GroupKey { get { if (string.IsNullOrEmpty(key)) key = name; return key; } }

        private void Awake()
        {
            TagController.Instence.RegistGroup(this);
            RegistTagBehaivers();
        }
        private void OnEnable()
        {
            ForEach(x => x.Show());
        }
        private void Update()
        {
            RefeshPosition();
        }
        private void OnDisable()
        {
            ForEach(x => x.Hide());
        }
        private void OnDestroy()
        {
            TagController.Instence.RemoveGroup(this);
        }

        private void Start()
        {
            RefeshPosition();
        }
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            foreach (var item in tagbehaviers)
            {
                Color lineColor = Color.white;

                if(UnityEditor.Selection.objects.Contains(item.gameObject))
                {
                    lineColor = Color.green;
                }

                var position = item.transform.position;
                Vector3 direction1 = Vector3.up;
                DirectionType dirType = item.directionRule.dirType;
                if (dirType == DirectionType.Auto)
                {
                    if (position.x >= 0)
                    {
                        dirType = DirectionType.Right;
                    }
                    else
                    {
                        dirType = DirectionType.Left;
                    }
                }
                if (dirType == DirectionType.Right)
                {
                    direction1 = (Mathf.Tan(Mathf.Deg2Rad * item.directionRule.angle) * Vector3.up + Vector3.right).normalized;
                }
                else if (dirType == DirectionType.Left)
                {
                    direction1 = (Mathf.Tan(Mathf.Deg2Rad * item.directionRule.angle) * Vector3.up + Vector3.left).normalized;
                }

                //节点一
                var endPos1 = position + direction1.normalized * item.lineRule.length;
                Debug.DrawLine(item.transform.position, endPos1, lineColor);
            }
        }
#endif

        private void RegistTagBehaivers()
        {
            var tgbs = gameObject.GetComponentsInChildren<TagBehaiver>(true);
            posCtrl = new TagPosController(tgbs);
            foreach (var item in tgbs)
            {
                if (!tagbehaviers.Contains(item))
                {
                    tagbehaviers.Add(item);
                }
                tagbehavierDic[item.ID] = item;
                item.Init(textPrefab, lineMaterial);
                item.Hide();
            }
        }

        public void RefeshPosition()
        {
            posCtrl.Refesh();
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="rule"></param>
        public bool SetLineRule(LineRule rule, string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                ForEach((x) =>
                {
                    SetLineRuleInternal(rule, x);
                });
                return true;
            }
            else
            {
                TagBehaiver item;
                bool contain = tagbehavierDic.TryGetValue(key, out item);
                if (contain && item != null)
                {
                    SetLineRuleInternal(rule, item);
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        private void SetLineRuleInternal(LineRule rule, TagBehaiver behaiver)
        {
            if (behaiver == null) return;
            behaiver.lineRule = rule;
        }

        /// <summary>
        /// 设置尺寸
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="index"></param>
        public bool SetFontRule(FontRule rule, string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                ForEach((x) =>
                {
                    SetFontInternal(rule, x);
                });
                return true;
            }
            else
            {
                TagBehaiver item;
                bool contain = tagbehavierDic.TryGetValue(key, out item);
                if (contain && item != null)
                {
                    SetFontInternal(rule, item);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private void SetFontInternal(FontRule rule, TagBehaiver behaiver)
        {
            if (behaiver == null) return;
            behaiver.fontRule = rule;
        }
        /// <summary>
        /// 设置方向
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="index"></param>
        public bool SetDirectionRule(DirectionRule rule, string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                ForEach((x) =>
                {
                    SetDirectionRuleInternal(rule, x);
                });
                return true;
            }
            else
            {
                TagBehaiver item;
                bool contain = tagbehavierDic.TryGetValue(key, out item);
                if (contain && item != null)
                {
                    SetDirectionRuleInternal(rule, item);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void SetDirectionRuleInternal(DirectionRule rule, TagBehaiver behaiver)
        {
            if (behaiver == null) return;
            behaiver.directionRule = rule;
        }
        /// <summary>
        /// 设置元素命名
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public bool SetName(string key, string newName)
        {
            if (tagbehavierDic.ContainsKey(key))
            {
                var item = tagbehavierDic[key];
                if (item != null)
                {
                    item.Name = newName;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 循环
        /// </summary>
        /// <param name="onRetrive"></param>
        private void ForEach(UnityAction<TagBehaiver> onRetrive)
        {
            foreach (var item in tagbehaviers)
            {
                onRetrive(item);
            }
        }
    }
}