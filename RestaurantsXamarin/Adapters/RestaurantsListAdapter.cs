using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DataAccess.RestaurantWebService;

namespace RestaurantsXamarin
{
    public class RestaurantsListAdapter : BaseAdapter<RestaurantEntity>
    {
        RestaurantEntity[] restaurant;
        Activity context;
        public static int restaurantsCount;
        
        public RestaurantsListAdapter(ListActivity context, RestaurantEntity[] restaurant) : base()
        {
            this.context = context;
            this.restaurant = restaurant;
        }
        
        public override RestaurantEntity this[int position]
        {
            get { return restaurant[position]; }
        }

        public override int Count
        {
            get { return restaurant.Length; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            restaurantsCount = restaurant.Length;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
            }

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = restaurant[position].Name;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = restaurant[position].Type;

            return view;
        }
    }
}

