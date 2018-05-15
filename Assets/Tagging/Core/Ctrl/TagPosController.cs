using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Tagging
{
    /// <summary>
    /// 用于分配各个behaiver的索引坐标
    /// </summary>
    public class TagPosController:ITagPosCtrl
    {
        private TagBehaiver[] tagbehaivers;

        public TagPosController(TagBehaiver[] tgbs)
        {
            this.tagbehaivers = tgbs;
        }

        public void Refesh()
        {
            foreach (var item in tagbehaivers)
            {
                item.ResetTagPos(item.transform.position);
            }
        }
    }

}