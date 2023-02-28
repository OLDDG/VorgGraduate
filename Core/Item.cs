using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Item : IItem
    {
        public string Ffunc { get; set; }

        public string Gfunc { get; set; }

        public string RegisterSize { get; set; }

        public string InputX { get; set; }

        public string InputY { get; set; }
    }
}
