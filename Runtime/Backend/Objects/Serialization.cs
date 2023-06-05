using System.Collections.Generic;
using UnityEngine;

namespace sxr_internal
{
   
    [System.Serializable]
    public class Serialization<T>
    {
        [SerializeField]
        private List<T> target;

        public List<T> ToList() { return target; }

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }
    
}