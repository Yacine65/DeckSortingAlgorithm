using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace DeckSortingAlgorithm
{
    public partial class Form1 : Form
    {
        List<Card> cardList = new List<Card>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadCards();
        }
        private async void loadCards()
        {
            var url = string.Format("https://db.ygoprodeck.com/api/v7/cardinfo.php?cardset=battle%20of%20chaos");
            using (var httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> getResponse = httpClient.GetAsync(url);
                HttpResponseMessage response = await getResponse;
                var responseJsonString = await response.Content.ReadAsStringAsync();

                dynamic array = JsonConvert.DeserializeObject(responseJsonString);
                foreach (var s in array)
                {
                    foreach (var t in s)
                    {
                        foreach (var card in t)
                        {
                            string name = (String)card.name;
                            string type = (String)card.type;
                            string race = (String)card.race;
                            string level = "-1";
                            string atk = "-1";
                            try
                            {
                                level = (String)card.level;
                                atk = (String)card.atk;
                            }
                            catch (Exception){}
                            Card newCard = new Card(name,type,race, level, atk);
                            cardList.Add(newCard); 
                        }
                    }
                }
                
            }
        }
        /**
         * Correctly sorts a list of cards (main deck, extra deck or side deck).
         * When adding/updating records to the card database, give every 
         * "Spell Card" and "Trap Card" a "level" of -1 and an "atk" of -1 to facilitate sorting
         * Both this method and shuffle can be tested by adding the following code after the first line : 
         * foreach (Card card in cardList)
            {
                System.Diagnostics.Debug.WriteLine(card.Name + " & LEVEL = " + card.Level + " & TYPE = " + card.Type + " & RACE = " + card.Race);
            }
         */
        private void btnSort_Click(object sender, EventArgs e)
        {
            cardList = cardList.OrderByDescending(x => x.Level).ThenBy(x => x.Type).ThenByDescending(x => x.Atk).ThenBy(x => x.Race).ToList();
        }
        /**
         * Randomly shuffles a list of cards (main deck, extra deck or side deck).
         */
        private void btnShuffle_Click(object sender, EventArgs e)
        {
            cardList.Shuffle();
        }
    }
    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random Local;
        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }
    static class MyExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
