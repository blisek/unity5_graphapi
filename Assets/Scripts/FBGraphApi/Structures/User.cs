using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FBGraphApi.Structures
{
    /// <summary>
    /// Klasa przechowująca informacje o użytkowniku.
    /// </summary>
    [Serializable]
    public class User
    {
        public string id;

        /// <summary>
        /// Imię i nazwisko.
        /// </summary>
        public string name;

        /// <summary>
        /// Obecne miejsce zamieszkania.
        /// </summary>
        public LocationPage location;

        /// <summary>
        /// Miejsce urodzenia (?).
        /// </summary>
        public LocationPage hometown;

        /// <summary>
        /// Używana przez użytkownika waluta.
        /// </summary>
        public Currency currency;

        /// <summary>
        /// Jeśli pola nie są puste - wystąpił błąd.
        /// </summary>
        public Error error;
    }
}
