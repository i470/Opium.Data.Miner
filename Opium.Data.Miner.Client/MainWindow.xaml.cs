using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json.Linq;
using RestSharp;


namespace Opium.Data.Miner.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Crypto> CryptoList;
        public JToken CryptoJToken;

        public MainWindow()
        {
            InitializeComponent();
            CryptoList=new ObservableCollection<Crypto>();
            

            this.btnToJson.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new RestClient("https://www.cryptocompare.com/api/data/");
            var request = new RestRequest("coinlist/", Method.POST);
          

            // execute the request
            IRestResponse response = client.Execute(request);
            JObject jObect = JObject.Parse(response.Content);

            CryptoJToken = jObect.SelectToken("Data");

            ObservableCollection<Crypto> ls  = new ObservableCollection<Crypto>();
            foreach (var c in CryptoJToken)
            {
                var x = c.Children().First().ToObject<Crypto>();
                var url = x.Url;

                x.Url = "https://cryptocompare.com" + url.ToString();

                if (!string.IsNullOrWhiteSpace(x.ImageUrl))
                {
                    var imageUrl = x.ImageUrl;
                    x.ImageUrl = "https://cryptocompare.com" + imageUrl.ToString();
                   
                }

                var rc = new RestClient("https://www.cryptocompare.com/api/data/");
               
                var rq = new RestRequest("coinsnapshotfullbyid/?id="+x.Id, Method.POST);
               
                //IRestResponse rs = client.Execute(rq);
                //JObject jo = JObject.Parse(rs.Content);
                //foreach (dynamic m in jo.SelectToken("Data"))
                //{
                //    var z = m;

                //}
                ls.Add(x);
            }

            CryptoList = ls;
            this.datagrid.ItemsSource = CryptoList;
            this.btnToJson.IsEnabled = true;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var filename = @"root-crypto.json";

                File.WriteAllText(path + filename,
                    CryptoJToken.ToString());

                MessageBox.Show("File name "+filename+" has been saved to your desktop", "OK");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ERROR");
            }
          
            
        }
    }

    public class Crypto
    {
      
        public string Id { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string CoinName { get; set; }
        public string FullName { get; set; }
        public string Algorithm { get; set; }
        public string ProofType { get; set; }
        public string FullyPremined { get; set; }
        public string TotalCoinSupply { get; set; }
        public string PreMinedValue { get; set; }
        public string TotalCoinsFreeFloat { get; set; }

        public string Description { get; set; }
        public string Features { get; set; }
        public string Technology { get; set; }
        
        public string DifficultyAdjustment { get; set; }
        public string BlockRewardReduction { get; set; }
        
        public string StartDate { get; set; }
        public string Twitter { get; set; }
        public string WebsiteUrl { get; set; }
        public string LastBlockExplorerUpdateTS { get; set; }
        public string BlockNumber { get; set; }
        public string BlockTime { get; set; }
        public string NetHashesPerSecond { get; set; }
        public string TotalCoinsMined { get; set; }
        public string BlockReward { get; set; }
        public string ICOStatus { get; set; }
        public string WhitePaper { get; set; }

    }

}
