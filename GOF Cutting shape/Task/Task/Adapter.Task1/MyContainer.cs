﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Adapter.Task1
{
    public class MyContainer<T> : IContainer<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int Count { get; set; }
    }
}