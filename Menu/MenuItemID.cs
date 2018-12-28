using System;

namespace OverlayGameMenu
{
    public class MenuItemID
    {
        public int ID { get; private set; }

        private bool _activated;
        private int choseID;

        public bool Activated
        {
            get
            {
                return _activated;
            }
            set
            {
                _activated = value;
                if (value == false)
                    ChoseID = 0;
            }
        }
        public int ChoseID
        {
            get
            {
                return choseID;
            }
            set
            {
                choseID = value;
                _activated = value != 0;
            }
        }

        public MenuItemID(int ID, bool Activated)
        {
            this.ID = ID;
            this.Activated = Activated;
            this.ChoseID = 0;
        }
    }
}
