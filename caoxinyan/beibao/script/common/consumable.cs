using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    class consumable:item
    {
        public int BackHp { get; private set; }
        public int BackSp { get; private set; }
        public consumable(int id, string name, string description,string icon, int backHp, int backSp) : base(id, name, description,icon) {
            this.BackHp = backHp;
            this.BackHp = backSp;
            base.ItemType = "consumable";
        }
    }
