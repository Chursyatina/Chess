﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFChessClient.EventArgsClasses;

namespace WPFChessClient.Interfaces
{
    public interface IPageChanger
    {
        event EventHandler<IPageArgs> PageChanged;

        void Start();
        
    }
}
