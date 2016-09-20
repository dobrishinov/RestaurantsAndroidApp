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
using DataAccess.UserWebService;

namespace RestaurantsXamarin
{
    public class UsersListAdapter : BaseAdapter<UserEntity>
    {
        UserEntity[] users;
        Activity context;

        public UsersListAdapter(Activity context, UserEntity[] users) : base()
        {
            this.context = context;
            this.users = users;
        }

        public override UserEntity this[int position]
        {
            get { return users[position]; }
        }

        public override int Count
        {
            get { return users.Length; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
            }

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = users[position].Username;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = users[position].FirstName + " " + users[position].LastName;

            return view;
        }
    }
}

