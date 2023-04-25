using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLC_Var_Management_App.Classes
{
    public class RequicisionD
    {
        #region miembros
        public int id;
        public int id_requisicion;
        public Int16 id_rm;
        public Int16 id_tanq_o;
        public Int16 id_tanq_d;
        public Int16 cant;
        public bool complete;
        public decimal inclusion;
        public bool process;
        public bool Recuperado;
        public int idUsuario;
        DataOperations dp = new DataOperations();
        #endregion
        
        public RequicisionD()
        {
        }

        public bool RecuperarRegistro(int pId)
        {
            try
            {
                SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);
                conn.Open();
                string sql = @"SELECT [id]
                                     ,[id_requisicion]
                                     ,[id_rm]
                                     ,[id_tanq_o]
                                     ,[id_tanq_d]
                                     ,[cant]
                                     ,[complete]
                                     ,[inclusion]
                                     ,[process]
                               FROM [dbo].[oil_req_d]
                               where id = " + pId;

                SqlCommand cmd = new SqlCommand(sql, conn);
                //int id = Convert.ToInt32(cmd.ExecuteScalar());
                
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                     this.id = dr.GetInt32(0);
                     id_requisicion = dr.GetInt32(1);
                     id_rm = Convert.ToInt16(dr.GetInt32(2));
                     id_tanq_o = Convert.ToInt16(dr.GetInt32(3));
                     id_tanq_d = Convert.ToInt16(dr.GetInt32(4));
                     cant = Convert.ToInt16(dr.GetInt32(5));
                     complete = dr.GetBoolean(6);
                     inclusion = dr.GetDecimal(7);
                     process = dr.GetBoolean(8);
                     Recuperado = true;
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ec)
            {
                Recuperado = false;
                MessageBox.Show(ec.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Recuperado;
        }

        public int RecuperarRM_FromBin(int pIdBin)
        {
            try
            {
                SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);
                conn.Open();
                string sql = @"" + pIdBin;

                SqlCommand cmd = new SqlCommand(sql, conn);
                int id = Convert.ToInt32(cmd.ExecuteScalar());

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ec)
            {
                Recuperado = false;
                MessageBox.Show(ec.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return 0;
        }


        public bool UpdateReqComplete(int pIdReqD)
        {
            bool r = false;
            try
            {
                SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);
                conn.Open();
                string sql = @"UPDATE [dbo].[oil_req_d]
                                   SET [complete] = 1
                                      ,[process] = 0
                                 WHERE id = " + pIdReqD.ToString();

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                r = true;
            }
            catch (Exception ec)
            {
                r = false;
                MessageBox.Show(ec.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return r;
        }


        public int RecuperarFromProcess()
        {
            int id = 0;
            try
            {
                SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);
                conn.Open();
                string sql = @"SELECT top 1 [id]
                               FROM [dbo].[oil_req_d]
                               where [process] = 1
                               order by id desc";

                SqlCommand cmd = new SqlCommand(sql, conn);
                id = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            catch (Exception ec)
            {
                Recuperado = false;
                MessageBox.Show(ec.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return id;
        }

        public bool UpdateReqInProcess(int pIdReq)
        {
            bool r = false;
            try
            {
                SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);
                conn.Open();
                string sql = @"UPDATE [dbo].[oil_req_d]
                                   SET [process] = 1
                                 WHERE id = " + pIdReq;

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ec)
            {
                r = false;
                MessageBox.Show(ec.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return r;
        }
    }
}
