using System;

namespace NetworkedGameServer
{
    class playerInfo
    {
        //Define variables
        public String IP { get; private set; }
        public String alias { get; private set; }

        //Set variables
        public playerInfo(String IP, String alias)
        {
            this.IP = IP;
            this.alias = alias;
        }
    }
}
