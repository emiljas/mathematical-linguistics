using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathematicalLinguistics
{
    public class Coin
    {
        public int Grosze { get; set; }

        private Coin(int grosze)
        {   
            Grosze = grosze;
        }

        public override string ToString()
        {
            return Grosze / 100 + "zł";
        }

        public static Coin FromGrosze(int grosze)
        {
            return new Coin(grosze);
        }

        public static Coin FromZlotys(int zlotys)
        {
            return new Coin(zlotys * 100);
        }
    }
}
