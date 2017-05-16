using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    class Weapon : item
    {
        public Weapon(int id, string name, string description,string icon) : base(id, name, description,icon) {
            
            base.ItemType = "Weapon";
            
        }
    }

