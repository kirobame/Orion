using System.Collections;
using System.Collections.Generic;

namespace Orion
{
    public abstract class Routine
    {
        public IEnumerator Call 
        {
            get
            {
                var call = GetCall();
                foreach (var link in chain) call = Chain(call, link);

                return call;
            }
        }
    
        private List<IEnumerator> chain = new List<IEnumerator>();
    
        protected abstract IEnumerator GetCall();

        public void Append(Routine routine) => chain.Add(routine.GetCall());
        public void Append(IEnumerator routine) => chain.Add(routine);

        private IEnumerator Chain(IEnumerator source, IEnumerator routine)
        {
            yield return source;
            yield return routine;
        }
    }
}