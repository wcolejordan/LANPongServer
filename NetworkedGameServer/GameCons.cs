using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace NetworkedGameServer
{
    public class GameCons
    {
        //defining variables
        Socket socketUdpRcv;
        Socket socketUdpSnd;
        int portA;
        int portB;

        public GameCons(int portA, int portB)
        {
            try
            {
                this.portA = portA;
                this.portB = portB;
                
                IPAddress IP = System.Net.IPAddress.Parse(NetCons.IPString); //Getting and parsing IP from NetCons
                IPEndPoint ClientEndPointRcv = new IPEndPoint(IP, portA); //Setting up end points
                IPEndPoint ClientEndPointSnd = new IPEndPoint(IP, portB); //Setting up end points
                socketUdpRcv = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); //initialize sockets
                socketUdpSnd = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socketUdpRcv.Blocking = false; //Setting to non blocking
                socketUdpSnd.Blocking = false;
                socketUdpRcv.Bind(ClientEndPointRcv); //binding sockets
                socketUdpSnd.Bind(ClientEndPointSnd);
            }
            catch (Exception e)
            {
              //  System.Diagnostics.Debug.WriteLine(e);
            }

        }

        public void sendUDP(byte[] data, String ip) //Take data and IP
        {
            try
            {
                IPEndPoint receiver = new IPEndPoint(IPAddress.Parse(ip), portA); //Set up end point
                socketUdpSnd.SendTo(data, receiver); //Send data
            }
            catch (Exception er)
            {
              //  System.Diagnostics.Debug.WriteLine(er);
            }
        }
        
        public String rcvUDP(String IP)
        {
            try
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Parse(IP), portB); //Setting sender details
                EndPoint receive = (EndPoint)(sender); //Setting end point
                byte[] data = new byte[1024]; //Initialize byte array
                int rcv = socketUdpRcv.ReceiveFrom(data, ref receive); //Recieve message
                String dataString = Encoding.ASCII.GetString(data, 0, rcv); //Convert byte array to string
                return dataString; //return
            }
            catch (Exception er)
            {
              //  System.Diagnostics.Debug.WriteLine(er);
                return null;
            }
        }

        //For when game ends, close sockets
        public void close()
        {
            socketUdpRcv.Close();
            socketUdpSnd.Close();
        }
    }
}