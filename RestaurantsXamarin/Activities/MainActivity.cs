using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.ServiceModel;
using System.ComponentModel;
using System.Linq;
using DataAccess.UserWebService;
using RestaurantsXamarin.Resources;

namespace RestaurantsXamarin
{
    [Activity(Label = "Restaurants", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button signin;
        private ImageButton eatinator;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            
            eatinator = FindViewById<ImageButton>(Resource.Id.eatinator);
            
            eatinator.Click += EatinatorButtonOnClick;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.add)
            {
                if (Auth.loggedUser == null)
                {
                    Toast.MakeText(this, "Login first!", ToastLength.Short).Show();
                    StartActivity(typeof(SignMenuActivity));
                    return true;
                }
                if (Auth.loggedUser != null)
                {
                    Toast.MakeText(this, "Add restaurant", ToastLength.Short).Show();
                    StartActivity(typeof(AddRestraurantActivity));
                    return true;
                }
            }
            else if (id == Resource.Id.account)
                {
                    if (Auth.loggedUser == null)
                    {
                        StartActivity(typeof(SignMenuActivity));
                        Toast.MakeText(this, "Login or Register", ToastLength.Short).Show();
                        return true;
                    }
                    if (Auth.loggedUser != null)
                    {
                        //StartActivity(typeof(RestaurantsActivity));
                        Toast.MakeText(this, "You are already logged in!", ToastLength.Short).Show();
                        return true;
                    }
                }
            else if (id == Resource.Id.restaurants)
            {
                if (Auth.loggedUser == null)
                {
                    StartActivity(typeof(SignMenuActivity));
                    Toast.MakeText(this, "Login first!", ToastLength.Short).Show();
                    return true;
                }
                if (Auth.loggedUser != null)
                {
                    StartActivity(typeof(RestaurantsListActivity));
                    Toast.MakeText(this, "All restaurants", ToastLength.Short).Show();
                    return true;
                }
            }
            else if (id == Resource.Id.exit)
            {
                Toast.MakeText(this, "Exit", ToastLength.Short).Show();
                System.Environment.Exit(0);
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        //private void SingInButtonOnClick(object sender, EventArgs eventArgs)
        //{
        //    if (Auth.loggedUser == null)
        //    {
        //        StartActivity(typeof(SignMenuActivity));
        //    }
            
        //    if (Auth.loggedUser != null)
        //    {
        //        StartActivity(typeof(RestaurantsActivity));
        //    }
        //}

        private void EatinatorButtonOnClick(object sender, EventArgs e)
        {
            //if (Auth.loggedUser == null)
            //{
            //    eatinator.Visibility = ViewStates.Gone;
            //}
            //if (Auth.loggedUser == null)
            //{
            //    Toast.MakeText(this, "Log in First!", ToastLength.Short).Show();
            //    StartActivity(typeof(SignMenuActivity));
            //}
            //if (Auth.loggedUser != null)
            //{
            //    StartActivity(typeof(RestaurantsActivity));
            //}

            StartActivity(typeof(RestaurantsActivity));

        }
    }
}
