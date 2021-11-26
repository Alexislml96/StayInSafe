using Alexis.CORE.Connection.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;
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
    public class PasswordService : IPassword, IDisposable
    {
        private bool disposedValue;
        private IConnectionDB<LoginModel> _conn;
        DynamicParameters _parameters = new DynamicParameters();
        public PasswordService(IConnectionDB<LoginModel> conn)
        {
            _conn = conn;
        }
        public LoginModel CheckUser(LoginModel login)
        {
            try
            {
                LoginModel model = new LoginModel();
                _parameters.Add("@Email", login.Email, DbType.String, ParameterDirection.Input);
                _conn.PrepararProcedimiento("dbo.[Users.CheckUser]", _parameters);
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
