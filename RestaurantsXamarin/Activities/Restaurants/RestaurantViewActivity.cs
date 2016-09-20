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
using Android.Graphics;
using System.Net;
using System.Threading;

namespace RestaurantsXamarin
{
    [Activity(Label = "Restaurants")]
    public class RestaurantViewActivity : Activity
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://10.195.20.45:49382/Services/RestaurantWebService.svc");
        private RestaurantWebServiceClient client;
        AutoResetEvent reseter = new AutoResetEvent(false);

        int restaurantID;
        RestaurantEntity restaurantModel = null;

        ImageView image;
        TextView name;
        TextView type;
        TextView description;
        TextView workingTime;
        TextView email;
        TextView address;
        TextView phone;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Restaurants);
            

            restaurantID = Intent.GetIntExtra("restaurantID", 0);

            InitializeRestaurantsServiceClient();

            image = FindViewById<ImageView>(Resource.Id.image);
            name = FindViewById<TextView>(Resource.Id.name);
            type = FindViewById<TextView>(Resource.Id.type);
            description = FindViewById<TextView>(Resource.Id.description);
            workingTime = FindViewById<TextView>(Resource.Id.workingTime);
            email = FindViewById<TextView>(Resource.Id.email);
            address = FindViewById<TextView>(Resource.Id.address);
            phone = FindViewById<TextView>(Resource.Id.phone);
            
        }

        private void InitializeRestaurantsServiceClient()
        {
            BasicHttpBinding binding = CreateBasicHttp();

            client = new RestaurantWebServiceClient(binding, EndPoint);

            client.GetByIdAsync(restaurantID);

            client.GetByIdCompleted += Client_GetByIdCompleted; ;
            reseter.WaitOne();
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

        private void Client_GetByIdCompleted(object sender, GetByIdCompletedEventArgs e)
        {
            restaurantModel = e.Result;

            reseter.Set();

            var imageBitmap = GetImageBitmapFromUrl(restaurantModel.ImageUrl);

            RunOnUiThread(() =>
            {
                image.SetImageBitmap(imageBitmap);
                name.Text = restaurantModel.Name;
                type.Text = restaurantModel.Type;
                description.Text = restaurantModel.Description;
                workingTime.Text = restaurantModel.WorkingTime;
                email.Text = restaurantModel.Email;
                address.Text = restaurantModel.Address;
                phone.Text = restaurantModel.Phone;
            });
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}