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

namespace RestaurantsXamarin.Resources
{
    [Activity(Label = "Account", Icon = "@drawable/icon")]
    public class SignMenuActivity : Activity
    {
        private Button login;
        private Button register;
        private TextView result;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string msg = null;

            SetContentView(Resource.Layout.SignMenu);

            login = FindViewById<Button>(Resource.Id.login);
            register = FindViewById<Button>(Resource.Id.register);
            result = FindViewById<TextView>(Resource.Id.result);

            if (Auth.loggedUser != null)
            {
                login.Visibility = ViewStates.Gone;
                register.Visibility = ViewStates.Gone;
                msg = "You already logged!";
            }

            RunOnUiThread(() => result.Text = msg);

            login.Click += LoginButtonOnClick;
            register.Click += RegisterButtonOnClick;
        }

        private void LoginButtonOnClick(object sender, EventArgs eventArgs)
        {
            StartActivity(typeof(LoginActivity));
            Finish();
        }

        private void RegisterButtonOnClick(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
            Finish();
        }
    }
}