using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder_Fighter.BSLayers
{
    class StatusBar
    {
        public Sprite healthBar = new Sprite();
        public Sprite staminaBar = new Sprite();

        public StatusBar()
        {
            this.healthBar.LoadGif(Resource.HEAHLTH);
            this.staminaBar.LoadGif(Resource.STAMINA);
        }

        public void update()
        {
            float percentH = (float)Fighter.health / Fighter.mHealth;
            if (percentH > 0.85)
            {
                this.healthBar.index = 0;
            }
            else if (percentH > 0.70)
            {
                this.healthBar.index = 1;
            }
            else if (percentH > 0.55)
            {
                this.healthBar.index = 2;
            }
            else if (percentH > 0.40)
            {
                this.healthBar.index = 3;
            }
            else if (percentH > 0.25)
            {
                this.healthBar.index = 4;
            }
            else if (percentH > 0.0)
            {
                this.healthBar.index = 5;
            }
            else
            {
                this.healthBar.index = 6;
            }

            float percentS = (float)Fighter.stamina / Fighter.mStamina;

            if (percentS > 0.85)
            {
                this.staminaBar.index = 0;
            }
            else if (percentS > 0.70)
            {
                this.staminaBar.index = 1;
            }
            else if (percentS > 0.55)
            {
                this.staminaBar.index = 2;
            }
            else if (percentS > 0.40)
            {
                this.staminaBar.index = 3;
            }
            else if (percentS > 0.25)
            {
                this.staminaBar.index = 4;
            }
            else if (percentS > 0.0)
            {
                this.staminaBar.index = 5;
            }
            else
            {
                this.staminaBar.index = 6;
            }

        }
        // 100, 85, 70, 55, 40, 25, 0
        public void paint(ref Graphics g)
        {
            this.healthBar.Draw(ref g, 10, 20, Fighter.w, Fighter.h/3);
            this.staminaBar.Draw(ref g, 10, 10 + Fighter.h / 3, Fighter.w, Fighter.h / 3);
        }
    }
}
