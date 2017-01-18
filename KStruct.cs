using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_A
{
    class KStruct
    {
        public string[] fields;
        public string name;

        public KInstance Instanciate(string name, KObject[] args)
        {
            if (args.Length != fields.Length)
                return null;
            var arguments = new KObject[fields.Length];
            for (int i = 0; i < fields.Length; ++i)
            {
                arguments[i] = new KObject() { name = fields[i], value = args[i].value };
            }
            KInstance kinst = new KInstance(name, arguments);
            return kinst;
        }
    }

    class KInstance : KObject
    {
        public KObject[] vars;
        public KInstance(string name, KObject[] vars)
        {
            this.name = name;
            this.vars = vars;
            value = this;
        }
        public KObject Get(string name)
        {
            foreach (KObject ko in vars)
            {
                if (ko.name == name)
                    return ko;
            }
            return null;
        }
    }
}
