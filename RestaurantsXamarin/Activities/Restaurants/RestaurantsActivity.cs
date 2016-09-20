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
using DataAccess.RestaurantWebService;
using Android.Media;
using Android.Graphics;
using System.Net;
using System.Threading;

namespace RestaurantsXamarin
{
    [Activity(Label = "Where can i eat today?", Icon = "@drawable/icon")]
    public class RestaurantsActivity : Activity
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://10.195.20.45:49382/Services/RestaurantWebService.svc");

        private RestaurantWebServiceClient client;
        AutoResetEvent reseter = new AutoResetEvent(false);

        int restaurantID;
        RestaurantEntity restaurantModel = null;
        List<RestaurantEntity> restaurantsModel = new List<RestaurantEntity>();

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

            //RestaurantsListAdapter restaurantAdapter = new RestaurantsListAdapter(this,);
            //var a = restaurantAdapter.Count;

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
            
            this.GenerateView();
        }

        private int Counter { get; set; }

        public int GetCount()
        {
            reseter.WaitOne();
            client.CountAsync();
            reseter.WaitOne();

            return Counter;
        }

        private void InitializeRestaurantsServiceClient()
        {
            BasicHttpBinding binding = CreateBasicHttp();

            client = new RestaurantWebServiceClient(binding, EndPoint);

            client.CountCompleted += Client_CountCompleted;

            //client.GetRestaurantByIdCompleted += GetRestaurantByIDCompleted;

            client.GetAllCompleted += Client_GetAllRestaurantsCompleted;

            client.GetAllAsync();
        }

        public RestaurantEntity GetRandromRestaurant()
        {
            int number = GetCount();

            Random rnd = new Random();
            int randomNum = rnd.Next(0, number);

            var restaurants = GetAllRestaurants();
            var restaurant = restaurants.ElementAt(randomNum);

            return restaurant;
        }

        public void GenerateView()
        {
            var restaurant = GetRandromRestaurant();

            var imageBitmap = GetImageBitmapFromUrl(restaurant.ImageUrl);

            RunOnUiThread(() =>
            {
                image.SetImageBitmap(imageBitmap);
                name.Text = restaurant.Name;
                type.Text = restaurant.Type;
                description.Text = restaurant.Description;
                workingTime.Text = restaurant.WorkingTime;
                email.Text = restaurant.Email;
                address.Text = restaurant.Address;
                phone.Text = restaurant.Phone;
            });

        }

        public List<RestaurantEntity> GetAllRestaurants()
        {
            //Thread.Sleep(2000);
            client.GetAllAsync();
            //Thread.Sleep(2000);
            reseter.WaitOne();

            return restaurantsModel;
        }

        private void Client_GetAllRestaurantsCompleted(object sender, GetAllCompletedEventArgs e)
        {
            //Thread.Sleep(2000);

            restaurantsModel = e.Result.ToList();
            reseter.Set();
        }

        private void Client_CountCompleted(object sender, CountCompletedEventArgs e)
        {
            Counter = e.Result;
            reseter.Set();
        }

        //private void ListAllRestaurants(object sender, GetAllRestaurantsCompletedEventArgs getAllCompletedEventArgs)
        //{
        //    Random rnd = new Random();
        //    restaurantsModel = getAllCompletedEventArgs.Result.ToList();
        //    int count = restaurantsModel.Count;

        //    client.GetByIdAsync(restaurantsModel[rnd.Next(0, count)].Id);
        //    client.GetRestaurantByIdCompleted += GetRestaurantByIDCompleted;
        //}

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
            Finish();
            StartActivity(typeof(RestaurantsActivity));

        }

        //private void GetRestaurantByIDCompleted(object sender, GetRestaurantByIdCompletedEventArgs getByIDCompletedEventArgs)
        //{
        //    restaurantModel = getByIDCompletedEventArgs.Result;

        //    var imageBitmap = GetImageBitmapFromUrl(restaurantModel.ImageUrl);

        //    RunOnUiThread(() =>
        //    {
        //        image.SetImageBitmap(imageBitmap);
        //        name.Text = restaurantModel.Name;
        //        type.Text = restaurantModel.Type;
        //        description.Text = restaurantModel.Description;
        //        workingTime.Text = restaurantModel.WorkingTime;
        //        email.Text = restaurantModel.Email;
        //        address.Text = restaurantModel.Address;
        //        phone.Text = restaurantModel.Phone;
        //    });
        //}

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