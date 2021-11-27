using Alexis.CORE.Connection.Interfaces;
using Alexis.CORE.Connection.Models;
using Dapper;
using Newtonsoft.Json;
using StayInSafe.Core.Interfaces;
using StayInSafe.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
