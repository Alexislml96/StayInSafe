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
    public class SitiosService : ISitios, IDisposable
    {
        bool disposedValue;
        IConnectionDB<Sitios> _conn;
        DynamicParameters _parameters = new DynamicParameters();
        public SitiosService(IConnectionDB<Sitios> conn)
        {
            _conn = conn;
        }
        public long AddSitio(Sitios sitio)
        {
            long id = 0;

            try
            {
                _parameters.Add("@p_user_json", JsonConvert.SerializeObject(sitio), DbType.String, ParameterDirection.Input);
                _conn.PrepararProcedimiento("dbo.[Sitios.AddSitio]", _parameters);
                id = (long)_conn.QueryFirstOrDefaultDapper(TipoDato.Numerico);
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                _conn.Dispose();
            }
        }

        public List<Sitios> GetSitios()
        {
            List<Sitios> list = new List<Sitios>();

            try
            {
                _conn.PrepararProcedimiento("dbo.[Sitios.GetSitios]", null);
                var Json = (string)_conn.QueryFirstOrDefaultDapper(TipoDato.Cadena);
                if (Json != string.Empty)
                {
                    JArray arr = JArray.Parse(Json);
                    foreach (JObject jsonOperaciones in arr.Children<JObject>())
                    {
                        list.Add(new Sitios()
                        {
                            Id_Sitio = Convert.ToInt32(jsonOperaciones["Id_Sitio"].ToString()),
                            Nombre = jsonOperaciones["Nombre"].ToString(),
                            Descripcion = jsonOperaciones["Descripcion"].ToString(),
                            Latitud = jsonOperaciones["Latitud"].ToString(),
                            Longitud = jsonOperaciones["Longitud"].ToString(),
                        });

                    }
                }

                return list;
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
