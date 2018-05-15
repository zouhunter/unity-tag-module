using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace Tagging
{
    public class TagController
    {
        private static TagController _instence;
        public static TagController Instence
        {
            get
            {
                if (_instence == null)
                {
                    _instence = new TagController();
                }
                return _instence;
            }
        }
        private List<TagGroupBehaiver> groupList = new List<TagGroupBehaiver>();
        public TagGroupBehaiver currentGroup { get; private set; }
        private Dictionary<string, UnityAction<TagGroupBehaiver>> waitDic = new Dictionary<string, UnityAction<TagGroupBehaiver>>();

        public void RegistGroup(TagGroupBehaiver group)
        {
            currentGroup = group;
            if (!groupList.Contains(group)){
                groupList.Add(group);

                if(waitDic.ContainsKey(group.GroupKey))
                {
                    var action = waitDic[group.GroupKey];
                    if(action != null)
                    {
                        action.Invoke(group);
                    }
                }
            }
        }

        public void RemoveGroup(TagGroupBehaiver group)
        {
            if (groupList.Contains(group))
            {
                groupList.Remove(group);
            }
        }

        public void RetriveAsync(string key,UnityAction<TagGroupBehaiver> onRetrive)
        {
            Debug.Assert(onRetrive == null);

            var item = groupList.Find(x => x.GroupKey == key);
            if(item != null)
            {
                onRetrive.Invoke(item);
            }
            else
            {
                if(waitDic.ContainsKey(key)){
                    waitDic[key] += onRetrive;
                }
                else{
                    waitDic[key] = onRetrive;
                }
            }
        }

        public void StopRetrive(string key,UnityAction<TagGroupBehaiver> onRetrive)
        {
            if(waitDic.ContainsKey(key))
            {
                waitDic[key] -= onRetrive;
            }
        }
    }
}