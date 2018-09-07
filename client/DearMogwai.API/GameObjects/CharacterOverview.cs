using System;
using System.Collections.Generic;
using System.Text;

namespace DearMogwai.API.GameObjects
{
    public class CharacterOverview
    {
        public string Address { get; set; }
        public bool IsBound { get; set; }
        public decimal Funds { get; set; }
        public string Name { get; set; }
        public decimal Rating { get; set; }
        public int Level { get; set; }
        public decimal Gold { get; set; }
    }
}
