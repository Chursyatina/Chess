using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChessClient.Saving
{
    class LeaderTablePlayer
    {
        public string Name {get;set;}

        public int Wins { get; set; }

        public int Games { get; set; }
        public LeaderTablePlayer()
        {

        }
    }
}
