using Alexis.CORE.Connection.Interfaces;
using Alexis.CORE.Connection.Models;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using StayInSafe.Core.Interfaces;
using StayInSafe.Core.Models;
using StayInSafe.Core.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayInSafe.Core.Services
{
    public class UserService : IUser, IDisposable
    {
        bool disposedValue;
        IConnectionDB<Users> _conn;
        private readonly HashTool _hashTool;
        DynamicParameters _parameters = new DynamicParameters();
        public UserService(IConnectionDB<Users> conn)
        {
            _conn = conn;
            _hashTool = new HashTool();
        }

        public long Register(Users user)
        {
            long id = 0;
            user.Pass = _hashTool.Hash(user.Pass);
            try
            {
                _parameters.Add("@p_user_json", JsonConvert.SerializeObject(user), DbType.String, ParameterDirection.Input);
                _conn.PrepararProcedimiento("dbo.[USERS.Register]", _parameters);
                id = (long)_conn.QueryFirstOrDefaultDapper(TipoDato.Numerico);
                return id;
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

        public Users GetUser(long id)
        {
            Users resp = new Users();
            try
            {
                _parameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                _conn.PrepararProcedimiento("dbo.[USERS.Get_Id]", _parameters);
                resp = _conn.QueryFirstOrDefaultDapper();
                return resp;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message);
            }
            catch (MySql.Data.MySqlClient.MySqlException mysqlEx)
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

        public bool UpdateUser(Users user)
        {
            throw new NotImplementedException();
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
