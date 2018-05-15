using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Tagging
{

    public interface ITagGroupBehaiver
    {
        string GroupKey { get; }
    }
}
