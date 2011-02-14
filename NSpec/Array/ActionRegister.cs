using System;

namespace NSpec.Array
{
    public class ActionRegister 
    {
        public Action this[string indexer]
        {
            get
            {
                //got to return the correct instance of some builder
                return null;
            } 
            set
            {
                //got to assign the action to the correct builder instance
            }
        }
    }
}