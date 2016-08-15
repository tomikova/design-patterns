﻿using System;

namespace TemplateMethod
{
    class Tea : Beverage
    {
        public override void Brew()
        {
            Console.WriteLine("Adding a teabag");
        }

        public override void AddCondiment()
        {
            Console.WriteLine("Adding lemon");
        }
    }
}
