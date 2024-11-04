using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace Open_data
{
    public partial class Form1 : Form
    {
        private List<giocatore> listGiocatori;
        private List<giocatore> listGiocatoriOriginale;
        private string fileName = "Elenco.csv";
        private string fileNazioni = "Elenco nazioni.csv";
        private List<nazionalita> listNazionalita;
        private bool isNazionalitaAscending = true; //Variabili necessarie per gli ordinamenti (true crescente -- false decrescente)
        private bool isPosizioneAscending = true;
        private bool isSquadraAscending = true;
        private bool isCampionatoAscending = true;
        private bool isEtaAscending = true;
        private bool isAnnoNascitaAscending = true;
        private bool isPartiteGiocateAscending = true;
        private bool isPartiteTitolareAscending = true;
        private bool isMinutiGiocatiAscending = true;
        private bool isMinutiSu90Ascending = true;
        private bool isGolAscending = true;
        private bool isNumeroElencoAscending = true;
        private bool isNomeGiocatoreAscending = true;

        public Form1()
        {
            InitializeComponent();
            listView1.ColumnClick += listView1_ColumnClick; // Associa l'evento del click della colonna
            listView1.ItemActivate += ListView1_ItemActivate; // Associa l'evento dell'attivamento di un item della listview
            listView1.FullRowSelect = true; // Garantisce la possibilità di selezionare righe intere
            chartConfrontoGiocatori.Visible = false; // Nasconde il grafico
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button8.Visible = false;
            listGiocatori = new List<giocatore>(); //Inizializza le liste di giocatori
            listGiocatoriOriginale = new List<giocatore>();
            listNazionalita = new List<nazionalita>();
            textBox1.TextChanged += textBox1_TextChanged;
            CaricaGiocatoriDaCSV();
            CaricaNazioniDaCSV();
            CaricaGiocatorinellalistview();
            PopolaComboBoxNazionalita();
            PopolaComboBoxCampionato();
            PopolaComboBoxSquadre(comboBox2.Text);
        }

        private void ListView1_ItemActivate(object sender, EventArgs e)
        {
            // Verifica se c'è un elemento selezionato
            if (listView1.SelectedItems.Count > 0)
            {
                // Ottieni l'elemento selezionato
                var selectedItem = listView1.SelectedItems[0];

                // Seleziona nome giocatore (colonna 2)
                string playerName = selectedItem.SubItems[1].Text;

                // Costruisci l'URL per la pagina Wikipedia del giocatore
                string wikipediaUrl = $"https://en.wikipedia.org/wiki/{playerName.Replace(" ", "_")}";

                // Usa Process.Start per aprire l'URL
                Process.Start(new ProcessStartInfo
                {
                    FileName = wikipediaUrl,
                    UseShellExecute = true // Necessario per aprire l'URL nel browser predefinito
                });
            }
        }
        private void FiltraGiocatori()
        {
            string testoRicerca = textBox1.Text.Trim().ToLower();

            // Filtra la lista di giocatori in base al nome
            var giocatoriFiltrati = listGiocatori
                .Where(g => g.Nomegiocatore.ToLower().Contains(testoRicerca))
                .ToList();

            // Aggiorna la ListView con i giocatori filtrati
            AggiornaListView(giocatoriFiltrati);
        }
        private void AggiornaListView(List<giocatore> giocatori)
        {
            listView1.Items.Clear();

            foreach (var g in giocatori)
            {
                var item = new ListViewItem(g.Numeroelenco.ToString());
                item.SubItems.Add(g.Nomegiocatore);
                item.SubItems.Add(g.Nazionalita);
                item.SubItems.Add(g.Posizione);
                item.SubItems.Add(g.Squadra);
                item.SubItems.Add(g.Campionato);
                item.SubItems.Add(g.Eta.ToString());
                item.SubItems.Add(g.Annodinascita.ToString());
                item.SubItems.Add(g.Partitegiocate.ToString());
                item.SubItems.Add(g.Partitegiocatetit.ToString());
                item.SubItems.Add(g.Minutigiocati.ToString());
                item.SubItems.Add(g.Partitegiocatesunov.ToString("F2"));
                item.SubItems.Add(g.Gol.ToString());

                listView1.Items.Add(item);
            }
        }



        private void PopolaComboBoxNazionalita()
        {
            // Estrai tutte le nazionalità dalla lista dei giocatori (Distinct per evitare duplicati) (Orderby per ordinarli in maniera alfabetica)
            var nazionalita = listGiocatori.Select(g => g.Nazionalita).Distinct().OrderBy(n => n).ToList();

            // Aggiungi un'opzione "Tutte" per visualizzare tutti i giocatori
            comboBox1.Items.Add("Tutte");

            // Aggiungi tutte le nazionalità alla ComboBox
            comboBox1.Items.AddRange(nazionalita.ToArray());

            // Seleziona "Tutte" di default
            comboBox1.SelectedIndex = 0;
        }
        private void PopolaComboBoxSquadre(string campionatoSelezionato)
        {
            // Estrai tutte le squadre dal campionato selezionato
            var squadre = listGiocatori
                .Where(g => g.Campionato == campionatoSelezionato) // Filtra per campionato
                .Select(g => g.Squadra) //Seleziona il campo "squadra"
                .Distinct() //Senza doppioni
                .OrderBy(s => s) //Ordine alfabetico
                .ToList(); //In una lista

            // Pulisci il ComboBox prima di aggiungere nuovi elementi
            comboBox3.Items.Clear();

            // Aggiungi un'opzione "Tutte" per visualizzare tutti i giocatori
            comboBox3.Items.Add("Tutte");

            // Aggiungi tutte le squadre alla ComboBox
            comboBox3.Items.AddRange(squadre.ToArray());

            // Seleziona "Tutte" di default
            comboBox3.SelectedIndex = 0;
        }


        private void PopolaComboBoxCampionato()
        {
            // Estrai tutti i campionati distinti dalla lista dei giocatori
            var campionati = listGiocatori.Select(g => g.Campionato).Distinct().OrderBy(c => c).ToList();

            // Aggiungi un'opzione "Tutti" per visualizzare tutti i giocatori
            comboBox2.Items.Add("Tutti");

            // Aggiungi tutti i campionati alla ComboBox
            comboBox2.Items.AddRange(campionati.ToArray());

            // Seleziona "Tutti" di default
            comboBox2.SelectedIndex = 0;
        }

        private void CaricaGiocatoriDaCSV()
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName, Encoding.UTF8))
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
        private void CaricaNazioniDaCSV()
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileNazioni))
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

                        // Verifica che ci siano almeno due campi
                        if (fields.Length >= 2)
                        {
                            nazionalita n = new nazionalita(
                                fields[0],
                                fields[1]
                            );
                            listNazionalita.Add(n);
                        }
                        else
                        {
                            Console.WriteLine($"Riga incompleta ignorata: {line}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore nella lettura del file CSV: " + ex.Message);
            }
        }
        private void AggiornaFiltri()
        {
            // Ottieni i valori selezionati dai filtri
            string nazionalitaSelezionata = comboBox1.SelectedItem?.ToString() ?? "Tutte";
            string campionatoSelezionato = comboBox2.SelectedItem?.ToString() ?? "Tutti";
            string squadraSelezionata = comboBox3.SelectedItem?.ToString() ?? "Tutte";

            // Recupera il numero minimo di gol se specificato
            int golMinimi = 0;
            if (!string.IsNullOrEmpty(textBox2.Text) && !int.TryParse(textBox2.Text, out golMinimi))
            {
                MessageBox.Show("Inserisci un numero intero valido per i gol.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Filtra i giocatori in base ai criteri selezionati
            var giocatoriFiltrati = listGiocatori.Where(g =>
                (nazionalitaSelezionata == "Tutte" || g.Nazionalita == nazionalitaSelezionata) &&
                (campionatoSelezionato == "Tutti" || g.Campionato == campionatoSelezionato) &&
                (squadraSelezionata == "Tutte" || g.Squadra == squadraSelezionata) &&
                (golMinimi == 0 || g.Gol >= golMinimi)
            ).ToList();

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

            Progress.Maximum = listGiocatori.Count;
            Progress.Value = 0;

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

                Progress.Value += 1;
            }

            Progress.Visible = false;
        }
        public void ConfrontaGiocatori(int numeroElencoGiocatore1, int numeroElencoGiocatore2)
        {
            // Trova i giocatori in base ai numeri di elenco forniti
            var giocatore1 = listGiocatori.FirstOrDefault(g => g.Numeroelenco == numeroElencoGiocatore1);
            var giocatore2 = listGiocatori.FirstOrDefault(g => g.Numeroelenco == numeroElencoGiocatore2);

            // Controlla che entrambi i giocatori esistano
            if (giocatore1 == null || giocatore2 == null)
            {
                MessageBox.Show("Uno o entrambi i giocatori non sono stati trovati.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ripulisce il grafico prima di aggiungere nuove serie
            chartConfrontoGiocatori.Series.Clear();
            chartConfrontoGiocatori.Titles.Clear();
            chartConfrontoGiocatori.Titles.Add("Confronto Statistiche Giocatori");

            // Aggiungi serie per ciascun giocatore
            Series serieGiocatore1 = new Series(giocatore1.Nomegiocatore);
            Series serieGiocatore2 = new Series(giocatore2.Nomegiocatore);

            // Imposta il tipo di grafico a barre
            serieGiocatore1.ChartType = SeriesChartType.Bar;
            serieGiocatore2.ChartType = SeriesChartType.Bar;

            // Aggiungi i dati del giocatore 1
            serieGiocatore1.Points.AddXY("Gol Fatti", giocatore1.Gol);
            serieGiocatore1.Points.AddXY("Partite Giocate", giocatore1.Partitegiocate);
            serieGiocatore1.Points.AddXY("Partite da Titolare", giocatore1.Partitegiocatetit);

            // Aggiungi i dati del giocatore 2
            serieGiocatore2.Points.AddXY("Gol Fatti", giocatore2.Gol);
            serieGiocatore2.Points.AddXY("Partite Giocate", giocatore2.Partitegiocate);
            serieGiocatore2.Points.AddXY("Partite da Titolare", giocatore2.Partitegiocatetit);

            // Aggiungi le serie al grafico
            chartConfrontoGiocatori.Series.Add(serieGiocatore1);
            chartConfrontoGiocatori.Series.Add(serieGiocatore2);

            // Imposta le proprietà del grafico per una migliore visualizzazione
            chartConfrontoGiocatori.ChartAreas[0].AxisX.Title = "Statistiche";
            chartConfrontoGiocatori.ChartAreas[0].AxisY.Title = "Valore";
            chartConfrontoGiocatori.ChartAreas[0].RecalculateAxesScale();
        }


        private void listView1_ColumnClick(object sender, ColumnClickEventArgs ordinaeriordinacolonna)
        {
            if (ordinaeriordinacolonna.Column == 0) // Indice della colonna "Numero Elenco"
            {
                if (isNumeroElencoAscending) //Inizializzata come true quindi ordine crecente
                {
                    listGiocatori.Sort((x, y) => x.Numeroelenco.CompareTo(y.Numeroelenco));
                }
                else
                {
                    listGiocatori.Sort((x, y) => y.Numeroelenco.CompareTo(x.Numeroelenco));
                }
                isNumeroElencoAscending = !isNumeroElencoAscending; //Portata a false quindi ordine decrescente
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
            Progress.Visible = true;

            // Ordina i giocatori in base al Numeroelenco in maniera crescente
            listGiocatori.Sort((x, y) => x.Numeroelenco.CompareTo(y.Numeroelenco));

            // Ricarica la ListView con l'ordine per Numeroelenco crescente
            CaricaGiocatorinellalistview();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nomeGiocatoreDaCercare = textBox1.Text;

            if (nomeGiocatoreDaCercare == "")
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
            if (textBox2.Text == "")
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
            textBox2.Clear();

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

        private void button5_Click(object sender, EventArgs e)
        {
            // Prendi il campionato selezionato nella ComboBox2
            string campionatoSelezionato = comboBox2.SelectedItem.ToString();

            // Filtra i giocatori in base al campionato
            List<giocatore> giocatoriFiltrati;

            if (campionatoSelezionato == "Tutti")
            {
                // Se è selezionata l'opzione "Tutti", mostra tutti i giocatori
                giocatoriFiltrati = listGiocatori;
            }
            else
            {
                // Filtra i giocatori per il campionato selezionato
                giocatoriFiltrati = listGiocatori.Where(g => g.Campionato == campionatoSelezionato).ToList();
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

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Prendi la squadra selezionata nella ComboBox
            string squadraSelezionata = comboBox3.SelectedItem.ToString();
            string campionatoSelezionato = comboBox2.SelectedItem.ToString();
            // Filtra i giocatori in base alla squadra e al campionato
            List<giocatore> giocatoriFiltrati;

            if (squadraSelezionata == "Tutte")
            {
                // Se è selezionata l'opzione "Tutte", mostra tutti i giocatori del campionato selezionato
                giocatoriFiltrati = listGiocatori.Where(g => g.Campionato == campionatoSelezionato).ToList();
            }
            else
            {
                // Filtra i giocatori per la squadra e il campionato selezionato
                giocatoriFiltrati = listGiocatori
                    .Where(g => g.Squadra == squadraSelezionata && g.Campionato == campionatoSelezionato)
                    .ToList();
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            AggiornaFiltri();
            string campionatoSelezionato = comboBox2.SelectedItem.ToString();
            PopolaComboBoxSquadre(campionatoSelezionato);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            AggiornaFiltri();
            string campionatoSelezionato = comboBox2.SelectedItem.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AggiornaFiltri();
            string abbtre = comboBox1.SelectedItem.ToString();
            string abbdue = "";
            if (abbtre.Length == 3)
            {
                foreach (var nazionalita in listNazionalita)
                {
                    if (abbtre == nazionalita.Abbreviazionetre) //Se coincide l'abbreviazazione a tre lettere, assegno alla nazione due il corrrispondente nel file della nazionalità a due lettere
                    {
                        abbdue = nazionalita.Abbreviazionedue;
                    }
                }
                nazionalita selectedNazionalita = new nazionalita(abbtre, abbdue);
                string percorsoBandiera = $"{selectedNazionalita.PercorsoBandiera()}"; // Assicurati di avere l'immagine nella directory corretta

                try
                {
                    // Carica l'immagine nella PictureBox
                    pictureBox1.Image = Image.FromFile(percorsoBandiera);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errore nel caricamento dell'immagine: {ex.Message}");
                }
            }
            else
            {
                pictureBox1.Image = null;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int numeroElencoGiocatore1, numeroElencoGiocatore2;

            if (!int.TryParse(textBox3.Text, out numeroElencoGiocatore1))
            {
                MessageBox.Show("Il valore inserito per il giocatore 1 non è valido. Inserisci un numero intero.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!int.TryParse(textBox4.Text, out numeroElencoGiocatore2))
            {
                MessageBox.Show("Il valore inserito per il giocatore 2 non è valido. Inserisci un numero intero.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ConfrontaGiocatori(numeroElencoGiocatore1, numeroElencoGiocatore2);
                chartConfrontoGiocatori.Visible = true;
                button8.Visible = true;
            }
        }

        private void comboBoxNazionalita_TextUpdate(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            FiltraGiocatori();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button8.Visible = false;
            chartConfrontoGiocatori.Visible = false;
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
    public class nazionalita
    {
        public string Abbreviazionetre { get; set; }
        public string Abbreviazionedue { get; set; }
        public nazionalita(string abbreviazionetre, string abbreviazionedue)
        {
            Abbreviazionetre = abbreviazionetre;
            Abbreviazionedue = abbreviazionedue;
        }
        public string PercorsoBandiera()
        {
            return $"{Abbreviazionedue.ToLower()}.png";
        }

    }
}


