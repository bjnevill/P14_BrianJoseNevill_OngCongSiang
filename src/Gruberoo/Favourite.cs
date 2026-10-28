using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gruberoo
{
    internal class Favourite
    {
        private string favouriteName;
        private string restuarantId;
        private List<OrderedFoodItem> items;
        private string specialRequest;

        public string FavouriteName
        {
            get { return favouriteName; }
            set { favouriteName = value; }
        }
        public string RestuarantId
        {
            get { return restuarantId; }
            set { restuarantId = value; }
        }
        public List<OrderedFoodItem> Items
        {
            get { return items; }
            set { items = value; }
        }
        public string SpecialRequest
        {
            get { return specialRequest; }
            set { specialRequest = value; }
        }

        public Favourite(string favouriteName, string restaurantId, string specialRequest)
        {
            FavouriteName = favouriteName;
            RestuarantId = restaurantId;
            Items = new List<OrderedFoodItem>();
            SpecialRequest = specialRequest;
        }
    }
}
