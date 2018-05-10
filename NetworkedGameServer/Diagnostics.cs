using System.Windows.Forms;

namespace NetworkedGameServer
{
    public partial class Diagnostics : Form
    {
        public Diagnostics()
        {
            InitializeComponent();
            LobbyHandler lb = new LobbyHandler(connectedLBox, currentGamesLBox); //Begins lobbyhandler
        }

        internal LobbyHandler LobbyHandler
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }
}
