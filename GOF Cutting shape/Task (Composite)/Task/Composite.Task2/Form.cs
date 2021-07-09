using System;
using System.Collections.Generic;
using System.Linq;

namespace Composite.Task2
{
    public class Form : IComponent
    {
        protected List<IComponent> _children = new List<IComponent>();

        String name;

        public Form(String name)
        {
            this.name = name;
        }

        public void AddComponent(IComponent component)
        {
            _children.Add(component);
        }

        public string ConvertToString(int depth = 0)
        {
            var tmp = depth;
            var tabs = "";
            while (tmp != 0)
            {
                tabs += " ";
                tmp--;
            }

            var result = $"{tabs}<form name='{name}'>";
            if (_children.Any())
            {
                depth++;
            }

            foreach(var child in _children)
            {
                result += $"\r\n{child.ConvertToString(depth)}";
            }

            result += $"\r\n{tabs}</form>";

            return result;
        }
    }
}