using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerkoopTruithesBL.Model {
    public class Club {
        public Club(string competitie, string ploegNaam) {
            Competitie = competitie.Trim();
            PloegNaam = ploegNaam.Trim();
        }

        public string Competitie { get; private set; }
        public string PloegNaam { get; private set; }

        public override string? ToString() {
            return $"Ploeg: {PloegNaam} Competitie: {Competitie}";
        }
    }
}
