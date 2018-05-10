using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NetworkedGameServer
{
    class LobbyHandler : MessageHandler
    {
        //Get NetCons instance and define variables
        NetCons network = NetCons.getInstance();
        MessageHandler message;
        List<String> players = new List<String>();
        System.Windows.Forms.Timer timerSend;
        System.Windows.Forms.Timer timerRecieve;
        ListBox connectedLBox;
        ListBox currentGamesLBox;
        //Defining binding source variables for listbox updates 
        BindingSource bindPlayers = new BindingSource(); //for proper updates to controls
        BindingSource bindGames = new BindingSource();

        public LobbyHandler(ListBox connectedLBox, ListBox currentGamesLBox)
        {
            this.connectedLBox = connectedLBox;
            this.currentGamesLBox = currentGamesLBox;
            connectedLBox.DisplayMember = "alias"; //Value to bind from Player
            currentGamesLBox.DisplayMember = "versus"; //Value to bind from GameHandler
            message = new MessageHandler(bindPlayers, bindGames); //Set up message handler
            bindPlayers.DataSource = message.player; //Set data source
            bindGames.DataSource = message.game; //Set data source
            connectedLBox.DataSource = bindPlayers; //bind data
            currentGamesLBox.DataSource = bindGames; //bind data
            //Initialize timers
            timerSend = new System.Windows.Forms.Timer();
            timerSend.Enabled = true;
            timerSend.Interval = 500;
            timerSend.Tick += new EventHandler(send);

            timerRecieve = new System.Windows.Forms.Timer();
            timerRecieve.Enabled = true;
            timerRecieve.Interval = 25;
            timerRecieve.Tick += new EventHandler(recieve);
            
        }

        public NetCons NetCons
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public void send(object sender, EventArgs e)
        {
            //broadcast connected players and current games periodically
            network.broadcastUDP(message.sendPlayers());
            network.broadcastUDP(message.sendGames());
        }

        public void recieve(object sender, EventArgs e)
        {
            //recieve all lobby messages from clients
            message.processMessage(network.rcvUDP());
        }
        
    }
}
