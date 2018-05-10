using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkedGameServer
{
    class GameHandler : MessageHandler
    {
        //Setting up get variables
        public bool active { get; private set; }
        public String playerA { get; private set; }
        public String playerB { get; private set; }
        public String versus { get; private set; }

        public GameCons GameCons
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
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

        //Get NetCons instance
        NetCons net = NetCons.getInstance();
        //define variables
        GameCons network; 
        GameCons network2;
        MessageHandler message;
        System.Windows.Forms.Timer timerSend;
        System.Windows.Forms.Timer timerRecieve;
        System.Windows.Forms.Timer gameTime;
        bool returnOk = true;
        
        private String ipA;
        private String ipB;

        //Size of game
        static int width = 750;
        static int height = 375;

        
        int ballForceX = 15; //will become negative when hitting top edge and positive on bottom
        int ballForceY; //becomes negative on playerBs side and positive on playerAs

        int ballx = width / 2;          //start point
        int bally = (height / 2) - 15;

        int pointsA = 0; //start points
        int pointsB = 0;

        int paddleA = (height / 2) - 35; //paddle default locations
        int paddleB = (height / 2) - 35;

        Random random = new Random(); //random number generator

        public GameHandler(GameCons network, GameCons network2, String playerA, String playerB, String ipA, String ipB)
        {
            //set game active and initialize variables
            active = true;
            this.playerA = playerA;
            this.playerB = playerB;
            versus = playerA + " vs " + playerB;
            this.network = network;
            this.network2 = network2;
            message = new MessageHandler();
            this.ipA = ipA;
            this.ipB = ipB;
            resetBall();
            //initialize timers
            timerSend = new System.Windows.Forms.Timer();
            timerSend.Enabled = true;
            timerSend.Interval = 35;
            timerSend.Tick += new EventHandler(send);

            timerRecieve = new System.Windows.Forms.Timer();
            timerRecieve.Enabled = true;
            timerRecieve.Interval = 25;
            timerRecieve.Tick += new EventHandler(recieve);

            gameTime = new System.Windows.Forms.Timer();
            gameTime.Enabled = false;
            gameTime.Interval = 40;
            gameTime.Tick += new EventHandler(gameTime_Tick);
            ballForceY = random.Next(-5, 5);
        }

        //Method to send periodically on timer
        private void send(object sender, EventArgs e)
        {
            network.sendUDP(message.gameInfo(paddleB, bally, ballx, pointsA, pointsB), ipA);
            network2.sendUDP(message.gameInfo(paddleA, bally, ballx, pointsA, pointsB), ipB);
        }
        //Method to recieve periodically on timer
        private void recieve(object sender, EventArgs e)
        {
            gameInfo(network.rcvUDP(ipA));
            gameInfo(network2.rcvUDP(ipB));
        }
        //Game time method to run periodically on timer
        private void gameTime_Tick(object sender, EventArgs e)
        {
            edgeCollision(); //Check if hits edge
            ballx += ballForceX; //Move ball
            bally += ballForceY;

            if (!returnOk) //If returnOK == false
            {
                if (ballx <= width - 35 && ballx >= 25) //Check location
                {
                    returnOk = true; //Set true
                }
            }

            if ((collisionA() || collisionB()) && returnOk) //Check if a point has been scored and if its valid
            {
                gameTime.Enabled = false; //Stop time
                network.sendUDP(message.gameInfo(paddleB, bally, ballx, pointsA, pointsB), ipA); //Send points update
                network2.sendUDP(message.gameInfo(paddleA, bally, ballx, pointsA, pointsB), ipB);                
            }
                        
            if (pointsA == 10 || pointsB == 10) //Checks if there is a winner
            {
                gameTime.Enabled = false; //Stops timers
                timerSend.Enabled = false;
                timerRecieve.Enabled = false;
                //Notify clients of winner
                network.sendUDP(generateMessage("420", pointsA.ToString(), pointsB.ToString()), ipA); 
                network2.sendUDP(generateMessage("420", pointsA.ToString(), pointsB.ToString()), ipB);
                active = false; //set game to unactive
                network.close(); //close sockets
                network2.close();
            }
        }

        private void resetBall()
        {
            //place ball in center
            ballx = (width / 2) - 3;
            bally = (height / 2) - 14;
        }
        
        //Method checks if there is a collision on side A and updates game accordingly, assigning random force 
        //in opposite direction if it is valid
        private bool collisionA()
        {
            if (bally >= paddleA - 20 && bally <= paddleA + 70 && ballx < 25)
            {
                ballForceY = random.Next(-5, 5);
                ballForceX = -1 * ballForceX;
                returnOk = false;
                return false;
            }
            else if (ballx <= width + 10)
            {
                return false;
            }
            else
            {
                resetBall();
                pointsA++;
                return true;
            }
        }

        //Method checks if there is a collision on side B and updates game accordingly, assigning random force 
        //in opposite direction if it is valid
        private bool collisionB()
        {
            if (bally >= paddleB - 20 && bally <= paddleB + 70 && ballx > width - 35)
            {
                ballForceY = random.Next(-5, 5);
                ballForceX = -1 * ballForceX;
                returnOk = false;
                return false;
            }
            else if (ballx >= -10)
            {
                return false;
            }
            else
            {
                pointsB++;
                resetBall();
                return true;
            }
        }
        private void edgeCollision()
        {
            if (bally <= 0 || bally >= height - 20)
            {
                ballForceY = -1 * ballForceY;
            }
        }

        //Game specific messages
        public void gameInfo(String message)
        {
            System.Diagnostics.Debug.WriteLine(message);
            if (message != null)
            {
                String temp = message.Substring(0, 3); //Take message code substring
                message = message.Substring(4, message.Length - 4); //Trim message code from message

                if (temp.Equals("330")) //Recieve paddle location
                {
                    String[] paddle = message.Split();
                    if (paddle[0].Equals("A"))
                    {
                        paddleA = int.Parse(paddle[1].Trim());
                    }
                    else if (paddle[0].Equals("B"))
                    {
                        paddleB = int.Parse(paddle[1].Trim());
                    }
                }
                
                if (temp.Equals("340")) //Message to begin gameTime [spacebar action from client]
                {
                    if (!gameTime.Enabled)
                    {
                        resetBall();
                        System.Diagnostics.Debug.WriteLine("340");
                        gameTime.Enabled = true;
                    }
                }

                if (temp.Equals("400")) //Client has exited the game
                {
                    //Disable all timers
                    gameTime.Enabled = false;
                    timerSend.Enabled = false;
                    timerRecieve.Enabled = false;
                    //Notify clients
                    network.sendUDP(generateMessage("410", "closed"), ipA);
                    network2.sendUDP(generateMessage("410", "closed"), ipB);
                    active = false; //game set to unactive
                    network.close(); //Close sockets
                    network2.close();
                }
            }
        }

        //Unimplemented score checker
        public String checkScore()
        {
            String score = pointsA + " - " + pointsB;
            return score;
        }

        public void streamMatch(String IP) //Request to add stream client
        {
            Task.Factory.StartNew(() => stream(IP)); //Creates a new thread to update new stream client
        }

        private void stream(String IP)
        {
            while (active) //While game is active, update stream client with game data
            {
                net.sendUDP(message.generateMessage("500", paddleA.ToString(), paddleB.ToString(), bally.ToString(), ballx.ToString(), pointsA.ToString(), pointsB.ToString()), IP);
                Thread.Sleep(40); //Synchronized with gametime
            }
            //Game ended message
            net.sendUDP(message.generateMessage("510", pointsA.ToString(), pointsB.ToString()), IP);
        }
    }
}

