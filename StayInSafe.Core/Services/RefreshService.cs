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
    public class RefreshService : IRefresh, IDisposable
    {
        private bool disposedValue;
        private IConnectionDB<RefreshToken> _conn;
        DynamicParameters _parameters = new DynamicParameters();
        public RefreshService(IConnectionDB<RefreshToken> conn)
        {
            _conn = conn;
        }
        public long Create(RefreshToken refreshToken)
        {
            long id = 0;
            try
            {
                RefreshToken model = new RefreshToken();
                _parameters.Add("@p_user_json", JsonConvert.SerializeObject(refreshToken), DbType.String, ParameterDirection.Input);
                _conn.PrepararProcedimiento("dbo.[RefreshTokens.Set_Tokens]", _parameters);
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

        public void Delete(long id)
        {
            try
            {
                _parameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                _conn.PrepararProcedimiento("dbo.[RefreshTokens.Delete_Tokens]", _parameters);
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
        public RefreshToken GetByToken(string token)
        {
            RefreshToken model = null;
            try
            {
                _parameters.Add("@Token", token, DbType.String, ParameterDirection.Input);
                _conn.PrepararProcedimiento("dbo.[RefreshTokens.Get_Tokens]", _parameters);
                model = _conn.QueryFirstOrDefaultDapper();
                return model;
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
