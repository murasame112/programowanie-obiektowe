using System;
using System.IO;
using System.Xml.Linq;


namespace lab_11_notatki
{

    public partial class MainWindow : MainWindow
    {

        class TableRates
        {
            [JsonPropertyName("table")]
            public string Table { get; set; }
            [JsonPropertyName("no")]
            public string Number { get; set; }
            [JsonPropertyName("tradingDate")]
            public DateTime TradingDate { get; set; }
            [JsonPropertyName("effectiveDate")]
            public DateTime EffectiveDate { get; set; }
            [JsonPropertyName("rates")]
            public List<JsonRate> Rates { get; set; }
        }

        record Rate(string Currency, string Code, decimal Ask, decimal Bid);
        record JsonRate
        {
            

            [JsonPropertyName("currency")]
            public string Currency { get; set; }
            [JsonPropertyName("code")]
            public string Code { get; set; }
            [JsonPropertyName("ask")]
            public decimal Ask { get; set; }
            [JsonPropertyName("bid")]
            public decimal Bid { get; set; }
        }

        Dictionary<string, Rate> Rates = new Dictionary<string, Rate>();


        private void DownloadDataJson()
        {
            WebClient client = new WebClient();
            client.Headers.Add("Accept", "application/json");
            string json = client.DownloadString("http://api.nbp.pl/api/exchangerates/tables/C");
            list<TableRates> tableRates = JsonSerializer.Deserialize<List<TableRates>>(json);
            if(tableRates.Count == 1)
            {
                tableRates[0].Add(new JsonRate() { Currency = "złoty", Code = "PLN", Ask = 1, Bid = 1 });
                foreach(JsonRate rate in tableRates[0].rates)
                {
                    Rate.Add(rate.Code, new Rate(rate.Currency, rate.Code, rate.Ask, rate.Bid));
                }
            }
        }

        private void DownloadData()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-En");
            WebClient client = new WebClient();

            string xml = client.DownloadString("http://api.nbp.pl/api/exchangerates/tables/C");
            XDocument doc = XDocument.Parse(xml);

        }
    }

    public MainWindow()
    {
        InitializeComponent();
        DownloadDataJson();
        UpdateGui();
    }
    
    private void UpdateGui()
    {
        InputCurrencyCode.Items.Clear();
        OutputCurrencyCode.Items.Clear();
        foreach (string code in Rates.Keys)
        {
            InputCurrencyCode.Items.Add(code);
            OutputCurrencyCode.Items.Add(code);
        }
        InputCurrencyCode.SelectedIndex = 0;
        OutputCurrencyCode.SelectedIndex = 1;
    }

    private void LoadFileButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Title = " Wczytaj plik z notowaniami";
        dialog.Filter = "Plik tekstowy (*.txt)|*.txt";
        dialog.DefaultExt = "*.txt";
        if (dialog.ShowDialog() == true)
        {
            if (File.Exists(dialog.FileName))
            {
                string[] lines = File.ReadAllLines(dialog.FileName);
                Rates.Clear();
                foreach (string line in lines)
                {
                    string[] tokens = line.Split(";");
                    Rate rate = new Rate()
                    {
                        Code = tokens[0],
                        Currency = tokens[1],
                        Ask = decimal.Parse(tokens[2]),
                        Bid = decimal.Parse(tokens[3])
                    };
                    Rates.Add(rate.Code, rate);

                }
            }
        }
    }

    private void LoadJsonFileButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Title = " Wczytaj plik z notowaniami";
        dialog.Filter = "Plik json (*.json)|*.json";
        dialog.DefaultExt = "*.json";
        if (dialog.ShowDialog() == true)
        {
            if (File.Exists(dialog.FileName))
            {
                string[] lines = File.ReadAllLines(dialog.FileName);
                lines = JsonSerializer.Deserialize(lines);
                Rates.Clear();
                foreach (string line in lines)
                {
                    string[] tokens = line.Split(";");
                    Rate rate = new Rate()
                    {
                        Code = tokens[0],
                        Currency = tokens[1],
                        Ask = decimal.Parse(tokens[2]),
                        Bid = decimal.Parse(tokens[3])
                    };
                    Rates.Add(rate.Code, rate);

                }
            }
        }
    }

    private void SaveFileButton_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog dialog = new SaveFileDialog();
        dialog.Filter = "Dokument w formacie json (*.json)|*.json";
        dialog.Title = "Zapisz notowania do pliku JSON";
        if(dialog.ShowDialog() == true)
        {
            File.WriteAllText(dialog.FileName, JsonSerializer.Serialize(Rates));
        }
    }

    private void CalcOutputValue(object sender, RoutedEventArgs e)
    {
        string inputCode = (string)InputCurrencyCode.SelectedItem;
        string outputCode = (string)OutputCurrencyCode.SelectedItem;
        string amountStr = InputValue.Text;
        if(decimal.TryParse(amountStr, out decimal amount))
        {
            Rate inputRate = Rates[inputCode];
            Rate outputRate = Rates[outputCode];
            decimal output = amount * inputRate.Ask
        }
    }

    public void NumberValidation(object sender, TextCompositionEventArgs e)
    {

        e.Handled = !decimal.TryParse(InputValue.Text + e.Text, out decimal values);
        if((InputValue.Text + e.Text).Equals(""){
            InputValue.Text = "0,00";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
