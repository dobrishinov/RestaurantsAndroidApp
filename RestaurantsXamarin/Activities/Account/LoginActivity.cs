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

namespace RestaurantsXamarin.Resources
{
    [Activity(Label = "Login", Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://10.195.20.45:49382/Services/UserWebService.svc");

        private UserWebServiceClient client;

        private EditText username;
        private EditText password;
        private Button login;
        private TextView result;

    protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            InitializeUserServiceClient();

            username = FindViewById<EditText>(Resource.Id.username);
            password = FindViewById<EditText>(Resource.Id.password);
            login = FindViewById<Button>(Resource.Id.login);
            result = FindViewById<TextView>(Resource.Id.result);

            login.Click += LoginButtonOnClick;
        }

        private void InitializeUserServiceClient()
        {
            BasicHttpBinding binding = CreateBasicHttp();

            client = new UserWebServiceClient(binding, EndPoint);
            client.GetAllCompleted += LoginCompleted;
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

        private void LoginButtonOnClick(object sender, EventArgs eventArgs)
        {
            client.GetAllAsync();
            Toast.MakeText(this, "Successfully Logged!", ToastLength.Short).Show();
            Finish();
        }

        private void LoginCompleted(object sender, GetAllCompletedEventArgs getAllCompletedEventArgs)
        {
            string msg = null;
            
            if (getAllCompletedEventArgs.Error != null)
            {
                msg = getAllCompletedEventArgs.Error.Message;
                msg += getAllCompletedEventArgs.Error.InnerException;
            }

            else if (getAllCompletedEventArgs.Cancelled)
            {
                msg = "Request was cancelled.";
            }

            else
            {
                UserEntity user = getAllCompletedEventArgs.Result.Where(u => u.Username == username.Text && u.Password == password.Text).FirstOrDefault();

                if (user != null)
                {
                    Auth.loggedUser = user;
                }

                else
                {
                    msg = "Error";
                }
            }
            RunOnUiThread(() => result.Text = msg);
        }
    }
}