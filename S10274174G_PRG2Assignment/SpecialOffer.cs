//==========================================================
// Student Number : S10274174G
// Student Name : Brian Jose Nevill
// Partner Name : Ong Cong Siang
//==========================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10274174G_PRG2Assignment
{
    internal class SpecialOffer
    {
        private string offerCode;
        private string offerDesc;
        private double discount;

        public string OfferCode
        {
            get { return offerCode; }
            set { offerCode = value; }
        }
        public string OfferDesc
        {
            get { return offerDesc; }
            set { offerDesc = value; }
        }
        public double Discount
        {
            get { return discount; }
            set { discount = value; }
        }
        public SpecialOffer(string code, string desc, double discount)
        {
            OfferCode = code;
            OfferDesc = desc;
            Discount = discount;
        }
        public override string ToString()
        {
            return $"{OfferCode} - {OfferDesc} - Discount: {Discount}%";
        }
    }
}
