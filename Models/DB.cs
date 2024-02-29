using Microsoft.Data.SqlClient;

namespace PoliziaMunicipale.Models
{
    public class DB
    {
        public static string connectionString = "server=DESKTOP-GS9691M\\SQLEXPRESS; Initial Catalog=PoliziaMunicipale; Integrated Security=true; TrustServerCertificate=True";
        public static SqlConnection conn = new SqlConnection(connectionString);    
    }
}
