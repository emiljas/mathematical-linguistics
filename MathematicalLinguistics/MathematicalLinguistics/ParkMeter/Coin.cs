using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathematicalLinguistics.ParkMeter
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
            if (Grosze >= 100)
                return (Grosze / 100).ToString() + "zł";
            else
                return Grosze.ToString() + "gr";
        }

        public static Coin FromGrosze(int grosze)
        {
            return new Coin(grosze);
        }

        public static Coin FromZlotys(int zlotys)
        {
            return new Coin(zlotys * 100);
        }

        public static Coin Parse(string value)
        {
            if (value.EndsWith("zł"))
            {
                var v = int.Parse(value.Replace("zł", ""));
                return Coin.FromZlotys(v);
            }
            else if (value.EndsWith("gr"))
            {
                var v = int.Parse(value.Replace("gr", ""));
                return Coin.FromGrosze(v);
            }

            throw new NotImplementedException();
        }

        public override bool Equals(object o)
        {
            var c = o as Coin;
            if (c == null)
                throw new NotImplementedException();

            return this.Grosze == c.Grosze;
        }

        public Coin Clone()
        {
            return MemberwiseClone() as Coin;
        }
    }
}
