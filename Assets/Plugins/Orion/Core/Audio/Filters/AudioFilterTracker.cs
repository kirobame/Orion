using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Orion
{
    
    public class AudioFilterTracker : MonoBehaviour
    {
        private List<AudioFilter> filters = new List<AudioFilter>();

        public void Set(AudioSource audioSource, IReadOnlyList<AudioFilter> filters)
        {
            var indices = new List<int>();
            for (var i = 0; i < filters.Count; i++) indices.Add(i);

            var removals = new List<AudioFilter>();
            foreach (var ownedFilter in this.filters)
            {
                var matchingIndex = -1;
                for (var i = 0; i < indices.Count; i++)
                {
                    if (filters[indices[i]].GetType() != ownedFilter.GetType()) continue;
                    
                    matchingIndex = indices[i];
                    break;
                }

                if (matchingIndex == -1)
                {
                    removals.Add(ownedFilter);
                    continue;
                }
                
                filters[matchingIndex].AssignValues(audioSource.gameObject);
                indices.Remove(matchingIndex);
            }

            foreach (var removal in removals)
            {
                removal.RemoveFrom(audioSource.gameObject);
                this.filters.Remove(removal);
            }
            
            for (var i = 0; i < indices.Count; i++)
            {
                var filter = filters[indices[i]];
                
                filter.AddTo(audioSource.gameObject);
                this.filters.Add(filter);
            }
        }

        public void Clear(AudioSource audioSource)
        {
            foreach (var filter in filters) filter.RemoveFrom(audioSource.gameObject);
            filters.Clear();
        }
    }
}