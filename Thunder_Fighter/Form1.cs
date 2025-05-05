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
        public static int width = 360 * 3 / 2;
        public static int height = 640 * 3 / 2;
        public static int frameCount = 0;
        int speed = 10;

        Fighter player = new Fighter("Player", 100, 0);
        List<Enemy> enemies = new List<Enemy>();
        List<Enemy> currentWave = new List<Enemy>();
        List<Dart> allEnemyBullets = new List<Dart>();
        List<Item> items = new List<Item>();
        Random rand = new Random();
        DateTime startTime = DateTime.Now;
        DateTime CompleteWave = DateTime.Now;
        double lastBigEnemySpawnTime = -10;
        bool showWaveText = false;
        DateTime waveTextStartTime;
        string waveTextContent = "";
        int waveTextDuration = 2000; // 2 giây
        bool wave1Shown = false;
        int wave = 0;


        int bigEnemyCount = 0;
        int smallEnemyWaveCount = 0;
        bool hasSpawnedBoss = false;
        bool stopSmallEnemySpawning = false;
        bool stopBigEnemySpawning = false;
        bool stopBossEnemySpawning = false;
        int bossSpawnCountdown = 0;
        int count = 0;

        bool isPause = false;
        bool isGameOver = false;
        bool isGameWin = false;

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
            wave++;
            gameTimer.Interval = 1000 / fps;
            gameTimer.Start();

            loadPlayerStatus();

            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(Resource.FONT);
            fontFamily = pfc.Families[0];
            items.Add(new Item("Shield", 4, 0, 0));
            items.Add(new Item("Weapon", 0, 100, 0));
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
            if (!this.isGameOver && !this.isGameWin)
            {
                this.update();
                this.paint();
                frameCount++;
            }
            else
            {
                if (this.isGameOver)
                {
                    this.paintOV();
                }
                else
                {
                    this.paintWG();
                }
            }
        }

        private async Task update()
        {
            double secondsElapsed = (DateTime.Now - startTime).TotalSeconds;
            bg.update(this.plMain.Width, this.plMain.Height);
            //wave1
            if (wave == 1)
            {
                if (secondsElapsed > 1 && !wave1Shown)
                {
                    ShowWave(wave);
                    wave1Shown = true;
                }
                if (secondsElapsed > 3 && enemies.Count == 0)
                {
                    spawnSmallEnemyWave();
                }

                if (stopSmallEnemySpawning && enemies.Count == 0)
                {
                    wave++;
                    CompleteWave=DateTime.Now;
                    wave1Shown = false;
                    if (secondsElapsed > 1 && !wave1Shown)
                    {
                        ShowWave(wave);
                        wave1Shown = true;
                    }
                    stopSmallEnemySpawning = false;
                }
            }
            //wave 2
            if (wave == 2)
            {
               if(enemies.Count == 0 && (DateTime.Now-CompleteWave).TotalSeconds > 2)
                {
                    spawnBigEnemy();
                    spawnSmallEnemyWave();
                }
                if (stopSmallEnemySpawning && enemies.Count == 0 && stopBigEnemySpawning)
                {
                    wave++;
                    CompleteWave = DateTime.Now;
                    wave1Shown = false;
                    if (secondsElapsed > 1 && !wave1Shown)
                    {
                        ShowWave(wave);
                        wave1Shown = true;
                    }
                    stopSmallEnemySpawning = false;
                    stopBigEnemySpawning = false;
                }
            }

            //wave 3
            if (wave == 3)
            {
                if (enemies.Count == 0 && (DateTime.Now - CompleteWave).TotalSeconds > 2)
                {
                    spawnBigEnemy();
                    spawnSmallEnemyWave();
                }
                if (stopSmallEnemySpawning && enemies.Count == 0 && stopBigEnemySpawning)
                {
                    wave++;
                    CompleteWave = DateTime.Now;
                    wave1Shown = false;
                    if (secondsElapsed > 1 && !wave1Shown)
                    {
                        ShowWave(wave);
                        wave1Shown = true;
                    }
                    stopSmallEnemySpawning = false;
                    stopBigEnemySpawning = false;
                }
            }

            //boss
            if (wave == 4)
            {
                if (enemies.Count == 0 && (DateTime.Now - CompleteWave).TotalSeconds > 2)
                {
                    spawnBossEnemy();
                    stopBossEnemySpawning = true;
                }
                if (enemies.Count == 0 && stopBossEnemySpawning)
                {
                    isGameWin = true;
                }

            }

            for (int i = items.Count - 1; i >= 0; i--)
            {
                Item item = items[i];
                item.update();
                if (item.isCollide(player))
                {
                    if (item.type < 4)
                    {
                        player.changeWeapon(item.type);
                    }
                    else if (item.type == 4)
                    {
                        player.getShield();
                    }
                    item.lifetime = 0;
                }
                if (item.lifetime <= 0)
                {
                    items.Remove(item);
                }
            }

            foreach (var enemy in enemies)
                enemy.Update();

            enemies.RemoveAll(e => e.isDead && !e.isExploding);

            allEnemyBullets.Clear();
            foreach (var enemy in enemies)
                allEnemyBullets.AddRange(enemy.bullets);

            foreach (var b in allEnemyBullets)
            {
                if (player.isCollide(b))
                {
                    player.getHit(b);
                }
            }
            allEnemyBullets.RemoveAll(b => b.y > 1000);

            // Kiểm tra va chạm giữa đạn của người chơi và các enemy
            foreach (var enemy in enemies)
            {
                foreach (var bullet in player.engine.fireDarts)
                {
                    if (bullet.isCollide(enemy))
                    {
                        // Giảm máu của enemy và hủy đạn
                        enemy.TakeDamage(bullet.damage);
                        player.engine.fireDarts.Remove(bullet);

                    }
                }
            }

           

            player.update();

            if (Fighter.status == 4)
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
            foreach (Item item in items)
            {
                item.paint(ref g);
            }

            if (showWaveText)
            {
                TimeSpan elapsed = DateTime.Now - waveTextStartTime;
                if (elapsed.TotalMilliseconds <= waveTextDuration)
                {
                    using (Font font = new Font(Form1.fontFamily, 36, FontStyle.Bold))
                    using (Brush brush = new SolidBrush(Color.White))
                    using (StringFormat format = new StringFormat()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    })
                    {
                        RectangleF rect = new RectangleF(0, 0, plMain.Width, plMain.Height);
                        g.DrawString(waveTextContent, font, brush, rect, format);
                    }
                }
                else
                {
                    showWaveText = false;
                }
            }



            this.plMain.CreateGraphics().DrawImage(screen, 0, 0);
        }

        public void paintOV()
        {
            GameText.show(ref g, "Game Over", 40, this.plMain.Height / 2 - 50, 40);
            this.plMain.CreateGraphics().DrawImage(screen, 0, 0);
        }

        public void paintWG()
        {
            GameText.show(ref g, "You Win", 40, this.plMain.Height / 2 - 50, 40);
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
            if (stopSmallEnemySpawning) return;
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
            
            if (wave == 1)
            {
                if(smallEnemyWaveCount <3)
                {
                    smallEnemyWaveCount++;
                }
                else
                {
                    stopSmallEnemySpawning = true;
                    smallEnemyWaveCount = 0;
                }
            }

            if (wave == 2)
            {
                if(smallEnemyWaveCount <5)
                {
                    smallEnemyWaveCount++;
                }
                else
                {
                    stopSmallEnemySpawning = true;
                    smallEnemyWaveCount = 0;
                }
            }

            if (wave == 3)
            {
                if (smallEnemyWaveCount < 5)
                {
                    smallEnemyWaveCount++;
                }
                else
                {
                    stopSmallEnemySpawning = true;
                    smallEnemyWaveCount = 0;
                }
            }

        }

        private void spawnBigEnemy()
        {
            if (stopBigEnemySpawning) return;

            int side = rand.Next(0, 2);
            int y = rand.Next(30, 80);
            Enemy big = new BigEnemy(side, y);
            enemies.Add(big);

            if (wave == 1)
            {
                stopBigEnemySpawning = true;
            }

            if (wave == 2)
            {
 
                if(bigEnemyCount < 1)
                {
                    
                    bigEnemyCount++;
                }
                else
                {
                    stopBigEnemySpawning = true;
                    bigEnemyCount = 0;
                    
                }
            }

            if (wave == 3)
            {
                if (bigEnemyCount < 3)
                {
                    bigEnemyCount++;
                }
                else
                {
                    stopBigEnemySpawning =true;
                    bigEnemyCount = 0;
                }
            }
            
        }

  

        private void spawnBossEnemy()
        {
            if(stopBossEnemySpawning) return;

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

        public void ShowWave(int number)
        {
            if (number <= 3)
            {
                waveTextContent = $"WAVE {number}/4";
            }
            else
            {
                waveTextContent = "FINAL BOSS";
            }
                waveTextStartTime = DateTime.Now;
            showWaveText = true;
        }


        private void plMain_MouseMove(object sender, MouseEventArgs e)
        {
            player.moveTo(e.X - player.getW() / 2, e.Y - player.getH() / 2);
        }
    }
}
