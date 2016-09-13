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

namespace RestaurantsXamarin
{
    [Activity(Label = "Users")]
    public class UsersActivity : ListActivity
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://100.98.78.97:49382/Services/UserWebService.svc");

        private UserWebServiceClient client;

        private UserEntity[] usersModel;

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
            UserEntity user = usersModel[position];

            Intent edit = new Intent(this, typeof(EditActivity));
            edit.PutExtra("userID", user.Id);
            StartActivity(edit);
        }

        private void InitializeUserServiceClient()
        {
            BasicHttpBinding binding = CreateBasicHttp();
            client = new UserWebServiceClient(binding, EndPoint);
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
            usersModel = getAllCompletedEventArgs.Result;
            RunOnUiThread(() => ListAdapter = new UsersListAdapter(this, usersModel));
        }
    }
}