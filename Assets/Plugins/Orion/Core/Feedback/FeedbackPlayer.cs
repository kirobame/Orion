using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class FeedbackPlayer : SerializedMonoBehaviour
    {
        [SerializeField] private Feedback[] feedbacks = new Feedback[0];

        private Coroutine routine;
        private int currentIndex = -1;

        [Button]
        public void Play()
        {
            if (!feedbacks.Any()) return;
            
            Stop();
            currentIndex = 0;

            foreach (var item in feedbacks) item.Prepare();
            
            var feedback = feedbacks.First();
            Debug.Log($"Beginning with : {feedback}");
            
            feedback.onCompletion += PlayNext;
            routine = StartCoroutine(feedback.GetRoutine());
        }
        public void Stop()
        {
            if (routine != null) StopCoroutine(routine);
            
            currentIndex = -1;
            routine = null;
        }

        private void PlayNext()
        {
            Debug.Log($"Ending : {feedbacks[currentIndex]}");
            
            feedbacks[currentIndex].onCompletion -= PlayNext;
            if (!feedbacks[currentIndex].GetNextIndex(currentIndex, feedbacks, out currentIndex))
            {
                Stop();
                return;
            }
            
            Debug.Log($"Playing : {feedbacks[currentIndex]}");
            
            feedbacks[currentIndex].onCompletion += PlayNext;
            routine = StartCoroutine(feedbacks[currentIndex].GetRoutine());
        }
    }
}