using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IItem
    {
        /// <summary>
        /// 
        /// </summary>
        string Ffunc { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        string Gfunc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string RegisterSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string InputX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string InputY { get; set; }
    }
}
