using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PoliziaMunicipale.Models;
using System.Diagnostics;

namespace PoliziaMunicipale.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            List<Verbale> verbali = []; 
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand("select * FROM Verbale as v  join Violazione as vio on vio.IDViolazione = v.IDViolazione", DB.conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var violazione = new Verbale()
                        {
                            IDVerbale = (int)reader["IDVerbale"],
                            DataViolazione = (DateTime)reader["DataViolazione"],
                            IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                            Nominativo_Agente = reader["Nominativo_Agente"].ToString(),
                            DataTrascrizioneVerbale = (DateTime)reader["DataTrascrizioneVerbale"],
                            Importo = (decimal)reader["Importo"],
                            DecurtamentoPunti = (int)reader["DecurtamentoPunti"],
                            IDAnagrafica = (int)reader["IDAnagrafica"],
                            Descrizione = reader["Descrizione"].ToString()
                        };

                     verbali.Add(violazione);

                    }

                    reader.Close();
                }
            }catch (Exception ex) 
            {
                return View("Error");
            }
            finally 
            {
                DB.conn.Close(); 
            }

            return View(verbali);
        }

        [HttpGet]
        public IActionResult Anagrafica([FromRoute] int? id)
        {
            var utente = new Anagrafica();

            if (id.HasValue)
            {
                try
                {
                    DB.conn.Open();
                    var cmd = new SqlCommand("select * from Anagrafica where IDAnagrafica=@IDAn ", DB.conn);
                    cmd.Parameters.AddWithValue("@IDAn", id);
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        utente.IDAnagrafica = (int)reader["IDAnagrafica"];
                        utente.Nome = reader["Nome"].ToString();
                        utente.Cognome = reader["Cognome"].ToString();
                        utente.Indirizzo = reader["Indirizzo"].ToString();
                        utente.Citta = reader["Citta"].ToString();
                        utente.CAP = reader["CAP"].ToString();
                        utente.Cod_Fisc = reader["Cod_Fisc"].ToString();

                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    return View("Error");
                }
                finally
                {
                    DB.conn.Close();
                }
                return View(utente);
            }
            else
            {
                return View("Error");
            }

           
        }
        //AGGIUNGE VERBALE
        [HttpGet]
        public IActionResult Add()
        {
            List<Anagrafica> utenti = new List<Anagrafica>();
            List<Violazione> violazioni = new List<Violazione>(); 

            try
            {
                DB.conn.Open();

                // Carica le violazioni
                var cmdViolazioni = new SqlCommand("SELECT * FROM Violazione", DB.conn);
                var readerViolazioni = cmdViolazioni.ExecuteReader();

                if (readerViolazioni.HasRows)
                {
                    while (readerViolazioni.Read())
                    {
                        var violazione = new Violazione()
                        {
                            IDViolazione = (int)readerViolazioni["IDViolazione"],
                            Descrizione = readerViolazioni["Descrizione"].ToString()
                        };

                        violazioni.Add(violazione);
                    }

                    readerViolazioni.Close();
                }

                // Carica gli utenti
                var cmdUtenti = new SqlCommand("SELECT * FROM Anagrafica", DB.conn);
                var readerUtenti = cmdUtenti.ExecuteReader();

                if (readerUtenti.HasRows)
                {
                    while (readerUtenti.Read())
                    {
                        var utente = new Anagrafica()
                        {
                            IDAnagrafica = (int)readerUtenti["IDAnagrafica"],
                            Nome = readerUtenti["Nome"].ToString(),
                            Cognome = readerUtenti["Cognome"].ToString(),
                            Indirizzo = readerUtenti["Indirizzo"].ToString(),
                            Citta = readerUtenti["Citta"].ToString(),
                            CAP = readerUtenti["CAP"].ToString(),
                            Cod_Fisc = readerUtenti["Cod_Fisc"].ToString()
                        };

                        utenti.Add(utente);
                    }

                    readerUtenti.Close();
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            finally
            {
                DB.conn.Close();
            }




            ViewBag.Violazioni = violazioni;
            ViewBag.Utenti = utenti;

            return View();
        }

        [HttpPost]
        public IActionResult Add(Verbale verbale)
        {
            var error = true;
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand(@"INSERT INTO Verbale 
                                           (DataViolazione, IndirizzoViolazione, Nominativo_Agente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, IDAnagrafica, IDViolazione)
                                           VALUES (@dataViolazione, @indirizzoViolazione, @nominativo_agente, @dataTrascrizioneVerbale, @importo, @decurtamentoPunti, @idAnagrafica, @idViolazione)", DB.conn);
                cmd.Parameters.AddWithValue("@dataViolazione", verbale.DataViolazione);
                cmd.Parameters.AddWithValue("@indirizzoViolazione", verbale.IndirizzoViolazione);
                cmd.Parameters.AddWithValue("@nominativo_agente", verbale.Nominativo_Agente);
                cmd.Parameters.AddWithValue("@dataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale);
                cmd.Parameters.AddWithValue("@importo", verbale.Importo);
                cmd.Parameters.AddWithValue("@decurtamentoPunti", verbale.DecurtamentoPunti);
                cmd.Parameters.AddWithValue("@idAnagrafica", verbale.IDAnagrafica);
                cmd.Parameters.AddWithValue("@idViolazione", verbale.IDViolazione);

                var nRows = cmd.ExecuteNonQuery();
                if (nRows > 0)
                {
                    error = false;
                }

            }
            catch (Exception ex)
            {
                
                return View(ex.Message);
            }
            finally
            {
                DB.conn.Close();
            }


            if (!error)
            {
                TempData["MessageSuccess"] = $"Il verbale è stato correttamente aggiunto alla base dati";
            }
            else
            {
                TempData["MessageError"] = $"Errore durante il caricamento nella base dati.";

            }

            return RedirectToAction("Index");
        }

        //AGGIUNGE TRASGRESSORE

        [HttpGet]
        public IActionResult AddAnagrafica()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult AddAnagrafica(Anagrafica utente)
        {
            var error = true;
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand(@"INSERT INTO Anagrafica 
                                    (Cognome, Nome, Indirizzo, Citta, CAP, Cod_Fisc)
                                    VALUES (@cognome, @nome, @indirizzo, @citta, @cap, @cod_fisc)", DB.conn);
                cmd.Parameters.AddWithValue("@cognome", utente.Cognome);
                cmd.Parameters.AddWithValue("@nome", utente.Nome);
                cmd.Parameters.AddWithValue("@indirizzo", utente.Indirizzo);
                cmd.Parameters.AddWithValue("@citta", utente.Citta);
                cmd.Parameters.AddWithValue("@cap", utente.CAP);
                cmd.Parameters.AddWithValue("@cod_fisc", utente.Cod_Fisc);


                var nRows = cmd.ExecuteNonQuery();

                if (nRows>0) 
                {
                    error = false; 
                }

            }
            catch (Exception ex)
            {
                
                return View(ex.Message);
            }
            finally
            {
                DB.conn.Close();
            }

            if (!error)
            {
                TempData["MessageSuccess"] = $"L'utente {utente.Nome} {utente.Cognome} è stato aggiunto in anagrafica.";
            }
            else
            {
                TempData["MessageError"] = $"Errore durante il caricamento nella base dati.";

            }

            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
