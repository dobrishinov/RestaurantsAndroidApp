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
using System.ComponentModel;
using DataAccess.RestaurantWebService;

namespace RestaurantsXamarin
{
    [Activity(Label = "Add Restraurant", Icon = "@drawable/icon")]
    public class AddRestraurantActivity : Activity
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://10.195.20.45:49382/Services/RestaurantWebService.svc");

        private RestaurantWebServiceClient client;

        private EditText restaurantName;
        private EditText restaurantType;
        private EditText restaurantDescription;
        private EditText restaurantWorkingTime;
        private EditText restaurantEmail;
        private EditText restaurantAddress;
        private EditText restaurantPhone;
        private EditText restaurantImageUrl;

        private Button addRestaurant;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddRestaurant);

            InitializeUserServiceClient();

            restaurantName = FindViewById<EditText>(Resource.Id.restaurantName);
            restaurantType = FindViewById<EditText>(Resource.Id.restaurantType);
            restaurantDescription = FindViewById<EditText>(Resource.Id.restaurantDescription);
            restaurantWorkingTime = FindViewById<EditText>(Resource.Id.restaurantWorkingTime);
            restaurantEmail = FindViewById<EditText>(Resource.Id.restaurantEmail);
            restaurantAddress = FindViewById<EditText>(Resource.Id.restaurantAddress);
            restaurantPhone = FindViewById<EditText>(Resource.Id.restaurantPhone);
            restaurantImageUrl = FindViewById<EditText>(Resource.Id.restaurantImageUrl);

            addRestaurant = FindViewById<Button>(Resource.Id.addRestaurant);

            addRestaurant.Click += RegisterButtonOnClick;
        }

        private void InitializeUserServiceClient()
        {
            BasicHttpBinding binding = CreateBasicHttp();

            client = new RestaurantWebServiceClient(binding, EndPoint);
            client.SaveCompleted += AddRestaurantCompleted;
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
            RestaurantEntity user = new RestaurantEntity
            {
                Id = 0,
                Name = restaurantName.Text,
                Type = restaurantType.Text,
                Description = restaurantDescription.Text,
                WorkingTime = restaurantWorkingTime.Text,
                Email = restaurantEmail.Text,
                Address = restaurantAddress.Text,
                Phone = restaurantPhone.Text,
                ImageUrl = restaurantImageUrl.Text,
                CreateTime = DateTime.Now,
                RestaurantsStatus = false
            };

            client.SaveAsync(user);
            Toast.MakeText(this, "Restaurant successfully add!", ToastLength.Short).Show();
        }

        private void AddRestaurantCompleted(object sender, AsyncCompletedEventArgs e)
        {
            StartActivity(typeof(MainActivity));
            Finish();
        }
    }
}