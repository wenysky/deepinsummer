using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Events
{
    public class ReturnCompletedEventArgs : EventArgs
    {
        public object ReturnObject { get; set; }

        public ReturnCompletedEventArgs(object returnObj)
        {
            this.ReturnObject = returnObj;
        }
    }
}
