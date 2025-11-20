using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BetterTriggers
{
    public class Trigger : MonoBehaviour
    {
        public event System.Action<Collider> OnTriggerEntered;
        public event System.Action<Collider> OnTriggerExited;
        public event System.Action<Collider> OnTriggerStayed;

        [Tooltip("If empty, all tags are allowed.")]
        [SerializeField] private List<string> allowedTags = new();

        void OnTriggerEnter(Collider collider)
        {
            if (allowedTags.Count > 0 && !allowedTags.Contains(collider.tag)) return;
            OnTriggerEntered?.Invoke(collider);
        }

        void OnTriggerExit(Collider collider)
        {
            if (allowedTags.Count > 0 && !allowedTags.Contains(collider.tag)) return;
            OnTriggerExited?.Invoke(collider);
        }

        void OnTriggerStay(Collider collider)
        {
            if (allowedTags.Count > 0 && !allowedTags.Contains(collider.tag)) return;
            OnTriggerStayed?.Invoke(collider);
        }
    }
}
