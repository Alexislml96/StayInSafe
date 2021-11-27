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
    public class ContactosService : IContactos, IDisposable
    {
        private bool disposedValue;
        private IConnectionDB<Contactos> _conn;
        DynamicParameters _parameters = new DynamicParameters();

        public ContactosService(IConnectionDB<Contactos> conn)
        {
            _conn = conn;
        }

        public long AddContact(Contactos contacto)
        {
            long id = 0;

            try
            {
                _parameters.Add("@p_user_json", JsonConvert.SerializeObject(contacto), DbType.String, ParameterDirection.Input);
                _conn.PrepararProcedimiento("dbo.[Contactos.Add]", _parameters);
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

        public void DeleteContact(int idContact)
        {
            try
            {
                _parameters.Add("@Id", idContact, DbType.Int32, ParameterDirection.Input);
                _conn.PrepararProcedimiento("dbo.[Contacts.DeleteContact]", _parameters);
                var affectedRows = _conn.Query();
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

        public IEnumerable<Contactos> GetContactsById(int id)
        {
            IEnumerable<Contactos> list = new List<Contactos>();

            try
            {
                _parameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                _conn.PrepararProcedimiento("dbo.[Contacts.GetById]", _parameters);
                list = _conn.Query();
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
