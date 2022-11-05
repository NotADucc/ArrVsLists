using System;
using System.Collections.Generic;
using VerkoopTruithesBL.Exceptions;

namespace VerkoopTruithesBL.Model
{
    public class Klant
    {
        public int KlantNr { get; private set; }
        public string Naam { get; private set; }
        public string Adres { get; private set; }
        private List<Bestelling> _bestellingen=new List<Bestelling>();
        public Klant(int klantNr, string naam, string adres) : this(naam, adres) {
            ZetKlantNr(klantNr);
        }

        public Klant(string naam, string adres) {
            ZetNaam(naam);
            ZetAdres(adres);
        }

        public void ZetKlantNr(int klantNr)
        {
            if (klantNr <= 0) throw new KlantException("Klantnr <=0");
            KlantNr = klantNr;
        }
        public void ZetNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new KlantException("naam invalid");
            Naam = naam.Trim();
        }
        public void ZetAdres(string adres)
        {
            if (adres.Length<5) throw new KlantException("adres invalid");
            Adres = adres.Trim();
        }
        public void VoegBestellingToe(Bestelling bestelling)
        {
            if (bestelling == null) throw new KlantException("bestelling is null");
            if (_bestellingen.Contains(bestelling)) throw new KlantException("bestelling bestaat reeds"); //TODO equals bestelling
            _bestellingen.Add(bestelling);
            if ((bestelling.Klant == null) || (bestelling.Klant!=this))
             bestelling.ZetKlant(this);

        }
        public void VerwijderBestelling(Bestelling bestelling)
        {
            if (bestelling == null) throw new KlantException("bestelling is null");
            if (!_bestellingen.Contains(bestelling)) throw new KlantException("bestelling bestaat reeds"); //TODO equals bestelling
            _bestellingen.Remove(bestelling);
            bestelling.VerwijderKlant();
        }
        public IReadOnlyList<Bestelling> GeefBestellingen()
        {
            return _bestellingen.AsReadOnly();
        }
        public int Korting() //in procent
        {
            if (_bestellingen.Count >= 10) return 20;
            if (_bestellingen.Count >= 5) return 10;
            return 0;
        }
        public bool IsDezelfde(Klant andereKlant)
        {
            if (andereKlant == null) throw new KlantException("IsDezelfde");
            if (!Naam.Equals(andereKlant.Naam)) return false;
            if (!Adres.Equals(andereKlant.Adres)) return false;
            if (KlantNr!=andereKlant.KlantNr) return false;
            return true;
        }

        public override bool Equals(object? obj)
        {
            return obj is Klant klant &&
                   KlantNr == klant.KlantNr;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(KlantNr);
        }

        public override string? ToString() {
            return $"{Naam} {Adres}";
        }
    }
}