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

namespace RestaurantsXamarin
{
    [Activity(Label = "Edit User")]
    public class EditActivity : Activity
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://100.98.78.97:49382/Services/UserWebService.svc");
        private UserWebServiceClient client;

        int userID;
        UserEntity userModel = null;

        EditText username;
        EditText password;
        EditText firstName;
        EditText lastName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.UserEdit);

            userID = Intent.GetIntExtra("userID", 0);

            InitializeUserServiceClient();

            username = FindViewById<EditText>(Resource.Id.usernameEdit);
            password = FindViewById<EditText>(Resource.Id.passwordEdit);
            firstName = FindViewById<EditText>(Resource.Id.firstNameEdit);
            lastName = FindViewById<EditText>(Resource.Id.lastNameEdit);
            Button save = FindViewById<Button>(Resource.Id.save);
            Button delete = FindViewById<Button>(Resource.Id.delete);
            Button cancel = FindViewById<Button>(Resource.Id.cancel);
            TextView errors = FindViewById<TextView>(Resource.Id.errorsEdit);

            save.Click += SaveButtonOnClick;

            delete.Click += DeleteButtonOnClick;

            cancel.Click += delegate { Finish(); };
        }

        private void InitializeUserServiceClient()
        {
            BasicHttpBinding binding = CreateBasicHttp();

            client = new UserWebServiceClient(binding, EndPoint);

            client.GetByIdAsync(userID);
            client.GetByIdCompleted += GetUserByIDCompleted;

            client.DeleteCompleted += DeleteUserCompleted;
            client.SaveCompleted += SaveUserCompleted;
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

        private void GetUserByIDCompleted(object sender, GetByIdCompletedEventArgs getByIDCompletedEventArgs)
        {
            userModel = getByIDCompletedEventArgs.Result;

            RunOnUiThread(() =>
            {
                username.Text = userModel.Username;
                password.Text = userModel.Password;
                firstName.Text = userModel.FirstName;
                lastName.Text = userModel.LastName;
            });
        }

        private void SaveButtonOnClick(object sender, EventArgs e)
        {
            userModel.Username = username.Text;
            userModel.Password = password.Text;
            userModel.FirstName = firstName.Text;
            userModel.LastName = lastName.Text;

            client.SaveAsync(userModel);
        }

        private void SaveUserCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Finish();
        }

        private void DeleteButtonOnClick(object sender, EventArgs e)
        {
            client.DeleteAsync(userModel);
        }

        private void DeleteUserCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Finish();
        }
    }
}