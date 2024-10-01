using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Open_data
{
    public partial class Form1 : Form
    {
        private List<giocatore> listGiocatori;
        private string fileName = "Elenco.csv"; // Il path del file caricato
        private bool isNazionalitaAscending = true; // Variabile per controllare l'ordinamento della nazionalità
        private bool isPosizioneAscending = true; // Variabile per controllare l'ordinamento della posizione
        private bool isSquadraAscending = true; // Variabile per controllare l'ordinamento della squadra
        private bool isCampionatoAscending = true; // Variabile per controllare l'ordinamento del campionato

        public Form1()
        {
            InitializeComponent();
            listView1.ColumnClick += listView1_ColumnClick; // Associa l'evento
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listGiocatori = new List<giocatore>();
            CaricaGiocatoriDaCSV();
            CaricaGiocatorinellalistview();
        }

        private void CaricaGiocatoriDaCSV()
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    bool isFirstLine = true;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        string[] fields = line.Split(';');
                        giocatore g = new giocatore(
                            int.Parse(fields[0]),
                            fields[1],
                            fields[2],
                            fields[3],
                            fields[4],
                            fields[5],
                            int.Parse(fields[6]),
                            int.Parse(fields[7]),
                            int.Parse(fields[8]),
                            int.Parse(fields[9]),
                            int.Parse(fields[10]),
                            double.Parse(fields[11]),
                            int.Parse(fields[12])
                        );

                        listGiocatori.Add(g);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore nella lettura del file CSV: " + ex.Message);
            }
        }

        private void CaricaGiocatorinellalistview()
        {
            listView1.View = View.Details;
            listView1.Columns.Clear(); // Azzera le colonne esistenti
            listView1.Columns.Add("Numero Elenco", 100);
            listView1.Columns.Add("Nome Giocatore", 150);
            listView1.Columns.Add("Nazionalità", 100);
            listView1.Columns.Add("Posizione", 100);
            listView1.Columns.Add("Squadra", 150);
            listView1.Columns.Add("Campionato", 150);
            listView1.Columns.Add("Età", 50);
            listView1.Columns.Add("Anno di Nascita", 120);
            listView1.Columns.Add("Partite Giocate", 120);
            listView1.Columns.Add("Partite Titolare", 120);
            listView1.Columns.Add("Minuti Giocati", 120);
            listView1.Columns.Add("Minuti su 90", 100);
            listView1.Columns.Add("Gol", 50);
            listView1.Items.Clear(); // Azzera gli item esistenti

            foreach (var giocatore in listGiocatori)
            {
                ListViewItem item = new ListViewItem(giocatore.Numeroelenco.ToString());
                item.SubItems.Add(giocatore.Nomegiocatore);
                item.SubItems.Add(giocatore.Nazionalita);
                item.SubItems.Add(giocatore.Posizione);
                item.SubItems.Add(giocatore.Squadra);
                item.SubItems.Add(giocatore.Campionato);
                item.SubItems.Add(giocatore.Eta.ToString());
                item.SubItems.Add(giocatore.Annodinascita.ToString());
                item.SubItems.Add(giocatore.Partitegiocate.ToString());
                item.SubItems.Add(giocatore.Partitegiocatetit.ToString());
                item.SubItems.Add(giocatore.Minutigiocati.ToString());
                item.SubItems.Add(giocatore.Partitegiocatesunov.ToString());
                item.SubItems.Add(giocatore.Gol.ToString());

                listView1.Items.Add(item);
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs ordinaeriordinacolonna)
        {
            if (ordinaeriordinacolonna.Column == 0) // Indice della colonna "Numero Elenco"
            {
                listGiocatori.Reverse(); // Inverti l'ordine
            }
            else if (ordinaeriordinacolonna.Column == 2) // Indice della colonna "Nazionalità"
            {
                if (isNazionalitaAscending)
                {
                    listGiocatori.Sort((x, y) => string.Compare(x.Nazionalita, y.Nazionalita));
                }
                else
                {
                    listGiocatori.Sort((x, y) => string.Compare(y.Nazionalita, x.Nazionalita));
                }
                isNazionalitaAscending = !isNazionalitaAscending; // Alterna lo stato
            }
            else if (ordinaeriordinacolonna.Column == 3) // Indice della colonna "Posizione"
            {
                if (isPosizioneAscending)
                {
                    listGiocatori.Sort((x, y) => string.Compare(x.Posizione, y.Posizione));
                }
                else
                {
                    listGiocatori.Sort((x, y) => string.Compare(y.Posizione, x.Posizione));
                }
                isPosizioneAscending = !isPosizioneAscending; // Alterna lo stato
            }
            else if (ordinaeriordinacolonna.Column == 4) // Indice della colonna "Squadra"
            {
                if (isSquadraAscending)
                {
                    listGiocatori.Sort((x, y) => string.Compare(x.Squadra, y.Squadra));
                }
                else
                {
                    listGiocatori.Sort((x, y) => string.Compare(y.Squadra, x.Squadra));
                }
                isSquadraAscending = !isSquadraAscending; // Alterna lo stato
            }
            else if (ordinaeriordinacolonna.Column == 5) // Indice della colonna "Campionato"
            {
                if (isCampionatoAscending)
                {
                    listGiocatori.Sort((x, y) => string.Compare(x.Campionato, y.Campionato));
                }
                else
                {
                    listGiocatori.Sort((x, y) => string.Compare(y.Campionato, x.Campionato));
                }
                isCampionatoAscending = !isCampionatoAscending; // Alterna lo stato
            }

            CaricaGiocatorinellalistview(); // Ricarica la ListView
        }

        class giocatore
        {
            private int _numeroelenco;
            private string _nomegiocatore;
            private string _nazionalita;
            private string _posizione;
            private string _squadra;
            private string _campionato;
            private int _eta;
            private int _annodinascita;
            private int _partitegiocate;
            private int _partitegiocatetit;
            private int _minutigiocati;
            private double _partitegiocatesunov;
            private int _gol;

            public giocatore(
                int numeroelenco,
                string nomegiocatore,
                string nazionalita,
                string posizione,
                string squadra,
                string campionato,
                int eta,
                int annodinascita,
                int partitegiocate,
                int partitegiocatetit,
                int minutigiocati,
                double partitegiocatesunov,
                int gol)
            {
                Numeroelenco = numeroelenco;
                Nomegiocatore = nomegiocatore;
                Nazionalita = nazionalita;
                Posizione = posizione;
                Squadra = squadra;
                Campionato = campionato;
                Eta = eta;
                Annodinascita = annodinascita;
                Partitegiocate = partitegiocate;
                Partitegiocatetit = partitegiocatetit;
                Minutigiocati = minutigiocati;
                Partitegiocatesunov = partitegiocatesunov;
                Gol = gol;
            }

            public int Numeroelenco { get; set; }
            public string Nomegiocatore { get; set; }
            public string Nazionalita { get; set; }
            public string Posizione { get; set; }
            public string Squadra { get; set; }
            public string Campionato { get; set; }
            public int Eta { get; set; }
            public int Annodinascita { get; set; }
            public int Partitegiocate { get; set; }
            public int Partitegiocatetit { get; set; }
            public int Minutigiocati { get; set; }
            public double Partitegiocatesunov { get; set; }
            public int Gol { get; set; }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
