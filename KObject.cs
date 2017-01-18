using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_A
{
    class KObject
    {
        public string name;
        public object value;

        public KObject()
        {

        }

        public KObject(string name, int i)
        {
            this.name = name;
            value = i;
        }

        public KObject(string name, float f)
        {
            this.name = name;
            value = f;
        }

        public KObject(string name, string str)
        {
            this.name = name;
            value = str;
        }

        public KObject(string name)
        {
            this.name = name;
        }

        public void PushValue(string value)
        {
            if (int.TryParse(value, out int i))
            {
                this.value = i;
            }
            else if (float.TryParse(value, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out float f))
            {
                this.value = f;
            }
            else
            {
                this.value = value;
            }
        }
    }
}
