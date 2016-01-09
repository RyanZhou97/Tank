using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My90Tank
{
    class Factory
    {
        public static Module getSomething(int x, int y, string thingType)
        {
            switch (thingType)
            {
                case "Grass":
                    return new Grass(x, y);
                case "Wall":
                    return new Wall(x, y);
                case "Water":
                    return new Water(x, y);
                case "Steel":
                    return new Steel(x, y);
            }

            return null;
        }
    }
}
