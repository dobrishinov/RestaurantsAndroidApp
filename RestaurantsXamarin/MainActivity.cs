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

namespace RestaurantsXamarin
{
    [Activity(Label = "RestaurantsXamarin", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://100.98.78.97:49382/Services/UserWebService.svc");

        private UserWebServiceClient client;

        private EditText username;
        private EditText password;
        private Button login;
        private Button register;
        private TextView result;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            InitializeUserServiceClient();

            username = FindViewById<EditText>(Resource.Id.username);
            password = FindViewById<EditText>(Resource.Id.password);
            login = FindViewById<Button>(Resource.Id.login);
            register = FindViewById<Button>(Resource.Id.register);
            result = FindViewById<TextView>(Resource.Id.result);

            login.Click += LoginButtonOnClick;
            register.Click += RegisterButtonOnClick;
        }

        private void InitializeUserServiceClient()
        {
            BasicHttpBinding binding = CreateBasicHttp();

            client = new UserWebServiceClient(binding, EndPoint);
            client.GetAllCompleted += LoginCompleted;
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

        private void LoginButtonOnClick(object sender, EventArgs eventArgs)
        {
            client.GetAllAsync();
        }

        private void RegisterButtonOnClick(object sender, EventArgs e)
        {
            UserEntity user = new UserEntity
            {
                Id = 0,
                Username = username.Text,
                Password = password.Text
            };
            client.SaveAsync(user);
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
                    StartActivity(typeof(UsersActivity));
                }

                else
                {
                    msg = "Fail!";
                }
            }
            RunOnUiThread(() => result.Text = msg);
        }

        private void RegisterCompleted(object sender, AsyncCompletedEventArgs e)
        {
            StartActivity(typeof(UsersActivity));
            //string msg = null;

            //if (getAllCompletedEventArgs.Error != null)
            //{
            //    msg = getAllCompletedEventArgs.Error.Message;
            //    msg += getAllCompletedEventArgs.Error.InnerException;
            //}

            //else if (getAllCompletedEventArgs.Cancelled)
            //{
            //    msg = "Request was cancelled.";
            //}
            //Users user = getAllCompletedEventArgs.Result.Where(u => u.Username == username.Text && u.Password == password.Text).FirstOrDefault();
            //if (user != null)
            //{
            //    StartActivity(typeof(UsersActivity));
            //}

            //else
            //{
            //    msg = "Fail!";
            //}
        }
    }

//    {
//        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://100.98.78.97:49381/UsersWebService.svc");

//        private UserWebService client;

//        private EditText username;
//        private EditText password;
//        private Button login;
//        private Button register;
//        private TextView result;

//        protected override void OnCreate(Bundle bundle)
//        {
//            base.OnCreate(bundle);

//            SetContentView(Resource.Layout.Main);

//            InitializeUserServiceClient();

//            username = FindViewById<EditText>(Resource.Id.username);
//            password = FindViewById<EditText>(Resource.Id.password);
//            login = FindViewById<Button>(Resource.Id.login);
//            register = FindViewById<Button>(Resource.Id.register);
//            result = FindViewById<TextView>(Resource.Id.result);

//            login.Click += LoginButtonOnClick;
//            register.Click += RegisterButtonOnClick;
//        }

//        private void InitializeUserServiceClient()
//        {
//            BasicHttpBinding binding = CreateBasicHttp();

//            client = new UserWebServiceClient(binding, EndPoint);
//            client.GetAllCompleted += LoginCompleted;
//            client.SaveCompleted += RegisterCompleted;
//        }

//        private static BasicHttpBinding CreateBasicHttp()
//        {
//            BasicHttpBinding binding = new BasicHttpBinding
//            {
//                Name = "basicHttpBinding",
//                MaxBufferSize = 2147483647,
//                MaxReceivedMessageSize = 2147483647
//            };
//            TimeSpan timeout = new TimeSpan(0, 0, 30);
//            binding.SendTimeout = timeout;
//            binding.OpenTimeout = timeout;
//            binding.ReceiveTimeout = timeout;
//            return binding;
//        }

//        private void LoginButtonOnClick(object sender, EventArgs eventArgs)
//        {
//            client.GetAllAsync();
//        }

//        private void RegisterButtonOnClick(object sender, EventArgs e)
//        {
//            UserEntity user = new UserEntity
//            {
//                Id = 0,
//                Username = username.Text,
//                Password = password.Text
//            };
//            client.SaveAsync(user);
//        }

//        private void LoginCompleted(object sender, GetAllCompletedEventArgs getAllCompletedEventArgs)
//        {
//            string msg = null;

//            if (getAllCompletedEventArgs.Error != null)
//            {
//                msg = getAllCompletedEventArgs.Error.Message;
//                msg += getAllCompletedEventArgs.Error.InnerException;
//            }

//            else if (getAllCompletedEventArgs.Cancelled)
//            {
//                msg = "Request was cancelled.";
//            }

//            else
//            {
//                UserEntity user = getAllCompletedEventArgs.Result.Where(u => u.Username == username.Text && u.Password == password.Text).FirstOrDefault();

//                if (user != null)
//                {
//                    StartActivity(typeof(UsersActivity));
//                }

//                else
//                {
//                    msg = "Fail!";
//                }
//            }
//            RunOnUiThread(() => result.Text = msg);
//        }

//        private void RegisterCompleted(object sender, AsyncCompletedEventArgs e)
//        {
//            StartActivity(typeof(UsersActivity));
//        }
//    }
}

