using Alexis.CORE.Connection.Interfaces;
using Alexis.CORE.Connection.Models;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StayInSafe.Core.Interfaces;
using StayInSafe.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Services
{
    public class LoginService : ILogin, IDisposable
    {
        private bool disposedValue;
        private IConnectionDB<Users> _conn;
        DynamicParameters _parameters = new DynamicParameters();
        public LoginService(IConnectionDB<Users> conn)
        {
            _conn = conn;
        }
        public Users Login(LoginModel login)
        {
            try
            {
                Users u = new Users();
                _parameters.Add("@p_login_json", JsonConvert.SerializeObject(login), DbType.String, ParameterDirection.Input);
                _conn.PrepararProcedimiento("dbo.[USERS.Login]", _parameters);
                var Json = (string)_conn.QueryFirstOrDefaultDapper(TipoDato.Cadena);
                if (Json != string.Empty)
                {
                    JArray arr = JArray.Parse(Json);
                    foreach (JObject jsonOperaciones in arr.Children<JObject>())
                    {
                        u = new Users()
                        {
                            Id = Convert.ToInt32(jsonOperaciones["Id"].ToString()),
                            Email = jsonOperaciones["Email"].ToString(),
                            P_Nombre = jsonOperaciones["Primer Nombre"].ToString(),
                            S_Nombre = jsonOperaciones["Segundo Nombre"].ToString(),
                            Apellido_Paterno = jsonOperaciones["Apellido Paterno"].ToString(),
                            Apellido_Materno = jsonOperaciones["Primer Nombre"].ToString(),
                        };

                    }
                }
                return u;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message);
            }
            catch (MySqlException mysqlEx)
            {
                throw new Exception(mysqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _conn.Dispose();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _conn.Dispose();
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
