using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace DAL
{
    public class ConnectionManager
    {
        private readonly string connectionString;

        public ConnectionManager()
        {
            try
            {
                // Obtiene la cadena de App.config
                connectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
            }
            catch (ConfigurationErrorsException ex)
            {
                // Manejo de errores al obtener la cadena de conexión
                throw new System.Exception("Error al obtener la cadena de conexión desde App.config", ex);
            }

        }

        public OracleConnection GetConnection()
        {
            // Crear una nueva conexión cada vez
            return new OracleConnection(connectionString);
        }
    }
}