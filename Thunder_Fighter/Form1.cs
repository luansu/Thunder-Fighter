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

        List<EnemyWave> enemyWaves = new List<EnemyWave>();
        List<Enemy> enemies = new List<Enemy>();
        Random rand = new Random();
        DateTime startTime = DateTime.Now;

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
            TimeSpan elapsed = DateTime.Now - startTime;

            // Spawn small enemy wave mỗi 2 giây
            if (elapsed.TotalSeconds < 30)
            {
                if (frameCount % 60 == 0)
                    spawnEnemyWave();
            }
            // Spawn big enemy 1 lần sau 30s
            else if (elapsed.TotalSeconds < 60)
            {
                if (frameCount % 180 == 0 && enemies.All(e => e.type != 1))
                    spawnEnemySingle(1);
            }
            // Spawn boss duy nhất sau 60s
            else
            {
                if (enemies.All(e => e.type != 2))
                    spawnEnemySingle(2);
            }

            // Cập nhật enemy wave
            foreach (var wave in enemyWaves)
                wave.update();

            // Cập nhật các enemy khác (big & boss)
            foreach (var e in enemies.Where(e => e.type != 0))
                e.update();

            // Cập nhật player
            player.update(ref g);
        }

        private void paint()
        {
            // Vẽ nền
            g.DrawImage(background, new Rectangle(0, 0, plMain.Width, plMain.Height));

            // Animation khiên
            if (frameCount % 4 == 0)
                player.shieldSprite.index++;

            // Vẽ enemy wave (small enemies)
            foreach (var wave in enemyWaves)
                wave.paint(ref g);

            // Vẽ big enemy và boss
            foreach (var e in enemies.Where(e => e.type != 0))
                e.paint(ref g);

            // Vẽ player
            player.paint(ref g);

            // Render toàn bộ
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

        private void spawnEnemyWave()
        {
            int count = rand.Next(3, 5);
            string path = Path.Combine(assetPath, "enemy1/base/Kla'ed - Scout - Engine.gif");
            Sprite sprite = new Sprite();
            sprite.LoadGif(path);

            int w = 100, h = 100;
            int health = 10;
            int startY = rand.Next(30, 100);
            int startX = rand.Next(0,plMain.Width - count * (w + 10));

            List<Enemy> waveMembers = new List<Enemy>();

            for (int i = 0; i < count; i++)
            {
                int x = startX + i * (w + 5);
                var e = new Enemy(0, x, startY, w, h, health, sprite)
                {
                    dx = 0,
                    dy = 0,
                    movementMode = 1 // not used here
                };
                waveMembers.Add(e);
                enemies.Add(e); // để dễ quản lý tất cả enemy
            }

            enemyWaves.Add(new EnemyWave(waveMembers, plMain.Width));
        }

        private void spawnEnemySingle(int type)
        {
            string path = type == 1
                ? Path.Combine(assetPath, "enemy1/base/Kla'ed - Battlecruiser - Engine.gif")
                : Path.Combine(assetPath, "enemy1/base/Kla'ed - Dreadnought - Engine.gif");

            Sprite sprite = new Sprite();
            sprite.LoadGif(path);

            int w = type == 1 ? 250 : 350;
            int h = w;
            int health = type == 1 ? 30 : 300;
            int x = rand.Next(0, plMain.Width - w);
            int y = rand.Next(30, 100); // ở trên cùng màn hình

            var e = new Enemy(type, x, y, w, h, health, sprite)
            {
                dx = rand.Next(1, 3) * (rand.Next(0, 2) == 0 ? -1 : 1),
                dy = 0,
                minY = y,
                maxY = y,
                movementMode = 1 // horizontal only
            };

            enemies.Add(e);
        }
    }
}

class EnemyWave
{
    public List<Enemy> members = new List<Enemy>();
    public int dx = 2;
    public int dy = 0;
    public int minX = 0;
    public int maxX;
    public int minY = 20;
    public int maxY = 120;

    public EnemyWave(List<Enemy> enemies, int panelWidth)
    {
        this.members = enemies;
        this.maxX = panelWidth;
    }

    public void update()
    {
        // di chuyển cả nhóm theo hướng dx
        foreach (var enemy in members)
        {
            enemy.x += dx;
            // có thể thêm dy nếu muốn zigzag nhẹ
        }

        // Nếu chạm biên thì đổi hướng
        int left = members.Min(e => e.x);
        int right = members.Max(e => e.x + e.w);

        if (left <= minX || right >= maxX)
        {
            dx = -dx;
        }
    }

    public void paint(ref Graphics g)
    {
        foreach (var enemy in members)
            enemy.paint(ref g);
    }
}
