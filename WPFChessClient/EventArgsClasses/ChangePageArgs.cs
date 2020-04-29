using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChessClient.EventArgsClasses
{
    public class ChangePageArgs
    {
        public NamePage Name { get; private set; }

        public ChangePageArgs(NamePage name)
        {
            Name = name;
        }
    }
}
