using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Thunder_Fighter.BSLayers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Thunder_Fighter
{
    public partial class Form1 : Form
    {
        Bitmap screen;
        Graphics g;

        public static string assetPath = Path.GetFullPath("../../../assets");
        GBackground bg = new GBackground();
        static int fps = 60;
        int width = 360 * 3 / 2;
        int height = 640 * 3 / 2;
        public static int frameCount = 0;
        int speed = 10;

        Fighter player = new Fighter("Player", 100, 0);
        List<Enemy> enemies = new List<Enemy>();
        List<Enemy> currentWave = new List<Enemy>();
        List<Dart> allEnemyBullets = new List<Dart>();
        Random rand = new Random();
        DateTime startTime = DateTime.Now;
        double lastBigEnemySpawnTime = -10;

        int bigEnemyCount = 0;
        bool hasTriggeredMassDestruction = false;
        bool hasSpawnedBoss = false;
        bool stopEnemySpawning = false;
        int bossSpawnCountdown = 0;

        bool isPause = false;
        bool isGameOver = false;

        public static FontFamily fontFamily;

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
            this.plMain.CreateGraphics().DrawImage(this.screen, 0, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gameTimer.Interval = 1000 / fps;
            gameTimer.Start();

            loadPlayerStatus();

            spawnSmallEnemyWave();

            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(Resource.FONT);
            fontFamily = pfc.Families[0];
        }

        private void loadPlayerStatus()
        {
            string gifP = Resource.SHIP_BODY;
            string gifShield = Resource.SHIELD;
            Sprite playerSprite = new Sprite();
            playerSprite.LoadGif(gifP);
            player.baseSprite = playerSprite;
            Sprite playerShield = new Sprite(Resource.SHIELD_F);
            player.shieldSprite = playerShield;
            Fighter.h = 120;
            Fighter.w = 120;
            Fighter.x = width / 2 - player.getW() / 2;
            Fighter.y = height - 50 - player.getH();
            player.loadEff();
            Engine pEngine = new Engine(2);
            player.engine = pEngine;
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(Fighter.x, Fighter.y);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (!this.isGameOver)
            {
                this.update();
                this.paint();
                frameCount++;
            }
            else
            {
                this.paintOV();
            }
        }

        private async Task update()
        {
            double secondsElapsed = (DateTime.Now - startTime).TotalSeconds;
            bg.update(this.plMain.Width, this.plMain.Height);

            if (!stopEnemySpawning && secondsElapsed >= 20 && (secondsElapsed - lastBigEnemySpawnTime >= 20))
            {
                spawnBigEnemy();
                lastBigEnemySpawnTime = secondsElapsed;
            }

            if (bossSpawnCountdown > 0)
            {
                bossSpawnCountdown--;
                if (bossSpawnCountdown == 0 && !hasSpawnedBoss)
                {
                    spawnBossEnemy();
                    hasSpawnedBoss = true;
                }
            }

            foreach (var enemy in enemies)
                enemy.Update();

            enemies.RemoveAll(e => e.isDead && !e.isExploding);

            // Gom tất cả đạn enemy
            allEnemyBullets.Clear();
            foreach (var enemy in enemies)
                allEnemyBullets.AddRange(enemy.bullets);

            // Kiểm tra va chạm đạn với player
            foreach (var b in allEnemyBullets)
            {
                if (player.isCollide(b))
                {
                    player.getHit(b);
                }
            }
            allEnemyBullets.RemoveAll(b => b.y > 1000);

            if (!stopEnemySpawning && currentWave.Count > 0 && currentWave.All(e => e.isDead))
            {
                spawnSmallEnemyWave();
            }
            player.update();
            if(Fighter.status == 4)
            {
                //Thread.Sleep(2000);
                this.isGameOver = true;
            }

        }

        private void paint()
        {
            bg.paint(ref g, this.plMain.Width, this.plMain.Height);

            foreach (var enemy in enemies)
                enemy.Paint(ref g);

            foreach (var b in allEnemyBullets)
                b.paint(ref g);

            player.paint(ref g);
            this.plMain.CreateGraphics().DrawImage(screen, 0, 0);
        }

        public void paintOV()
        {
            GameText.show(ref g, "Game Over", 40, this.plMain.Height / 2 - 50, 40);
            this.plMain.CreateGraphics().DrawImage(screen, 0, 0);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.A) player.move(-speed, 0);
            else if (e.KeyChar == (char)Keys.D) player.move(speed, 0);
            else if (e.KeyChar == (char)Keys.W) player.move(0, -speed);
            else if (e.KeyChar == (char)Keys.S) player.move(0, speed);
            //Test destruction của enemy
            //else if (e.KeyChar == 'k' || e.KeyChar == 'K')
            //{
            //    if (enemies.Count > 0 && !enemies[0].isDead)
            //        enemies[0].TakeDamage(999);
            //}
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void spawnSmallEnemyWave()
        {
            currentWave.Clear();

            int count = rand.Next(3, 5);
            int enemyWidth = 100;
            int spacing = 10;
            int totalWidth = count * enemyWidth + (count - 1) * spacing;
            int startX = rand.Next(0, Math.Max(1, plMain.Width - totalWidth));
            int y = rand.Next(30, 100);

            for (int i = 0; i < count; i++)
            {
                int x = startX + i * (enemyWidth + spacing);
                Enemy enemy = new SmallEnemy(x, y);
                enemies.Add(enemy);
                currentWave.Add(enemy);
            }
        }

        private void spawnBigEnemy()
        {
            if (stopEnemySpawning) return;

            int side = rand.Next(0, 2);
            int y = rand.Next(30, 80);
            Enemy big = new BigEnemy(side, y);
            enemies.Add(big);

            bigEnemyCount++;
            if (bigEnemyCount == 4 && !hasTriggeredMassDestruction)
            {
                triggerMassDestruction();
            }
        }

        private void triggerMassDestruction()
        {
            foreach (var enemy in enemies)
            {
                if (!enemy.isDead)
                    enemy.TakeDamage(999);
            }

            hasTriggeredMassDestruction = true;
            stopEnemySpawning = true;
            bossSpawnCountdown = fps * 2;
        }

        private void spawnBossEnemy()
        {
            int x = plMain.Width / 2 - 175;
            int y = -400;
            Enemy boss = new BossEnemy(x, y);
            enemies.Add(boss);
        }

        private bool IsColliding(IObject a, IObject b)
        {
            Rectangle ra = new Rectangle(a.getX(), a.getY(), a.getW(), a.getH());
            Rectangle rb = new Rectangle(b.getX(), b.getY(), b.getW(), b.getH());
            return ra.IntersectsWith(rb);
        }

        private void plMain_MouseMove(object sender, MouseEventArgs e)
        {
            player.moveTo(e.X - player.getW() / 2, e.Y - player.getH() / 2);
        }
    }
}
