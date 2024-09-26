using System;
using System.Collections.Generic;
using System.IO;  // Per leggere i file
using System.Windows.Forms;

namespace Open_data
{
    public partial class Form1 : Form
    {
        private List<giocatore> listGiocatori;
        private string fileName = "Elenco.csv"; // Il path del file caricato

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listGiocatori = new List<giocatore>();

            // Carica i dati dal file CSV nella lista di giocatori
            CaricaGiocatoriDaCSV();

            // Popola la ListView1 con i dati
            CaricaGiocatorinellalistview();
        }

        private void CaricaGiocatoriDaCSV()
        {
            try
            {
                // Apri il file CSV
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    bool isFirstLine = true; // Per saltare l'intestazione

                    // Leggi ogni riga del file
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (isFirstLine)
                        {
                            isFirstLine = false; // Salta la prima riga
                            continue;
                        }

                        // Dividi la riga nei campi separati da virgola
                        string[] fields = line.Split(';');

                        // Assumi che il file CSV sia formattato correttamente
                        giocatore g = new giocatore(
                            int.Parse(fields[0]), // Numero elenco
                            fields[1],             // Nome giocatore
                            fields[2],             // Nazionalità
                            fields[3],             // Posizione
                            fields[4],             // Squadra
                            fields[5],             // Campionato
                            int.Parse(fields[6]),  // Età
                            int.Parse(fields[7]),  // Anno di nascita
                            int.Parse(fields[8]),  // Partite giocate
                            int.Parse(fields[9]),  // Partite giocate da titolare
                            int.Parse(fields[10]), // Minuti giocati
                            double.Parse(fields[11]), // Minuti su 90
                            int.Parse(fields[12])  // Gol
                        );

                        // Aggiungi il giocatore alla lista
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
            // Configura le colonne della ListView1
            listView1.View = View.Details;
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

            // Aggiungi i giocatori alla ListView
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

        class giocatore
        {
            // Campi della classe giocatore
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
    }
}
