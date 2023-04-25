using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace PLC_Var_Management_App.Classes
{
    class DataOperations
    {
        #region Connection Credentials

        internal string ConnectionStringCostos = @"Server=" + Globals.CTS_ServerAddress + @";
                                                   Database=" + Globals.CTS_ActiveDB + @";
                                                   User Id=" + Globals.CTS_DB_User + @";
                                                   Password=" + Globals.CTS_DB_Pass + ";";

        //        internal string ConnectionStringCostos_History = @"Server=" + Globals.CTSH_ServerAddress + @";
        //                                                           Database=" + Globals.CTSH_ActiveDB + @";
        //                                                           User Id=" + Globals.CTSH_DB_User + @";
        //                                                           Password=" + Globals.CTSH_DB_Pass + ";";

        internal string ConnectionStringConsola = @";Database=" + Globals.CMS_ActiveDB + @";
                                                    User Id=" + Globals.CMS_DB_User + @";
                                                    Password=" + Globals.CMS_DB_Pass + ";";

        internal string ConnectionStringPelletServer = @"Server=" + Globals.CMS_ServerPellet + @";
                                                       Database=" + Globals.CMS_ActiveDB + @";
                                                       User Id=" + Globals.CMS_DB_User + @";
                                                       Password=" + Globals.CMS_DB_Pass + ";";

        internal string ConnectionStringExtruderServer = @"Server=" + Globals.CMS_ServerExtruder + @";
                                                       Database=" + Globals.CMS_ActiveDB + @";
                                                       User Id=" + Globals.CMS_DB_User + @";
                                                       Password=" + Globals.CMS_DB_Pass + ";";

        //        internal string ConnectionStringODOO = @"Provider=PostgreSQL OLE DB Provider;
        //                                                 Data Source=" + Globals.odoo_ServerAddress + @";
        //                                                 location=" + Globals.odoo_ActiveDB + @";
        //                                                 User ID=" + Globals.odoo_DB_User + @";
        //                                                 password=" + Globals.odoo_DB_Pass + @";";

        internal string ConnectionStringODOO = @"Server=" + Globals.odoo_ServerAddress + @";
                                                 Port=5432;
                                                 Database=" + Globals.odoo_ActiveDB + @";
                                                 User Id=" + Globals.odoo_DB_User + @";
                                                 Password=" + Globals.odoo_DB_Pass + @";
                                                 CommandTimeout=20;";

        //       internal string ConnectionStringPRININ = @"Server=" + Globals.prinin_ServerAddress + @";
        //                                                   Database=" + Globals.prinin_ActiveDB + @";
        //                                                   User Id=" + Globals.prinin_DB_User + @";
        //                                                   Password=" + Globals.prinin_DB_Pass + ";";

        internal string ConnectionStringAPMS = @"Server=" + Globals.APMS_Server + @";
                                                       Database=" + Globals.APMS_ActiveDB + @";
                                                       User Id=" + Globals.APMS_DB_User + @";
                                                       Password=" + Globals.APMS_DB_Pass + ";";

        public string GetConnectionString()
        {
            return ConnectionStringCostos;
        }

        #endregion

        #region ACS Related Methods

        /// <summary>
        /// Retorna el numero de materias primas inactivas en una formula
        /// </summary>
        /// <param name="idFormula">El id unico de la formula a buscar</param>
        /// <returns></returns>
        public int get_inactive_rm(int idFormula)
        {
            DataTable ingredientes = new DataTable();
            int veces = 0;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id_formula", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@peso_total", SqlDbType.Int));

            cmd.Parameters["@id_formula"].Value = idFormula;
            cmd.Parameters["@peso_total"].Value = Convert.ToDouble(2500);

            ingredientes = ACS_Exec_SP_Get_Data("PP_Plan_Ordenes_Get_Ingredientes", cmd);

            foreach (DataRow row in ingredientes.Rows)
            {
                if (row["estado"].ToString() == "i")
                {
                    veces++;
                }
            }

            return veces;
        }

        public DataSet ACS_GetSelectData(string FixedQuery)
        {
            DataSet data = new DataSet();

            SqlConnection Conn = new SqlConnection(ConnectionStringCostos);
            Conn.Open();

            SqlDataAdapter adapter = new SqlDataAdapter(FixedQuery, Conn);
            adapter.Fill(data);

            Conn.Close();

            return data;
        }

        public int ACS_Exec_SP_GetID(string Procedure_Name, SqlCommand command, SqlParameter returnParameter)
        {
            Int32 ID;

            SqlConnection conn = new SqlConnection(ConnectionStringCostos);

            if (command.CommandType == CommandType.StoredProcedure)
            {
                command.Connection = conn;
                command.CommandText = Procedure_Name;
                conn.Open();
                command.ExecuteNonQuery();

                ID = Convert.ToInt32(returnParameter.Value);

                conn.Close();
            }
            else
            {
                ID = -1;
            }

            return ID;
        }

        public void ACS_Exec_SP(string Procedure_Name, SqlCommand command)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConnectionStringCostos);

                if (command.CommandType == CommandType.StoredProcedure)
                {
                    command.Connection = conn;
                    command.CommandText = Procedure_Name;
                    conn.Open();
                    command.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public DataTable ACS_Exec_SP_Get_Data(string Procedure_Name, SqlCommand command)
        {
            DataTable data = new DataTable();

            SqlConnection conn = new SqlConnection(ConnectionStringCostos);

            command.CommandText = Procedure_Name;
            command.Connection = conn;
            command.CommandType = CommandType.StoredProcedure;

            conn.Open();

            data.Load(command.ExecuteReader());

            conn.Close();

            return data;
        }

        public int ACS_InsertAndReturnID(string Command)
        {
            Int32 InsertedID;
            SqlConnection conn = new SqlConnection(ConnectionStringCostos);
            conn.Open();

            SqlCommand cmd = new SqlCommand(Command, conn);

            InsertedID = Convert.ToInt32(cmd.ExecuteScalar());

            conn.Close();

            return InsertedID;
        }

        public void ACS_DoSmallDBOperation(string Command)
        {
            SqlConnection conn = new SqlConnection(ConnectionStringCostos);
            conn.Open();

            SqlCommand cmd = new SqlCommand(Command, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        #endregion

        #region APMS Related Methods

        public DataSet APMS_GetSelectData(string FixedQuery)
        {
            DataSet data = new DataSet();

            SqlConnection Conn = new SqlConnection(ConnectionStringAPMS);
            Conn.Open();

            SqlDataAdapter adapter = new SqlDataAdapter(FixedQuery, Conn);
            adapter.Fill(data);

            Conn.Close();

            return data;
        }

        public int APMS_Exec_SP_GetID(string Procedure_Name, SqlCommand command, SqlParameter returnParameter)
        {
            Int32 ID;

            SqlConnection conn = new SqlConnection(ConnectionStringAPMS);

            if (command.CommandType == CommandType.StoredProcedure)
            {
                command.Connection = conn;
                command.CommandText = Procedure_Name;
                conn.Open();
                command.ExecuteNonQuery();

                ID = Convert.ToInt32(returnParameter.Value);

                conn.Close();
            }
            else
            {
                ID = -1;
            }

            return ID;
        }

        public void APMS_Exec_SP(string Procedure_Name, SqlCommand command)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConnectionStringAPMS);

                if (command.CommandType == CommandType.StoredProcedure)
                {
                    command.Connection = conn;
                    command.CommandText = Procedure_Name;
                    conn.Open();
                    command.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public DataTable APMS_Exec_SP_Get_Data(string Procedure_Name, SqlCommand command)
        {
            DataTable data = new DataTable();

            SqlConnection conn = new SqlConnection(ConnectionStringAPMS);

            command.CommandText = Procedure_Name;
            command.Connection = conn;
            command.CommandType = CommandType.StoredProcedure;

            conn.Open();

            data.Load(command.ExecuteReader());

            conn.Close();

            return data;
        }

        public int APMS_InsertAndReturnID(string Command)
        {
            Int32 InsertedID;
            SqlConnection conn = new SqlConnection(ConnectionStringAPMS);
            conn.Open();

            SqlCommand cmd = new SqlCommand(Command, conn);

            InsertedID = Convert.ToInt32(cmd.ExecuteScalar());

            conn.Close();

            return InsertedID;
        }

        public void APMS_DoSmallDBOperation(string Command)
        {
            SqlConnection conn = new SqlConnection(ConnectionStringAPMS);
            conn.Open();

            SqlCommand cmd = new SqlCommand(Command, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void APMS_NewLogEntry(string user, string location, string action)
        {
            string Command = @" INSERT INTO [QDAS].[dbo].[QDAS_Logs]
		                                    ([date],[time],[user],[location],[action])
                                     VALUES (SYSDATETIME(),SYSDATETIME()," + user + ",'" + location + "','" + action + "')";

            SqlConnection conn = new SqlConnection(ConnectionStringCostos);
            conn.Open();
            SqlCommand cmd = new SqlCommand(Command, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        #endregion


        #region WinCC_6_2 (Old Production) Related Methods

        public void WinCC_Exec_SP(string Procedure_Name, SqlCommand command)
        {
            SqlConnection conn = new SqlConnection(ConnectionStringPelletServer);

            if (command.CommandType == CommandType.StoredProcedure)
            {
                command.Connection = conn;
                command.CommandText = Procedure_Name;
                conn.Open();
                command.ExecuteNonQuery();

                conn.Close();
            }
        }

        public DataSet GetSelectData_SetServer(string FixedQuery, string SelectedServer)
        {
            DataSet data = new DataSet();

            SqlConnection Conn = new SqlConnection(string.Format("Server={0}{1}", SelectedServer, ConnectionStringConsola));
            Conn.Open();

            SqlDataAdapter adapter = new SqlDataAdapter(FixedQuery, Conn);
            adapter.Fill(data);

            Conn.Close();

            return data;
        }

        #endregion

        #region Custom Methods
        //Method Implemented and Improved in the SecOps Class
        //public void SendEmailAlert(DataSet Receivers, int ColumnNumber, string Subject, string Body)
        //{
        //    MailMessage message = new MailMessage();
        //    SmtpClient smtp = new SmtpClient();

        //    message.From = new MailAddress("noreply@aquafeedhn.com", "Quality Data Analysis System v1.0");

        //    foreach (DataRow row in Receivers.Tables[0].Rows)
        //    {
        //        message.To.Add(new MailAddress(row[ColumnNumber].ToString()));
        //    }

        //    message.Subject = Subject;
        //    message.Body = Body;

        //    smtp.EnableSsl = false;
        //    smtp.Port = 80;
        //    smtp.Host = "smtpout.secureserver.net";
        //    smtp.UseDefaultCredentials = true;
        //    smtp.Credentials = new NetworkCredential("scans@aquafeedhn.com", "A1dd1cf460&");
        //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

        //    smtp.Send(message);
        //}

        #endregion

        #region Nutreco Related Methods

        //public DataSet PRININ_GetSelectData(string FixedQuery)
        //{
        //    DataSet data = new DataSet();

        //    SqlConnection Conn = new SqlConnection(ConnectionStringPRININ);
        //    Conn.Open();

        //    SqlDataAdapter adapter = new SqlDataAdapter(FixedQuery, Conn);
        //    adapter.Fill(data);

        //    Conn.Close();

        //    return data;
        //}

        #endregion

        #region Not Implemented Methods

        //public DataSet H_GetSelectData(string FixedQuery)
        //{
        //    DataSet data = new DataSet();

        //    SqlConnection Conn = new SqlConnection(ConnectionStringCostos_History);
        //    Conn.Open();

        //    SqlDataAdapter adapter = new SqlDataAdapter(FixedQuery, Conn);
        //    adapter.Fill(data);

        //    Conn.Close();

        //    return data;
        //}

        //public int H_InsertAndReturnID(string Command)
        //{
        //    Int32 InsertedID;
        //    SqlConnection conn = new SqlConnection(ConnectionStringCostos_History);
        //    conn.Open();

        //    SqlCommand cmd = new SqlCommand(Command, conn);

        //    InsertedID = Convert.ToInt32(cmd.ExecuteScalar());

        //    conn.Close();

        //    return InsertedID;
        //}

        //public void H_DoSmallDBOperation(string Command)
        //{
        //    SqlConnection conn = new SqlConnection(ConnectionStringCostos_History);
        //    conn.Open();

        //    SqlCommand cmd = new SqlCommand(Command, conn);
        //    cmd.ExecuteNonQuery();

        //    conn.Close();
        //}

        public void NewLogEntry(string user, string location, string action)
        {
            string Command = @" INSERT INTO [QDAS].[dbo].[QDAS_Logs]
		                                    ([date],[time],[user],[location],[action])
                                     VALUES (SYSDATETIME(),SYSDATETIME()," + user + ",'" + location + "','" + action + "')";

            SqlConnection conn = new SqlConnection(ConnectionStringCostos);
            conn.Open();
            SqlCommand cmd = new SqlCommand(Command, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        internal int Exec_SP_GetID()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
