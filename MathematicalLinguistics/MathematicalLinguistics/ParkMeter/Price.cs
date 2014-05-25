using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalLinguistics.ParkMeter
{
    public class Price
    {
        public int Grosze { get; set; }

        private Price(int grosze)
        {
            Grosze = grosze;
        }

        public static Price FromGrosze(int grosze)
        {
            return new Price(grosze);
        }

        public static Price FromZlotys(int zlotys)
        {
            return new Price(zlotys * 100);
        }

        public static Price FromCoins(List<Coin> coins)
        {
            int sum = 0;
            coins.ForEach(c => sum += c.Grosze);

            return Price.FromGrosze(sum);
        }

        public static Price operator-(Price first, Price second)
        {
            return Price.FromGrosze(first.Grosze - second.Grosze);
        }

        public override string ToString()
        {
            if (Grosze < 100)
                return Grosze.ToString() + "gr";
            else
                return (Grosze / 100).ToString() + "zł";
        }
    }
}
