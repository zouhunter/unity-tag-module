using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Tagging;

public class TaggingDemo : MonoBehaviour {

    [SerializeField]
    private LineRule myLineRule = new LineRule() { startColor = Color.red, endColor = Color.green, startWidth = 0.2f };
    [SerializeField]
    private FontRule myFontRule = new FontRule() { fontSize = 10, fontColor = Color.white };
    [SerializeField]
    private DirectionRule myDirectionRule = new DirectionRule() { dirType = DirectionType.Auto, angle = 45 };

    private TagGroupBehaiver group
    {
        get
        {
            return TagController.Instence.currentGroup;
        }
    }
    private void OnGUI()
    {
        if (group == null) return;
        if (GUILayout.Button("全部居右"))
        {
            myDirectionRule.dirType = DirectionType.Right;
            group.SetDirectionRule(myDirectionRule);
        }
        if (GUILayout.Button("全部居左"))
        {
            myDirectionRule.dirType = DirectionType.Left;
            group.SetDirectionRule(myDirectionRule);
        }
        if (GUILayout.Button("自由布局"))
        {
            myDirectionRule.dirType = DirectionType.Auto;
            group.SetDirectionRule(myDirectionRule);
        }
        if (GUILayout.Button("id为 1 的为红色"))
        {
            var rule = myLineRule;
            rule.startColor = rule.endColor = Color.red;
            group.SetLineRule(rule, "1");
        }
    }
    private void Update()
    {
        if (group == null) return;
        group.RefeshPosition();
    }
}
