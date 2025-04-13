using System.Drawing.Imaging;
using System.Xml.Serialization;
using Thunder_Fighter.BSLayers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Thunder_Fighter
{
    public partial class Form1 : Form
    {
        Bitmap screen;
        Graphics g;

        static string assetPath = Path.GetFullPath("../../../assets");
        string bgPath = Path.Combine(assetPath, "background/bg1.png");
        Image background;
        static int fps = 60;
        int width = 360 * 3 / 2;
        int height = 640 * 3 / 2;
        int frameCount = 0;
        int speed = 10;

        Fighter player = new Fighter("Player", 100, 100);
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.plMain.Size = new Size(width, height);
            this.Size = new Size(width + 16, height + 39);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.screen = new Bitmap(this.plMain.Width, this.plMain.Height, this.plMain.CreateGraphics());
            this.g = Graphics.FromImage(this.screen);
            background = Image.FromFile(bgPath);
            //g.DrawImage(background, new Rectangle(0, 0, plMain.Width, plMain.Height));
            this.plMain.CreateGraphics().DrawImage(this.screen, 0, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gameTimer.Interval = 1000 / fps;
            gameTimer.Start();

            string gifP = Path.Combine(assetPath, "player/Ship_body.gif");
            string gifShield = Path.Combine(assetPath, "player/Shield.gif");
            Sprite playerSprite = new Sprite();
            playerSprite.LoadGif(gifP);
            player.baseSprite = playerSprite;
            Sprite playerShield = new Sprite();
            playerShield.LoadGif(gifShield);
            player.shieldSprite = playerShield;
            player.h = 120;
            player.w = 120;
            //player.x = (int)(this.plMain.Width / 2 - player.getW() / 2);
            //player.y = (int)(this.plMain.Height - player.getH());
            player.x = width / 2 - player.getW() / 2;
            player.y = height - 50 - player.getH();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            this.update();
            this.paint();
            frameCount++;
        }

        private void update()
        {
            player.update(ref g);
        }

        private void paint()
        {
            g.DrawImage(background, new Rectangle(0, 0, plMain.Width, plMain.Height));
            if (frameCount % 4 == 0)
            {
                player.shieldSprite.index++;
            }
            player.paint(ref g);
            this.plMain.CreateGraphics().DrawImage(screen, 0, 0);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //MessageBox.Show("Key Pressed: " + e.KeyChar.ToString());
            if (e.KeyChar == (char)Keys.A)
            {
                player.move(-speed, 0);

            }
            else if (e.KeyChar == (char)Keys.D)
            {
                player.move(speed, 0);
            }
            else if (e.KeyChar == (char)Keys.W)
            {
                player.move(0, -speed);
            }
            else if (e.KeyChar == (char)Keys.S)
            {
                player.move(0, speed);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("Key Pressed: " + e.KeyCode.ToString());
        }
    }
}
