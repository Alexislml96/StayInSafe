using Alexis.CORE.Connection.Interfaces;
using Alexis.CORE.Connection.Models;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
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
    public class ComentarioService : IComentario, IDisposable
    {
        private bool disposedValue;
        private IConnectionDB<Comentarios> _conn;
        DynamicParameters _parameters = new DynamicParameters();

        public ComentarioService(IConnectionDB<Comentarios> conn)
        {
            _conn = conn;
        }
        public long AddComment(Comentarios comentario)
        {
            long id = 0;

            try
            {
                _parameters.Add("@p_user_json", JsonConvert.SerializeObject(comentario), DbType.String, ParameterDirection.Input);
                _conn.PrepararProcedimiento("dbo.[Comentarios.AddCmentario]", _parameters);
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

        public IEnumerable<Comentarios> GetCommentSito(int id)
        {
            IEnumerable<Comentarios> list = new List<Comentarios>();

            try
            {
                _parameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                _conn.PrepararProcedimiento("dbo.[Comentarios.GetComentarios]", _parameters);
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
