using System;
using System.Collections.Generic;
using System.Text;

namespace Messages
{
    public class Messages
    {
        private Messages(string value) => Value = value; 
        public string Value { set; get; }

    }
}
