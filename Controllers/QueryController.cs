using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PoliziaMunicipale.Models;

namespace PoliziaMunicipale.Controllers
{
    public class QueryController : Controller
    {
        public IActionResult VerbaliTrasgre()
        {
            List<VerbaliTrasgre> verbali = new List<VerbaliTrasgre>();
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand("SELECT v.IDAnagrafica, a.Cognome, a.Nome,  count(*) as TotVerbali  FROM Verbale as v  join dbo.Anagrafica as a on a.IDAnagrafica = v.IDAnagrafica  Group by v.IDAnagrafica, a.Cognome, a.Nome", DB.conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var verbaleTot = new VerbaliTrasgre()
                        {
                            Totale = (int)reader["TotVerbali"],
                            IDAnagrafica = (int)reader["IDAnagrafica"],
                            Nome = reader["Nome"].ToString(),
                            Cognome= reader["Cognome"].ToString(),
                         
                        };

                        verbali.Add(verbaleTot);

                    }

                    reader.Close();
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

            return View(verbali);
        }

        public IActionResult TrasgrePunti()
        {
            List<VerbaliTrasgre> verbali = new List<VerbaliTrasgre>();
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand("SELECT v.IDAnagrafica, a.Cognome, a.Nome,  sum(v.DecurtamentoPunti) as TotPunti FROM Verbale as v  join Anagrafica as a on a.IDAnagrafica = v.IDAnagrafica  Group by v.IDAnagrafica, a.Cognome, a.Nome", DB.conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var verbaleTot = new VerbaliTrasgre()
                        {
                            Totale = (int)reader["TotPunti"],
                            IDAnagrafica = (int)reader["IDAnagrafica"],
                            Nome = reader["Nome"].ToString(),
                            Cognome = reader["Cognome"].ToString(),

                        };

                        verbali.Add(verbaleTot);

                    }

                    reader.Close();
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

            return View(verbali);
        }

        public IActionResult Punti()
        {
            List<PuntiSaldo> verbali = new List<PuntiSaldo>();
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand("SELECT a.Cognome,a.Nome,  v.DataViolazione,  v.Importo, v.DecurtamentoPunti  FROM Verbale as v join Anagrafica as a on a.IDAnagrafica = v.IDAnagrafica  WHERE v.DecurtamentoPunti > 10", DB.conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var verbale = new PuntiSaldo()
                        {
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            DataViolazione = (DateTime)reader["DataViolazione"],
                            Importo = (decimal)reader["Importo"],
                            DecurtamentoPunti = (int)reader["DecurtamentoPunti"],

                        };

                        verbali.Add(verbale);

                    }

                    reader.Close();
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

            return View(verbali);
        }

        public IActionResult FiltroImporto()
        {
            List<PuntiSaldo> verbali = new List<PuntiSaldo>();
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand("SELECT a.Cognome, a.Nome, v.DataViolazione,  v.Importo, v.DecurtamentoPunti  FROM Verbale as v join Anagrafica as a on a.IDAnagrafica = v.IDAnagrafica  WHERE v.Importo > 400", DB.conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var verbale = new PuntiSaldo()
                        {
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            DataViolazione = (DateTime)reader["DataViolazione"],
                            Importo = (decimal)reader["Importo"],
                            DecurtamentoPunti = (int)reader["DecurtamentoPunti"],

                        };

                        verbali.Add(verbale);

                    }

                    reader.Close();
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

            return View(verbali);
        }








    }
}
