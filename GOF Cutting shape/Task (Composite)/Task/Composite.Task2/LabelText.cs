﻿using System;

namespace Composite.Task2
{
    public class LabelText : IComponent
    {
        string value;

        public LabelText(string value)
        {
            this.value = value;
        }

        public string ConvertToString(int depth = 0)
        {
            var tabs = "";
            while (depth != 0)
            {
                tabs += " ";
                depth--;
            }

            return $"{tabs}<label value='{value}'/>";
        }
    }
}
