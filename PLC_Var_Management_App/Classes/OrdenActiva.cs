using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLC_Var_Management_App.Classes
{
    public class OrdenActiva
    {
        DataOperations dp;
        public OrdenActiva()
        {
            dp = new DataOperations();
        }

        public Int64 Id;
        public Int64 Id_Order;
        public string OrderNumber;
        public Int64 Lote;
        public Double batch;
        public bool Recuperado;
        public int nBatch;
        public int nBatchPlan;
        public int MixNum;
        public Double BatchPlankg;

        public bool GuardarBatchLecitina()
        {
            bool r = false;
            SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);

            try
            {
                string sql = @"INSERT INTO [dbo].[OP_Batch_Intake_Data_RM]
                                                ([order_id]
                                                ,[order_mix_id]
                                                ,[material_id]
                                                ,[bin_id]
                                                ,[bin_codes]
                                                ,[lot_id]
                                                ,[intake_type]
                                                ,[batch_no]
                                                ,[intake_plan]
                                                ,[intake_real]
                                                ,[mix]
                                                ,[logged_user]
                                                ,[batch_start])
                                            VALUES
                                                (" + Id_Order.ToString() + ","+
                                                 Id.ToString() + ",33,46," +
                                                "'OIL4',0,'RM'," +
                                                nBatch.ToString() + "," +
                                                BatchPlankg.ToString() + "," +
                                                batch.ToString() +"," +
                                                MixNum.ToString() + ",' ', getdate())";

                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                r = true;
                conn.Close();
            }
            catch (Exception ec)
            {
                conn.Close();
                MessageBox.Show(ec.Message);
            }
            return r;
        }

        public bool RecuperarRegistro()
        {
            bool r = false;
            SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);
                
            try
            {
                string sql = @"SELECT mix.[id]
                                      ,[order_id]
                                      ,[mix_fullCode]
                                      ,mix.[fp_lot_number]
	                                  ,coalesce((SELECT [plan_kg_batch]
		                                FROM [APMS].[dbo].[OP_Production_Orders_Structure]
		                                where order_id = mix.order_id and
		                                      rm_id = 33),0) as batch,
                                       mix.real_batch,
                                       mix.plan_batch,
                                       mix.mix_num,
                                       mix.plan_kg
                                FROM [APMS].[dbo].[OP_Production_Orders_Main_Mix] mix join 
                                    [APMS].[dbo].OP_Production_Orders_Main mm on
	                                mix.order_id = mm.id
                                where mix.status = 70 and
		                            mix.mix_num = 1";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    r = true;
                    Id = dr.GetInt64(0);
                    Id_Order = dr.GetInt64(1);
                    OrderNumber = dr.GetString(2);
                    Lote = dr.GetInt64(3);
                    batch = Convert.ToDouble(dr.GetDecimal(4));
                    nBatch = dr.GetInt32(5);
                    nBatchPlan = dr.GetInt32(6);
                    MixNum = dr.GetInt32(7);
                    //BatchPlankg = Convert.ToDouble(dr.GetDecimal(8));
                    BatchPlankg = batch;
                    Recuperado = true;
                }
                else
                {
                    Id = 0;
                    Id_Order = 0;
                    OrderNumber = "";
                    Lote = 0;
                    batch = 0;
                    Recuperado = false;
                }
                conn.Close();
            }
            catch (Exception ec)
            {
                conn.Close();
                MessageBox.Show(ec.Message);
            }
            return r;
        }

        public bool RecuperarRegistroSegundoMix()
        {
            bool r = false;
            SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);

            try
            {
                string sql = @"SELECT mix.[id]
                                      ,[order_id]
                                      ,[mix_fullCode]
                                      ,mix.[fp_lot_number]
	                                  ,coalesce((SELECT [plan_kg_batch]
		                                FROM [APMS].[dbo].[OP_Production_Orders_Structure]
		                                where order_id = mix.order_id and
		                                      rm_id = 33),0) as batch,
                                       mix.real_batch,
                                       mix.plan_batch,
                                       mix.mix_num,
                                       mix.plan_kg
                                FROM [APMS].[dbo].[OP_Production_Orders_Main_Mix] mix join 
                                    [APMS].[dbo].OP_Production_Orders_Main mm on
	                                mix.order_id = mm.id
                                where mix.status = 70 and
		                            mix.mix_num = 2";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    r = true;
                    Id = dr.GetInt64(0);
                    Id_Order = dr.GetInt64(1);
                    OrderNumber = dr.GetString(2);
                    Lote = dr.GetInt64(3);
                    batch = Convert.ToDouble(dr.GetDecimal(4));
                    nBatch = dr.GetInt32(5);
                    nBatchPlan = dr.GetInt32(6);
                    MixNum = dr.GetInt32(7);
                    BatchPlankg = Convert.ToDouble(dr.GetDecimal(8));
                    Recuperado = true;
                }
                else
                {
                    Id = 0;
                    Id_Order = 0;
                    OrderNumber = "";
                    Lote = 0;
                    batch = 0;
                    Recuperado = false;
                }
                conn.Close();
            }
            catch (Exception ec)
            {
                conn.Close();
                MessageBox.Show(ec.Message);
            }
            return r;
        }

        public bool GuardarBatchFylaxMezcla()
        {
            bool r = false;
            SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);

            try
            {
                string sql = @"INSERT INTO [dbo].[OP_Batch_Intake_Data_RM]
                                                ([order_id]
                                                ,[order_mix_id]
                                                ,[material_id]
                                                ,[bin_id]
                                                ,[bin_codes]
                                                ,[lot_id]
                                                ,[intake_type]
                                                ,[batch_no]
                                                ,[intake_plan]
                                                ,[intake_real]
                                                ,[mix]
                                                ,[logged_user]
                                                ,[batch_start])
                                            VALUES
                                                (" + Id_Order.ToString() + "," +
                                                 Id.ToString() + ",39,92," +
                                                "'Fylax1',0,'RM'," +
                                                nBatch.ToString() + "," +
                                                BatchPlankg.ToString() + "," +
                                                batch.ToString() + "," +
                                                MixNum.ToString() + ",' ', getdate())";

                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                r = true;
                conn.Close();
            }
            catch (Exception ec)
            {
                conn.Close();
                MessageBox.Show(ec.Message);
            }
            return r;
        }


        public double RecuperarKGPlanFylax()
        {
            double r = 0;
            SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);

            try
            {
                string sql = @"SELECT coalesce((SELECT [plan_kg_batch]
		                                FROM [APMS].[dbo].[OP_Production_Orders_Structure]
		                                where order_id = mix.order_id and
		                                        rm_id = 39),0) as batch
                                FROM [APMS].[dbo].[OP_Production_Orders_Main_Mix] mix join 
                                    [APMS].[dbo].OP_Production_Orders_Main mm on
	                                mix.order_id = mm.id
                                where mix.status = 70 and
	                                mix.mix_num = 1";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    r = Convert.ToDouble(dr.GetDecimal(0));
                }
                conn.Close();
            }
            catch (Exception ec)
            {
                conn.Close();
                MessageBox.Show(ec.Message);
            }
            return r;
        }

        public bool GuardarBatchFylax()
        {
            bool r = false;
            SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);

            try
            {
                string sql = @"INSERT INTO [dbo].[RM_FYLAX_CONSUMO]
                                                                   ([peso])
                                                             VALUES
                                                                   (" + batch.ToString() + ")";

                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                r = true;
                conn.Close();
            }
            catch (Exception ec)
            {
                conn.Close();
                MessageBox.Show(ec.Message);
            }
            return r;
        }


        public decimal GetPorcentajeBatchFylax()
        {
            decimal val = 0;
            SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);

            try
            {
                string sql = @"EXEC [dbo].[recupera_porcentaje_fylax_liquido_vs_agua]";

                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                val = Convert.ToDecimal(cmd.ExecuteScalar());
                conn.Close();
            }
            catch (Exception ec)
            {
                conn.Close();
                MessageBox.Show(ec.Message);
            }
            return val;
        }









    }


}
