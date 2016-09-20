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
using System.ServiceModel;

namespace RestaurantsXamarin
{
    [Activity(Label = "All restaurants", Icon = "@drawable/icon")]
    public class RestaurantsListActivity : ListActivity
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://10.195.20.45:49382/Services/RestaurantWebService.svc");

        private RestaurantWebServiceClient client;

        private RestaurantEntity[] restaurantsModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitializeUserServiceClient();

            PopulateListView();
        }

        protected override void OnResume()
        {
            base.OnResume();

            PopulateListView();
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            RestaurantEntity restaurant = restaurantsModel[position];

            Intent restaurantEdit = new Intent(this, typeof(RestaurantViewActivity));
            restaurantEdit.PutExtra("restaurantID", restaurant.Id);
            StartActivity(restaurantEdit);
        }

        private void InitializeUserServiceClient()
        {
            BasicHttpBinding binding = CreateBasicHttp();
            client = new RestaurantWebServiceClient(binding, EndPoint);
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

        protected void PopulateListView()
        {
            client.GetAllAsync();
            client.GetAllCompleted += ListViewPopulatedCompleted;
        }


        private void ListViewPopulatedCompleted(object sender, GetAllCompletedEventArgs getAllCompletedEventArgs)
        {
            restaurantsModel = getAllCompletedEventArgs.Result;
            RunOnUiThread(() => ListAdapter = new RestaurantsListAdapter(this, restaurantsModel));
        }
    }
}