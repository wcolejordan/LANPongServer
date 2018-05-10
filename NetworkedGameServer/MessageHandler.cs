using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetworkedGameServer
{
    class MessageHandler
    {
        //define variables and lists
        public List<playerInfo> player;
        public List<GameHandler> game;
        BindingSource bindPlayers;
        BindingSource bindGames;
        int currentPort = 10250; //start port for game instances

        NetCons network = NetCons.getInstance(); //Get NetCons instance
        
        public MessageHandler(BindingSource bindPlayers, BindingSource bindGames)
        {
            //initialize sources and lists
            this.bindPlayers = bindPlayers;
            this.bindGames = bindGames;
            player = new List<playerInfo>();
            game = new List<GameHandler>();
        }

        public MessageHandler() //Alternative constructor
        {
            player = new List<playerInfo>();
            game = new List<GameHandler>();
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

        internal playerInfo playerInfo
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal GameHandler GameHandler
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public byte[] generateMessage(String code, String overload1) //1 variable message
        {
            String temp = code + " " + overload1; //concatonate string
            byte[] data = Encoding.ASCII.GetBytes(temp); //convert to bytes 
            return data; //returns message
        }

        public byte[] generateMessage(String code, String overload1, String overload2) //2 variable message
        {
            String temp = code + " " + overload1 + " " + overload2; //concatonate string
            byte[] data = Encoding.ASCII.GetBytes(temp); //convert to bytes 
            return data; //returns message
        }

        public byte[] generateMessage(String code, String overload1, String overload2, String overload3) //3 variable message
        {
            String temp = code + " " + overload1 + " " + overload2 + " " + overload3;//concatonate string
            byte[] data = Encoding.ASCII.GetBytes(temp); //convert to bytes 
            return data; //return message
        }

        public byte[] generateMessage(String code, String overload1, String overload2, String overload3, String overload4) //4 variable message
        {
            String temp = code + " " + overload1 + " " + overload2 + " " + overload3 + " " + overload4;//concatonate string
            byte[] data = Encoding.ASCII.GetBytes(temp); //convert to bytes 
            return data; //return message
        }

        public byte[] generateMessage(String code, String overload1, String overload2, String overload3, String overload4, String overload5, String overload6) //6 variable message
        {
            String temp = code + " " + overload1 + " " + overload2 + " " + overload3 + " " + overload4 + " " + overload5 + " " + overload6;//concatonate string
            byte[] data = Encoding.ASCII.GetBytes(temp); //convert to bytes 
            return data; //return message
        }

        public byte[] sendPlayers() //broadcast connected players message
        {
            if (player != null) //if players are connected
            {
                int cp = player.Count; //length of array cp current players
                String temp = "200" + " "; //initialise temp String
                for (int i = 0; i < cp; i++)//for each player in count
                {
                    temp += player[i].alias + " " + player[i].IP + " "; //concatonate string
                }
                byte[] data = Encoding.ASCII.GetBytes(temp); //convert to bytes 
                return data; //return message
            }
            else
            {
                return null;
            }
            
        }

        public byte[] sendGames() //broadcast message
        {
            String temp = "250 "; //initialise temp String
            if (game.Count > 0) //if games are playing
            {
                for (int i = 0; i < game.Count; i++)
                {
                    if(game[i].active == true) //checking for active games
                    {
                        temp += game[i].playerA + " " + game[i].playerB + " ";
                    }
                    else //remove unactive and notify clients
                    {
                        network.broadcastUDP(generateMessage("255", game[i].playerA, game[i].playerB)); //notify clients game is no longer active
                        game.RemoveAt(i); //remove game
                        bindGames.ResetBindings(false);
                    }
                }
            }
            byte[] data = Encoding.ASCII.GetBytes(temp); //convert to bytes 
            return data; //return message
        }

        public byte[] gameInfo(int paddle, int ballx, int bally, int pointsA, int pointsB) //Message to send gamedata
        {
            String temp = "300" + " " + paddle + " " + ballx + " " + bally + " " + pointsA + " " + pointsB;
            System.Diagnostics.Debug.WriteLine(temp); //Diagnostic output
            byte[] data = Encoding.ASCII.GetBytes(temp); //convert to byte array
            return data;
        }

        public void processMessage(String message)
        {
            if (message != null) //if there is a message
            {
                String temp = message.Substring(0, 3); //Take message code
                message = message.Substring(4, message.Length - 4); //remove message code from message

                if (temp.Equals("105")) //Server Discovery
                {
                    String[] temp2 = message.Split(' ');
                    String IP = temp2[0];
                    System.Diagnostics.Debug.WriteLine(IP);
                    network.sendUDP(generateMessage("106", NetCons.IPString), IP); //Return server IP to sender
                }

                if (temp.Equals("100")) //Join lobby
                {
                    String[] temp2 = message.Split(' '); //Separate message
                    String IP = temp2[1];
                    String alias = temp2[0];
                    if (player.FirstOrDefault(o => o.alias.Equals(temp2[0])) == null) //Checks alias not taken
                    {
                        player.Add(new playerInfo(IP, alias)); //Adds to player base
                        bindPlayers.ResetBindings(false); //Updates list
                    }
                }

                else if (temp.Equals("110")) //Leave lobby
                {
                    var tempPlayer = player.FirstOrDefault(o => o.alias.Equals(message)); //finds record which matches alias
                    if (tempPlayer != null)//if it has found a player / stops duplicate message causing issue
                    {
                        network.broadcastUDP(generateMessage("205", tempPlayer.alias)); //notify clients
                        player.Remove(tempPlayer); //deletes the record
                        bindPlayers.ResetBindings(false); //update list
                    }
                }

                else if (temp.Equals("120")) //game invite
                {
                    String[] tempAliases = message.Split(); //Split message
                    String tempA = tempAliases[0];
                    String tempB = tempAliases[1].Trim(); //Trim any trailing space
                    var infoB = player.FirstOrDefault(o => o.alias.Equals(tempB)); //get details to send to
                    network.sendUDP(generateMessage("210", tempA, tempB), infoB.IP); //send game invite to recipient
                }
                else if (temp.Equals("130")) //game accept
                {
                    String[] tempInfo = message.Split();
                    String tempA = tempInfo[0];
                    String tempB = tempInfo[1].Trim();
                    var infoA = player.FirstOrDefault(o => o.alias.Equals(tempA)); //find records
                    var infoB = player.FirstOrDefault(o => o.alias.Equals(tempB));
                    //Get game ports
                    int portA = getGamePort();
                    int portB = getGamePort();
                    int portC = getGamePort();
                    int portD = getGamePort();
                    GameCons net = new GameCons(portA, portB); //Set up GameCons instances
                    GameCons net2 = new GameCons(portC, portD);
                    //Notify clients to start games with all relevant data
                    network.sendUDP(generateMessage("320", "A", portA.ToString(), portB.ToString(), tempB), infoA.IP);
                    network.sendUDP(generateMessage("320", "B", portC.ToString(), portD.ToString(), tempA), infoB.IP);
                    //Create game instance and add to list
                    game.Add(new GameHandler(net, net2, tempA, tempB, infoA.IP, infoB.IP));
                    bindGames.ResetBindings(false); //update listbox
                }
                else if (temp.Equals("140")) //broadcast message to chat
                {
                    String[] tempChat = message.Split();
                    String tempA = tempChat[0];
                    int tempInt = tempA.Length + 1; //find start index of message for substring
                    String tempMessage = message.Substring(tempInt);
                    network.broadcastUDP(generateMessage("220", tempA, tempMessage)); //broadcast to all clients
                }
                else if (temp.Equals("150")) //private message
                {
                    String[] tempChat = message.Split();
                    String tempA = tempChat[0];
                    String tempB = tempChat[1].Trim();
                    int tempInt = tempA.Length + tempB.Length + 2; //find start index of message for substring
                    String tempMessage = message.Substring(tempInt);
                    var infoB = player.FirstOrDefault(o => o.alias.Equals(tempB)); //get details to send to
                    System.Diagnostics.Debug.WriteLine(infoB.IP);
                    //Send to recipient
                    network.sendUDP(generateMessage("230", tempA, tempMessage), infoB.IP);
                }
                else if (temp.Equals("160")) //begin stream
                {
                    String[] tempChat = message.Split();
                    String IP = tempChat[0];
                    String tempA = tempChat[1].Trim();
                    String tempB = tempChat[2].Trim();
                    //find game
                    var getInstance = game.FirstOrDefault(o => o.playerA.Equals(tempA) && o.playerB.Equals(tempB)); //get details to send to
                    //Start streaming to senders IP
                    getInstance.streamMatch(IP);
                }
            }
        }
        

        public int getGamePort()
        {
            int temp = currentPort;
            currentPort += 1; //Adds to current port
            if (currentPort > 30000) //If it ever reaches 30,000 will reset port counter.
            {
                currentPort = 10250;
            }
            return temp; //returns a port
        }

    }
}
