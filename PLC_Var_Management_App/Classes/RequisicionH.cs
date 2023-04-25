using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLC_Var_Management_App.Classes
{
    public class RequisicionH
    {
        public int id;
        public DateTime fecha_p;
        public int id_usuario;
        public string motivo;
        public int tipo;
        public bool Recuperado;
        DataOperations dp;
        public RequisicionH()
        {
            dp = new DataOperations();
        }

        public bool RecuperarRegistro(int pId)
        {
            try
            {
                SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);
                conn.Open();
                string sql = @"SELECT [id]
                                     ,[fecha_p]
                                     ,[id_usuario]
                                     ,[motivo]
                                     ,[tipo]
                               FROM [APMS].[dbo].[oil_req_h] 
                               where id = " + pId;

                SqlCommand cmd = new SqlCommand(sql, conn);
                int id = Convert.ToInt32(cmd.ExecuteScalar());

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    this.id = dr.GetInt32(0);
                    fecha_p = dr.GetDateTime(1);
                    id_usuario = dr.GetInt32(2);
                    motivo = dr.GetString(3);
                    tipo = dr.GetInt32(4);
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
    }
}
