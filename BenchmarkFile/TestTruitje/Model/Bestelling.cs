using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Exceptions;

namespace VerkoopTruithesBL.Model
{
    public class Bestelling
    {
        private Dictionary<Truitje, int> _truitjes = new Dictionary<Truitje, int>(); //int = aantal truitjes

        public Bestelling(DateTime tijdstip)
        {
            Tijdstip = tijdstip;
        }

        public Bestelling(int bestellingNr, DateTime tijdstip, double prijs, Klant klant, bool betaald):this(tijdstip,prijs,klant,betaald) {
            ZetBestellingNr(bestellingNr);
        }
        public Bestelling(DateTime tijdstip, double prijs, Klant klant, bool betaald) {
            Tijdstip = tijdstip;
            ZetPrijs(prijs);
            ZetKlant(klant);
            Betaald = betaald;
        }
        public int BestellingNr { get; private set; }
        public DateTime Tijdstip { get; set; }
        public double Prijs { get; private set; }
        public Klant Klant { get; private set; }
        public bool Betaald { get; private set; }
        private bool RecentBetaald { get; set; }
        public void ZetBestellingNr(int bestellingNr)
        {
            if (bestellingNr <= 0) throw new BestellingException("bestellingnr <=0");
            BestellingNr = bestellingNr;
        }
        public void ZetPrijs(double prijs)
        {
            if (prijs < 0) throw new BestellingException("prijs < 0");
            Prijs = prijs;
        }
        public void ZetKlant(Klant nieuweKlant)
        {
            if (nieuweKlant.Equals(Klant)) throw new BestellingException("klant is null");
            if (nieuweKlant == null) throw new BestellingException("klant is null");
            if (Klant != null) Klant.VerwijderBestelling(this);
            Klant = nieuweKlant;
            if (!nieuweKlant.GeefBestellingen().Contains(this))
                nieuweKlant.VoegBestellingToe(this);
        }
        public void VerwijderKlant()
        {
            if (Klant != null) 
                if (Klant.GeefBestellingen().Contains(this))  Klant.VerwijderBestelling(this);
            Klant = null;
        }
        public void ZetBetaald()
        {
            Betaald = true;
            RecentBetaald = true;
            ZetPrijs(BerekenPrijs());
        }
        public double BerekenPrijs()
        {
            double prijs=0.0;
            foreach(KeyValuePair<Truitje,int> keyValuePair in _truitjes)
            {
                prijs += keyValuePair.Value * keyValuePair.Key.Prijs;
            }
            if (Klant == null)
            {
                return prijs;
            }
            return prijs*(1-(Klant.Korting()/100.0));
            
        }
        public void ZetOnbetaald()
        {
            Betaald = false;
            RecentBetaald = false;
            Prijs = 0.0;
        }
        public void VoegTruitjesToe(Truitje truitje,int aantal)
        {
            if (RecentBetaald) throw new BestellingException("VoegTruitjesToe - reeds betaald");
            if (truitje == null) throw new BestellingException("VoegTruitjesToe");
            if (aantal<=0) throw new BestellingException("VoegTruitjesToe");
            if (_truitjes.ContainsKey(truitje))
            {
                _truitjes[truitje] += aantal;
            }
            else
            {
                _truitjes.Add(truitje, aantal);
            }
        }
        public void VerwijderTruitjes(Truitje truitje,int aantal)
        {
            if (Betaald) throw new BestellingException("VoegTruitjesToe - reeds betaald");
            if (truitje == null) throw new BestellingException("VoegTruitjesToe");
            if (aantal <= 0) throw new BestellingException("VoegTruitjesToe");
            if (!_truitjes.ContainsKey(truitje))
            {
                throw new BestellingException("VoegTruitjesToe");
            }
            if (_truitjes[truitje]<aantal) throw new BestellingException("VoegTruitjesToe");
            if (_truitjes[truitje]==aantal) _truitjes.Remove(truitje);
            else _truitjes[truitje] -= aantal;
        }
        public IReadOnlyDictionary<Truitje,int> GetTruitjes()
        {
            return _truitjes;
        }
        public bool IsDezelfde(Bestelling bestelling)
        {
            //TODO implement IsDezelfde
            return false;
        }

        public override bool Equals(object? obj)
        {
            return obj is Bestelling bestelling &&
                   this.BestellingNr == bestelling.BestellingNr;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BestellingNr);
        }
    }
}
