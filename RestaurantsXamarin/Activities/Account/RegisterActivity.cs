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
using System.ServiceModel;
using DataAccess.UserWebService;
using System.ComponentModel;

namespace RestaurantsXamarin.Resources
{
    [Activity(Label = "Register", Icon = "@drawable/icon")]
    public class RegisterActivity : Activity
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://10.195.20.45:49382/Services/UserWebService.svc");

        private UserWebServiceClient client;

        private EditText username;
        private EditText password;
        private EditText firstName;
        private EditText lastName;
        private EditText email;
        private EditText address;
        private EditText phone;
        private EditText description;
        private EditText favouriteFoods;
        private EditText favouritePlace;
        private EditText profileImageUrl;

        private Button register;
        private TextView result;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);

            InitializeUserServiceClient();

            username = FindViewById<EditText>(Resource.Id.username);
            password = FindViewById<EditText>(Resource.Id.password);
            firstName = FindViewById<EditText>(Resource.Id.firstName);
            lastName = FindViewById<EditText>(Resource.Id.lastName);
            email = FindViewById<EditText>(Resource.Id.email);
            address = FindViewById<EditText>(Resource.Id.address);
            phone = FindViewById<EditText>(Resource.Id.phone);
            description = FindViewById<EditText>(Resource.Id.description);
            favouriteFoods = FindViewById<EditText>(Resource.Id.favouriteFoods);
            favouritePlace = FindViewById<EditText>(Resource.Id.favouritePlace);
            profileImageUrl = FindViewById<EditText>(Resource.Id.profileImageUrl);

            register = FindViewById<Button>(Resource.Id.register);
            result = FindViewById<TextView>(Resource.Id.result);

            register.Click += RegisterButtonOnClick;
        }
        
        private void InitializeUserServiceClient()
        {
            BasicHttpBinding binding = CreateBasicHttp();

            client = new UserWebServiceClient(binding, EndPoint);
            client.SaveCompleted += RegisterCompleted;
        }

        private static BasicHttpBinding CreateBasicHttp()
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                Name = "basicHttpBinding",
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647
            };
            TimeSpan timeout = new TimeSpan(0, 0, 30);
            binding.SendTimeout = timeout;
            binding.OpenTimeout = timeout;
            binding.ReceiveTimeout = timeout;
            return binding;
        }

        private void RegisterButtonOnClick(object sender, EventArgs e)
        {
                UserEntity user = new UserEntity
                {
                    Id = 0,
                    Username = username.Text,
                    Password = password.Text,
                    FirstName = firstName.Text,
                    LastName = lastName.Text,
                    Email = email.Text,
                    Address = address.Text,
                    Phone = phone.Text,
                    Description = description.Text,
                    FavouriteFoods = favouriteFoods.Text,
                    FavouritePlace = favouritePlace.Text,
                    ProfileImageUrl = profileImageUrl.Text
                };

                client.SaveAsync(user);
                Toast.MakeText(this, "Successfully register", ToastLength.Short).Show();
        }
        
        private void RegisterCompleted(object sender, AsyncCompletedEventArgs e)
        {
            StartActivity(typeof(LoginActivity));
            Finish();
        }
    }
}