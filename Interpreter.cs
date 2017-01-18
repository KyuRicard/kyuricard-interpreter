using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_A
{
    class Interpreter
    {
        static Dictionary<string, Action<string[]>> functions = new Dictionary<string, Action<string[]>>();
        static Dictionary<string, KObject> objects = new Dictionary<string, KObject>();
        static Dictionary<string, KStruct> structs = new Dictionary<string, KStruct>();
        static KObject[] numericRegisters = new KObject[8];
        static KObject[] decimalRegisters = new KObject[6];
        static KObject[] textRegisters = new KObject[2];

        public static void Interprete(string[] lines)
        {
            string function = lines[0];
            string[] args = new string[lines.Length - 1];
            for (int i = 1; i < lines.Length; ++i)
                args[i - 1] = lines[i];
            if (functions.ContainsKey(function))
                functions[function].Invoke(args);
            else if (function == "EVAL")
            {
                string line = "";
                for (int i = 0; i < args.Length - 1; ++i)
                    line += args[i] + " ";
                line += args[args.Length - 1];
                ExpressionEvaluator.EvaluateExpression(line);
            }
        }

        public static void Init()
        {
            functions.Add("EXIT", (args) =>
            {
                Environment.Exit(0);
            });
            functions.Add("SET", (args) =>
            {
                if (args.Length < 1)
                    return;
                if (objects.ContainsKey(args[0]))
                {
                    objects.Remove(args[0]);
                }
                if (args.Length == 1)
                {
                    objects.Add(args[0], new KObject() { name = args[0], value = new object() });
                }
                else
                {
                    KObject obj = new KObject()
                    {
                        name = args[0],
                        value = new object()
                    };
                    if (int.TryParse(args[1], out int i))
                    {
                        obj.value = i;
                    }
                    else if (float.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float f))
                    {
                        obj.value = f;
                    }
                    else
                    {
                        string text = "";
                        if (args.Length > 2)
                        {
                            for (int j = 1; j < args.Length - 1; ++j)
                                text += args[j] + " ";
                            text += args[args.Length - 1];
                        }
                        else
                            text = args[1];
                        obj.value = text;
                    }
                    objects.Add(obj.name, obj);
                }
            });
            functions.Add("PRINT", (args) =>
            {
                if (args.Length < 1)
                    return;
                if (objects.ContainsKey(args[0]))
                {
                    var obj = objects[args[0]];
                    Console.WriteLine($"{obj.name}: {obj.value}");
                }
            });
            functions.Add("PARSE", (args) =>
            {
                //PARSE type from to
                if (args.Length != 3)
                    return;
                switch (args[0])
                {
                    case "INT":
                        break;
                    case "FLOAT":
                        break;
                    case "STR":
                        break;
                }
            });
            functions.Add("TYPE", (args) =>
            {
                if (args.Length < 1)
                    return;
                if (objects.ContainsKey(args[0]))
                {
                    var obj = objects[args[0]];
                    if (obj.value.GetType() == typeof(Int32))
                    {
                        Console.WriteLine($"Int32 {obj.name}");
                    }
                    else if (obj.value.GetType() == typeof(Single))
                    {
                        Console.WriteLine($"Single {obj.name}");
                    }
                    else if (obj.value.GetType() == typeof(String))
                    {
                        Console.WriteLine($"String {obj.name}");
                    }
                    else
                    {
                        Console.WriteLine($"Object {obj.name}");
                    }
                }
            });
            functions.Add("READ", (args) =>
            {
                if (args.Length < 1)
                    return;
                if (objects.ContainsKey(args[0]))
                {
                    var tmp = Console.ReadLine();
                    objects[args[0]].value = tmp;
                }
            });
            functions.Add("NEW", (args) =>
            {
                //Check if the struct EXISTS. 0->name
                if (!structs.ContainsKey(args[0]))
                    return;
                //Pack all args. If an arg isn't a var pack. arg > 1
                var arguments = new KObject[args.Length - 2];
                for (int i = 2; i < args.Length; ++i)
                {
                    if (objects.ContainsKey(args[i]))
                    {
                        arguments[i - 2] = objects[args[i]];
                    }
                    else
                    {
                        KObject kobj = new KObject($"tmp_{i}");
                        kobj.PushValue(args[i]);
                        arguments[i - 2] = kobj;
                    }
                }
                //Create struct
                var str = structs[args[0]].Instanciate(args[1], arguments);
                objects.Add(str.name, str);
            });
            functions.Add("STRUCT", (args) =>
            {
                if (args.Length < 1)
                    return;
                KStruct ks = new KStruct()
                {
                    name = args[0]
                };
                ks.fields = new string[args.Length - 1];
                for (int i = 1; i < args.Length; ++i)
                {
                    ks.fields[i - 1] = args[i];
                }
                structs.Add(ks.name, ks);
            });
            functions.Add("GET", (args) =>
            {
                //GET from var to
                if (args.Length != 3)
                    return;
                objects[args[2]] = ((KInstance)objects[args[0]]).Get(args[1]);
            });
            functions.Add("SETREG", (args) =>
            {
                if (args.Length < 3)
                    return;
                //R0->R8 integers
                //F0->F6 decimals
                //T0 & T1 text
                int.TryParse(args[1], out int index);
                switch (args[0][0])
                {
                    case 'R':
                        {
                            if (index >= numericRegisters.Length)
                                return;
                            int.TryParse(args[2], out int value);
                            numericRegisters[index].value = value;
                            break;
                        }
                    case 'F':
                        {
                            if (index >= decimalRegisters.Length)
                                return;
                            float.TryParse(args[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float value);
                            decimalRegisters[index].value = value;
                            break;
                        }
                    case 'T':
                        if (index >= textRegisters.Length)
                            return;
                        textRegisters[index].value = args[2];
                        break;
                }
            });
        }

    }
}
