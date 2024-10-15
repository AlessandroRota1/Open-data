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
        private List<giocatore> listGiocatoriOriginale; // Lista per memorizzare l'ordine originale
        private string fileName = "Elenco.csv"; // Il path del file caricato
        private bool isNazionalitaAscending = true;
        private bool isPosizioneAscending = true;
        private bool isSquadraAscending = true;
        private bool isCampionatoAscending = true;

        // Aggiunta variabili per le nuove colonne
        private bool isEtaAscending = true;
        private bool isAnnoNascitaAscending = true;
        private bool isPartiteGiocateAscending = true;
        private bool isPartiteTitolareAscending = true;
        private bool isMinutiGiocatiAscending = true;
        private bool isMinutiSu90Ascending = true;
        private bool isGolAscending = true;

        // Aggiunta variabili per "Numero Elenco" e "Nome Giocatore"
        private bool isNumeroElencoAscending = true;
        private bool isNomeGiocatoreAscending = true;

        public Form1()
        {
            InitializeComponent();
            listView1.ColumnClick += listView1_ColumnClick; // Associa l'evento
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listGiocatori = new List<giocatore>();
            listGiocatoriOriginale = new List<giocatore>(); // Inizializza la lista per l'ordine originale

            CaricaGiocatoriDaCSV();
            CaricaGiocatorinellalistview();
            PopolaComboBoxNazionalita();
        }
        private void PopolaComboBoxNazionalita()
        {
            // Estrai tutte le nazionalità dalla lista dei giocatori
            var nazionalita = listGiocatori.Select(g => g.Nazionalita).Distinct().OrderBy(n => n).ToList();

            // Aggiungi un'opzione "Tutte" per visualizzare tutti i giocatori
            comboBox1.Items.Add("Tutte");

            // Aggiungi tutte le nazionalità alla ComboBox
            comboBox1.Items.AddRange(nazionalita.ToArray());

            // Seleziona "Tutte" di default
            comboBox1.SelectedIndex = 0;
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

                    // Crea una copia della lista originale
                    listGiocatoriOriginale = new List<giocatore>(listGiocatori);
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
                if (isNumeroElencoAscending)
                {
                    listGiocatori.Sort((x, y) => x.Numeroelenco.CompareTo(y.Numeroelenco));
                }
                else
                {
                    listGiocatori.Sort((x, y) => y.Numeroelenco.CompareTo(x.Numeroelenco));
                }
                isNumeroElencoAscending = !isNumeroElencoAscending;
            }
            else if (ordinaeriordinacolonna.Column == 1) // Indice della colonna "Nome Giocatore"
            {
                if (isNomeGiocatoreAscending)
                {
                    listGiocatori.Sort((x, y) => string.Compare(x.Nomegiocatore, y.Nomegiocatore));
                }
                else
                {
                    listGiocatori.Sort((x, y) => string.Compare(y.Nomegiocatore, x.Nomegiocatore));
                }
                isNomeGiocatoreAscending = !isNomeGiocatoreAscending;
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
                isNazionalitaAscending = !isNazionalitaAscending;
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
                isPosizioneAscending = !isPosizioneAscending;
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
                isSquadraAscending = !isSquadraAscending;
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
                isCampionatoAscending = !isCampionatoAscending;
            }
            else if (ordinaeriordinacolonna.Column == 6) // Indice della colonna "Età"
            {
                if (isEtaAscending)
                {
                    listGiocatori.Sort((x, y) => x.Eta.CompareTo(y.Eta));
                }
                else
                {
                    listGiocatori.Sort((x, y) => y.Eta.CompareTo(x.Eta));
                }
                isEtaAscending = !isEtaAscending;
            }
            else if (ordinaeriordinacolonna.Column == 7) // Indice della colonna "Anno di Nascita"
            {
                if (isAnnoNascitaAscending)
                {
                    listGiocatori.Sort((x, y) => x.Annodinascita.CompareTo(y.Annodinascita));
                }
                else
                {
                    listGiocatori.Sort((x, y) => y.Annodinascita.CompareTo(x.Annodinascita));
                }
                isAnnoNascitaAscending = !isAnnoNascitaAscending;
            }
            else if (ordinaeriordinacolonna.Column == 8) // Indice della colonna "Partite Giocate"
            {
                if (isPartiteGiocateAscending)
                {
                    listGiocatori.Sort((x, y) => x.Partitegiocate.CompareTo(y.Partitegiocate));
                }
                else
                {
                    listGiocatori.Sort((x, y) => y.Partitegiocate.CompareTo(x.Partitegiocate));
                }
                isPartiteGiocateAscending = !isPartiteGiocateAscending;
            }
            else if (ordinaeriordinacolonna.Column == 9) // Indice della colonna "Partite Titolare"
            {
                if (isPartiteTitolareAscending)
                {
                    listGiocatori.Sort((x, y) => x.Partitegiocatetit.CompareTo(y.Partitegiocatetit));
                }
                else
                {
                    listGiocatori.Sort((x, y) => y.Partitegiocatetit.CompareTo(x.Partitegiocatetit));
                }
                isPartiteTitolareAscending = !isPartiteTitolareAscending;
            }
            else if (ordinaeriordinacolonna.Column == 10) // Indice della colonna "Minuti Giocati"
            {
                if (isMinutiGiocatiAscending)
                {
                    listGiocatori.Sort((x, y) => x.Minutigiocati.CompareTo(y.Minutigiocati));
                }
                else
                {
                    listGiocatori.Sort((x, y) => y.Minutigiocati.CompareTo(x.Minutigiocati));
                }
                isMinutiGiocatiAscending = !isMinutiGiocatiAscending;
            }
            else if (ordinaeriordinacolonna.Column == 11) // Indice della colonna "Minuti su 90"
            {
                if (isMinutiSu90Ascending)
                {
                    listGiocatori.Sort((x, y) => x.Partitegiocatesunov.CompareTo(y.Partitegiocatesunov));
                }
                else
                {
                    listGiocatori.Sort((x, y) => y.Partitegiocatesunov.CompareTo(x.Partitegiocatesunov));
                }
                isMinutiSu90Ascending = !isMinutiSu90Ascending;
            }
            else if (ordinaeriordinacolonna.Column == 12) // Indice della colonna "Gol"
            {
                if (isGolAscending)
                {
                    listGiocatori.Sort((x, y) => x.Gol.CompareTo(y.Gol));
                }
                else
                {
                    listGiocatori.Sort((x, y) => y.Gol.CompareTo(x.Gol));
                }
                isGolAscending = !isGolAscending;
            }

            CaricaGiocatorinellalistview();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Ordina i giocatori in base al Numeroelenco in maniera crescente
            listGiocatori.Sort((x, y) => x.Numeroelenco.CompareTo(y.Numeroelenco));

            // Ricarica la ListView con l'ordine per Numeroelenco crescente
            CaricaGiocatorinellalistview();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nomeGiocatoreDaCercare = textBox1.Text;

            if (nomeGiocatoreDaCercare=="")
            {
                MessageBox.Show("Inserisci il nome di un giocatore.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cerca il giocatore nella lista in base al nome
            giocatore giocatoreTrovato = listGiocatori.FirstOrDefault(g => g.Nomegiocatore.Equals(nomeGiocatoreDaCercare, StringComparison.OrdinalIgnoreCase));

            if (giocatoreTrovato != null)
            {
                // Crea una stringa con le informazioni del giocatore
                string infoGiocatore = $"Nome: {giocatoreTrovato.Nomegiocatore}\n" +
                                       $"Nazionalità: {giocatoreTrovato.Nazionalita}\n" +
                                       $"Posizione: {giocatoreTrovato.Posizione}\n" +
                                       $"Squadra: {giocatoreTrovato.Squadra}\n" +
                                       $"Campionato: {giocatoreTrovato.Campionato}\n" +
                                       $"Età: {giocatoreTrovato.Eta}\n" +
                                       $"Anno di Nascita: {giocatoreTrovato.Annodinascita}\n" +
                                       $"Partite Giocate: {giocatoreTrovato.Partitegiocate}\n" +
                                       $"Partite Titolare: {giocatoreTrovato.Partitegiocatetit}\n" +
                                       $"Minuti Giocati: {giocatoreTrovato.Minutigiocati}\n" +
                                       $"Minuti su 90: {giocatoreTrovato.Partitegiocatesunov}\n" +
                                       $"Gol: {giocatoreTrovato.Gol}";

                // Mostra il messaggio
                MessageBox.Show(infoGiocatore, "Dettagli Giocatore", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Giocatore non trovato.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Controlla se l'input è vuoto
            if (textBox2.Text=="")
            {
                MessageBox.Show("Inserisci un numero valido di gol.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Prova a convertire il valore della TextBox in un numero
            if (!int.TryParse(textBox2.Text, out int golMinimi))
            {
                MessageBox.Show("Inserisci un numero intero valido.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Filtra i giocatori con un numero di gol maggiore del valore inserito
            var giocatoriFiltrati = listGiocatori.Where(g => g.Gol >= golMinimi).ToList();

            // Controlla se sono stati trovati giocatori che soddisfano i criteri
            if (giocatoriFiltrati.Count == 0)
            {
                MessageBox.Show("Nessun giocatore trovato con un numero di gol maggiore di " + golMinimi, "Risultato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Aggiorna la ListView con i giocatori filtrati
            listView1.Items.Clear();
            foreach (var giocatore in giocatoriFiltrati)
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

        private void button4_Click(object sender, EventArgs e)
        {
            // Prendi la nazionalità selezionata nella ComboBox
            string nazionalitaSelezionata = comboBox1.SelectedItem.ToString();

            // Filtra i giocatori in base alla nazionalità
            List<giocatore> giocatoriFiltrati;

            if (nazionalitaSelezionata == "Tutte")
            {
                // Se è selezionata l'opzione "Tutte", mostra tutti i giocatori
                giocatoriFiltrati = listGiocatori;
            }
            else
            {
                // Filtra i giocatori per la nazionalità selezionata
                giocatoriFiltrati = listGiocatori.Where(g => g.Nazionalita == nazionalitaSelezionata).ToList();
            }

            // Aggiorna la ListView con i giocatori filtrati
            listView1.Items.Clear();
            foreach (var giocatore in giocatoriFiltrati)
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
    }
}


public class giocatore
    {
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

        public giocatore(int numeroelenco, string nomegiocatore, string nazionalita, string posizione, string squadra, string campionato, int eta, int annodinascita, int partitegiocate, int partitegiocatetit, int minutigiocati, double partitegiocatesunov, int gol)
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
    }

