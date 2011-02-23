using System;
using System.Collections.Generic;

namespace NSpecSpec
{
    public class Tuples<T, U> : List<Tuple<T,U>> 
    {
        public void Add(T t, U u)
        {
            base.Add(new Tuple<T,U>(t,u));
        }
    }
}