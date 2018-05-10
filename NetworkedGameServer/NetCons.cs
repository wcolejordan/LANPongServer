using System;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;

namespace NetworkedGameServer
{
    public class NetCons
    {
        //define variables and initialize instance
        private static NetCons instance = null;
        Socket socketUdpRcv;
        Socket socketUdpSnd;
        //Get set for IP
        public static String IPString { get; private set; }

        //Singleton implementation, checks if there is an instance and if not, creates one
        public static NetCons getInstance(){
            if (instance == null){
                instance = new NetCons();
            }
            return instance;
        }

        private NetCons() //Can only be created through getInstance method
        {
            try
            {
                //Find default adapter and set IP of it. Machines have different names for the interface hence all the compares
                var local = NetworkInterface.GetAllNetworkInterfaces().Where(i => i.Name == "Local Area Connection" || i.Name == "Local Area Connection 2" || i.Name == "Local Area Connection 3" || i.Name == "Ethernet" || i.Name == "Local Area Network").FirstOrDefault();
                if (local.GetIPProperties().UnicastAddresses[0].Address.ToString().Length <= 15)
                {
                    IPString = local.GetIPProperties().UnicastAddresses[0].Address.ToString(); //Checks it has not recieved IPv6 address
                }
                else
                {
                    IPString = local.GetIPProperties().UnicastAddresses[1].Address.ToString(); //If not, this will be the IPv4 location
                }
                IPAddress IP = System.Net.IPAddress.Parse(IPString); //Parse IP address
                IPEndPoint ClientEndPointRcv = new IPEndPoint(IP, 10001); //Setting up end points
                IPEndPoint ClientEndPointSnd = new IPEndPoint(IP, 10011); //Setting up end points
                //Initialize sockets
                socketUdpRcv = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socketUdpSnd = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socketUdpRcv.Blocking = false;
                socketUdpSnd.Blocking = false;
                socketUdpRcv.Bind(ClientEndPointRcv);
                socketUdpSnd.Bind(ClientEndPointSnd);
            }
            catch (Exception e)
            {
               // System.Diagnostics.Debug.WriteLine(e);
            }

        }

        public void sendUDP(byte[] data, String ip)
        {
            try
            {
                IPEndPoint receiver = new IPEndPoint(IPAddress.Parse(ip), 10002); //Set end point
                socketUdpSnd.SendTo(data, receiver); //send data
            }
            catch (Exception er)
            {
               // System.Diagnostics.Debug.WriteLine(er);
            }
        }

        public void broadcastUDP(byte[] data)
        {
            try
            {
                //Check last 8 bits and convert to broadcast address
                String[] lastBits = NetCons.IPString.Split('.');
                String broadcastAddress = NetCons.IPString.Remove(NetCons.IPString.Length - lastBits[3].Length);
                broadcastAddress += "255"; 
                IPEndPoint receiver = new IPEndPoint(IPAddress.Parse(broadcastAddress), 10002); //set endpoint as broadcast address
                socketUdpSnd.SendTo(data, receiver); //Send data
            }
            catch (Exception er)
            {
               // System.Diagnostics.Debug.WriteLine(er);
            }
        }

        public String rcvUDP()
        {
            try
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 10012); //Setting sender details
                EndPoint receive = (EndPoint)(sender); //Set end point
                byte[] data = new byte[1024]; //Initialize byte array
                int rcv = socketUdpRcv.ReceiveFrom(data, ref receive); //Recieve data
                String dataString = Encoding.ASCII.GetString(data, 0, rcv);//Convert to string
                return dataString;//Return
            }
            catch (Exception er)
            {
              //  System.Diagnostics.Debug.WriteLine(er);
                return null;
            }
        }


        public void close()
        {
            //Close sockets
            socketUdpRcv.Close();
            socketUdpSnd.Close();
        }
    }
}