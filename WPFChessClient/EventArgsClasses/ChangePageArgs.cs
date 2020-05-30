using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFChessClient.Interfaces;

namespace WPFChessClient.EventArgsClasses
{
    public class ChangePageArgs: IPageArgs
    {
        public NamePage Name { get; set; }

        public ChangePageArgs(NamePage name)
        {
            Name = name;
        }
    }
}
