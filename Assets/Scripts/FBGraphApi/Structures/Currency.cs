using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FBGraphApi.Structures
{
    /// <summary>
    /// Klasa zawierająca informacje o walucie
    /// </summary>
    [Serializable]
    public class Currency
    {
        public int currency_offset;

        /// <summary>
        /// Wartość jednej jednostki waluty w USD
        /// </summary>
        public string usd_exchange;

        /// <summary>
        /// Koszt 1 USD w tej walucie
        /// </summary>
        public string usd_exchange_inverse;

        /// <summary>
        /// Ustandaryzowana nazwa waluty
        /// </summary>
        public string user_currency;
    }
}
