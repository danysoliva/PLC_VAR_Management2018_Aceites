using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using S7.Net;
using PLC_Var_Management_App.Classes;
using System.Data.SqlClient;

namespace PLC_Var_Management_App
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        #region Developer Defined Variables

        DataOperations dp = new DataOperations();
        DateTime log_date;
        int IdReqActual;
        OrdenActiva OrdenA = new OrdenActiva();

        string sDoneEnvio1 = "DB33.DBX22.3";//Indica que hay un batch y registrar 
        string sDoneEnvio2 = "DB33.DBX22.1";//Indica que la transferencia finalizo
        string sTanqueOrigen = "DB33.DBW24";//Indica el id del tanque de origen
        string sTanqueDestino = "DB33.DBW26";//Indica el id del tanque de Destino
        string sProductoTQExt1 = "DB33.DBW28";//Indica el id Producto asignado
        string sProductoTQExt2 = "DB33.DBW30";//Indica el id Producto asignado
        string sProductoTQExt3 = "DB33.DBW32";//Indica el id Producto asignado
        string sCantidadSolicitadaTQExt1 = "DB33.DBD34";//cantidad a dosificar
        string sCantidadSolicitadaTQExt2 = "DB33.DBD38";//cantidad a dosificar
        string sCantidadSolicitadaTQExt3 = "DB33.DBD42";//cantidad a dosificar
        string sDosificadoRealTQext1 = "DB33.DBD46";//cantidad total dosificada
        string sDosificadoRealTQext2 = "DB33.DBD50";//cantidad total dosificada
        string sDosificadoRealTQext3 = "DB33.DBD54";//cantidad total dosificada
        string sDosificadoPorBatch = "DB33.DBD58";//Cantidad pesada en tq bascula
        string sSeleccionDestino = "DB33.DBW62";//Donde escribo el destino donde se hace la transf. de aceite
        string sOrdenDosificacion = "DB33.DBX22.2";//Bloqueo para iniciar la transferencia
        string sUsuarioPLC = "DB33.DBB";//Variable con el nombre de usuario

        string sProductoTQ_Int1 = "DB33.DBW86";//Indica el id Producto asignado
        string sProductoTQ_Int2 = "DB33.DBW88";//Indica el id Producto asignado
        string sProductoTQ_Int3 = "DB33.DBW90";//Indica el id Producto asignado

        string sDoneEnvio3 = "DB33.DBX116.0";//indica que hay que guardar un batch de lecitina
        string sSetPesoLecitina = "DB33.DBD118";//Indica el peso del batch a cargar desde plc
        string sPesoPesoRealLecitina = "DB33.DBD122";//Indica el peso del batch capturada en la bascula con lecitina

        string sResetPantallaTraslados = "DB43.DBX0.0";//Bit que indica que se hizo reset en la pantalla

        int idOrden1 = 0;
        int idOrden2 = 0;
        int idOrden3 = 0;

        int LeerOrden = 0;

        #region PLC1200

        Plc plc1200;
        static CpuType plc1200_CPUType = CpuType.S71200;
        static string plc1200_IPAddress = "192.168.10.74";
        static Int16 plc1200_Rack = 0;
        static Int16 plc1200_Slot = 1;

        #endregion

        #endregion

        #region Developer Defined Methods
        private void Connect_PLC()
        {
            try
            {
                
                if (!plc1200.IsConnected)
                {
                    plc1200.Open();
                    txt_Status1200.Text = "Connection Stablished";
                    txt_Status1200.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Ubicacion error: Connect_PLC(); Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Connect_PLC();
            }
        }
        private void Disconect_PLC()
        {
            try
            {
               
                if (plc1200.IsConnected)
                {
                    plc1200.Close();
                    txt_Status1200.Text = "Disconnected";
                    txt_Status1200.ForeColor = System.Drawing.Color.OrangeRed;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Check_PLC_Availability()
        {
            try
            {
                
                if (plc1200.IsConnected)
                {
                    txt_Status1200.Text = "Available";
                    txt_Status1200.ForeColor = System.Drawing.Color.ForestGreen;
                }
                else
                {
                    txt_Status1200.Text = "Unavailable";
                    txt_Status1200.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Check_PLC_Connectivity()
        {
            try
            {
                
                if (plc1200.IsConnected)
                {
                    txt_Status1200.Text = "Connection Stablished";
                    txt_Status1200.ForeColor = System.Drawing.Color.ForestGreen;
                }
                else
                {
                    txt_Status1200.Text = "Disconnected";
                    txt_Status1200.ForeColor = System.Drawing.Color.OrangeRed;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Load_Log() 
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void set_log_entry(string writen_plc, string writen_db, string writen_var, string writen_value, string process_index, string var_description, string custom_data_1, string custom_data_2, string custom_data_3, string custom_data_4, string custom_data_5) 
        {
            try
            {
                #region Parametros_SP_Entrada
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@writen_plc", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@writen_db", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@writen_var", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@writen_value", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@process_index", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@var_description", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@custom_data_1", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@custom_data_2", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@custom_data_3", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@custom_data_4", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@custom_data_5", SqlDbType.VarChar));

                cmd.Parameters["@writen_plc"].Value = writen_plc;
                cmd.Parameters["@writen_db"].Value = writen_db;
                cmd.Parameters["@writen_var"].Value = writen_var;
                cmd.Parameters["@writen_value"].Value = writen_value;
                cmd.Parameters["@process_index"].Value = process_index;
                cmd.Parameters["@var_description"].Value = var_description;
                cmd.Parameters["@custom_data_1"].Value = custom_data_1;
                cmd.Parameters["@custom_data_2"].Value = custom_data_2;
                cmd.Parameters["@custom_data_3"].Value = custom_data_3;
                cmd.Parameters["@custom_data_4"].Value = custom_data_4;
                cmd.Parameters["@custom_data_5"].Value = custom_data_5;
                #endregion

                dp.APMS_Exec_SP("SYS_Var_Write_Log_Entry", cmd);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Constructors

        public Form1()
        {
            InitializeComponent();
        }

        #endregion

        #region Form Events
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                plc1200 = new Plc(plc1200_CPUType, plc1200_IPAddress, plc1200_Rack, plc1200_Slot);
                txt_Address1200.Text = plc1200_IPAddress;
                txt_rack1200.Text = plc1200_Rack.ToString();
                txt_Slot1200.Text = plc1200_Slot.ToString();

                Check_PLC_Availability();

                //txt_ServiceStatus.Caption = "Service Stopped";
                //txt_ServiceStatus.Appearance.ForeColor = System.Drawing.Color.Red;
                //btn_Stop_Service.Enabled = false;

                log_date = Convert.ToDateTime(dp.APMS_GetSelectData(@"SELECT SYSDATETIME()").Tables[0].Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                //SEND E-MAIL Message on Service Failure.
                //MessageBox.Show("Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_Start_Service_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                MainTimer.Enabled = true;
                MainTimer.Start();

                //VarReaderMonitor.Enabled = true;
                //VarReaderMonitor.Start();

                //Connect_PLC();
                //txt_ServiceStatus.Caption = "Service Started";
                //txt_ServiceStatus.Appearance.ForeColor = System.Drawing.Color.Green;

                //btn_Start_Service.Enabled = false;
                //btn_Stop_Service.Enabled = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            //Process Danys
            //bool v =  (bool)plc1200.Read("DB438.DBX116.0");
            //double moisture_readx = ((uint)plc1200.Read("DB33.DBD58")).ConvertToFloat();
            //double value = Convert.ToDouble(row["plc_var_value_to_set"].ToString());
            //plc1200.Write(sProductoTQExt1, 31);// row["plc_var_full_name"].ToString(), value);
        }
        private void btn_Stop_Service_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult r = MessageBox.Show("Are you sure you want close this process?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (r == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();
            }

            //try
            //{
            //    MainTimer.Enabled = false;
            //    MainTimer.Stop();

            //    VarReaderMonitor.Enabled = false;
            //    VarReaderMonitor.Stop();

            //    Disconect_PLC();
            //    txt_ServiceStatus.Caption = "Service Stopped";
            //    txt_ServiceStatus.Appearance.ForeColor = System.Drawing.Color.Red;

            //    btn_Start_Service.Enabled = true;
            //    btn_Stop_Service.Enabled = false;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
        private void btn_Test1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                short num = 1;
                plc1200.Write("DB438.DBW112.0", num);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_Test2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                short num = 2;
                plc1200.Write("DB438.DBW112.0", num);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public Int16 GetMaterialIdBin(int pIdBin)
        {
            Int16 idRM = 0;
            try
            {
                string sql = @"SELECT [set_rm_id]
                               FROM [APMS].[dbo].[MD_Bins]
                               where id = " + pIdBin;
                SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                idRM = Convert.ToInt16(cmd.ExecuteScalar());
                conn.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return idRM;
        }


        public void ResetRequisicion()
        {
            try
            {
                string sql = @"UPDATE [dbo].[oil_req_d]
                                    SET [process] = 0
                               WHERE [process] = 1 and 
                                     [complete] = 0";
                SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteScalar();
                conn.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void MainTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                //if (!plc1200.IsConnected)
                {
                    if (!plc1200.IsConnected)
                        Connect_PLC();

                    
                    if (Convert.ToBoolean(plc1200.Read(sResetPantallaTraslados)))
                    {
                        //Update en false requisicion en proceso
                        ResetRequisicion();

                        //Borrar el valor de la variable
                        plc1200.Write(sResetPantallaTraslados, 0);
                    }
                    else
                    {
                        //ResetRequisicion();
                        #region Escribir orden dosificacion

                        //Escribir los materiales de cada tanque
                        plc1200.Write(sProductoTQExt1, GetMaterialIdBin(Convert.ToInt16(88)));//92
                        plc1200.Write(sProductoTQExt2, GetMaterialIdBin(Convert.ToInt16(90)));
                        plc1200.Write(sProductoTQExt3, GetMaterialIdBin(Convert.ToInt16(91)));

                        plc1200.Write(sProductoTQ_Int1, GetMaterialIdBin(Convert.ToInt16(43)));
                        plc1200.Write(sProductoTQ_Int2, GetMaterialIdBin(Convert.ToInt16(44)));
                        plc1200.Write(sProductoTQ_Int3, GetMaterialIdBin(Convert.ToInt16(45)));


                        //Escribir los nombre de Bines para pantalla de alimentacion manual.
                        PLCOPS fn = new PLCOPS();
                        //Escribir los nombre en pantalla de alimentacion
                        string db = "DB507.DBX";//Nombre de la DB que es fijo, solo cambia el bit del offset
                        int Multiplicador = 0;//Valor acumulado para formar el DB en el PLC

                        int bin_idx = 4;//id de la base de datos
                        //Los id estan ordenados de forma ascendente de la fd1 = 4, fd2 = 5 ..... hasta fd12 = 15 
                        //Incrementaremos de 1 en 1 

                        //while (Multiplicador <= 132)//Es la coleccion proporcionada de PLC. Mantiene una secuencia de 12 char en el db por lo que concatenamos el nombre del db sumando +12
                        //{
                        //    string name_db = db + Multiplicador;//creamos nuestro nombre de DB
                            

                        //    //Get Nambe value
                        //    string name = fn.GetRM_ShortName_APMS_from_Bin_id(bin_idx);//Obtenemos de la base de datos el nombre corto del material en tolva.

                        //    if (name.Length >= 11)
                        //    {
                        //        name = name.Substring(0, 10);
                        //    }
                        //    //plc1200.Write(name_db, name);//Escribimos el string en el PLC319
                        //    plc1200.Write( DataType.DataBlock,507,Multiplicador, name);//Escribimos el string en el PLC319
                        //    bin_idx += 1;
                        //    Multiplicador += 12;//Incrementamos las 12 unidades para el sigueinte db
                        //}

                        //if (OrdenA.RecuperarRegistro())
                        //{
                        //    //Proceso de lectura y captura de la lecitina
                        //    plc1200.Write(DataType.DataBlock, 33, 118, OrdenA.batch);

                        //    //escritura para el 300 ver la cantidad dosificada.
                        //    plc1200.Write(DataType.DataBlock, 22, 0, OrdenA.batch);

                        //    double fylaxSetPoint = OrdenA.RecuperarKGPlanFylax();
                        //    plc1200.Write(DataType.DataBlock, 71, 0, fylaxSetPoint);
                            

                        //    //bool done1Lecitina = Convert.ToBoolean(plc1200.Read("DB33.DBX116.0"));

                        //    //if (done1Lecitina)
                        //    //{
                        //    //    double TotalBatch = ((uint)plc1200.Read("DB33.DBD122")).ConvertToFloat();
                        //    //    OrdenA.batch = TotalBatch;
                        //    //    OrdenA.GuardarBatchLecitina();
                        //    //    //set done1 en 0
                        //    //    plc1200.Write("DB33.DBX116.0", false);
                        //    //}
                        //}

                        
                        //if (OrdenA.RecuperarRegistroSegundoMix())
                        //{
                        //    ////DONE FYLAX****
                        //    //bool done1Fylax = Convert.ToBoolean(plc1200.Read("DB33.DBX126.0"));
                        //    //if (done1Fylax)
                        //    //{
                        //    //    double TotalBatch = ((uint)plc1200.Read("DB33.DBD128")).ConvertToFloat();
                        //    //    //double por = Convert.ToDouble(OrdenA.GetPorcentajeBatchFylax());
                        //    //    //OrdenA.batch = (TotalBatch * (por / 100));
                        //    //    OrdenA.batch = TotalBatch;
                        //    //    OrdenA.GuardarBatchFylax();

                        //    //    //set done1 en 0
                        //    //    plc1200.Write("DB33.DBX126.0", false);
                        //    //}

                        //    //DONE MEZCLA****
                        //    bool done1Mezcla = Convert.ToBoolean(plc1200.Read("DB33.DBX126.1"));
                        //    if (done1Mezcla)
                        //    {
                        //        double TotalBatch = ((uint)plc1200.Read("DB33.DBD132")).ConvertToFloat();
                        //        double por = Convert.ToDouble(OrdenA.GetPorcentajeBatchFylax());
                        //        OrdenA.batch = (TotalBatch * (por / 100));
                        //        //OrdenA.batch = TotalBatch;
                        //        OrdenA.GuardarBatchFylaxMezcla();

                        //        //set done1 en 0
                        //        plc1200.Write("DB33.DBX126.1", false);
                        //    }
                        //}

                        if (ReqInProcess() > 0)
                        {
                            //trabajar la orden que esta en proceso
                            bool done1 = Convert.ToBoolean(plc1200.Read(sDoneEnvio1));
                            bool done2 = Convert.ToBoolean(plc1200.Read(sDoneEnvio2));

                            RequicisionD reqx = new RequicisionD();

                            //Guardar la transferencia
                            if (done1)
                            {
                                RequisicionH reqH1 = new RequisicionH();
                                if (reqx.RecuperarRegistro(reqx.RecuperarFromProcess()))
                                {
                                    reqH1.RecuperarRegistro(reqx.id_requisicion);
                                    IdReqActual = reqx.id;
                                    double TotalBatch = ((uint)plc1200.Read(sDosificadoPorBatch)).ConvertToFloat();
                                    switch (reqx.id_tanq_o)//set de la cantidad a transferir al tanque configurado
                                    {
                                        //Escribir la cantidad solicitada
                                        case 88:
                                            TotalBatch = ((uint)plc1200.Read(sDosificadoPorBatch)).ConvertToFloat();
                                            break;
                                        case 90:
                                            TotalBatch = ((uint)plc1200.Read(sDosificadoPorBatch)).ConvertToFloat();
                                            break;
                                        case 91:
                                            TotalBatch = ((uint)plc1200.Read(sDosificadoPorBatch)).ConvertToFloat();
                                            break;
                                    }

                                    string UserPlc = plc1200.Read(DataType.DataBlock, 33, 64, VarType.String, 20).ToString();
                                    GuardarTransferencia(reqx.id_rm,
                                                         reqx.id_tanq_o,
                                                         reqx.id_tanq_d,
                                                         TotalBatch,
                                                         reqx.cant,
                                                         reqx.idUsuario,
                                                         UserPlc,
                                                         0,
                                                         IdReqActual);

                                    //set done1 en 0
                                    plc1200.Write(sDoneEnvio1, false);
                                    //Desactivar la orden de dosificacion
                                    //plc1200.Write(sOrdenDosificacion, false);
                                }
                            }

                            //Confirmacion que se efectuo el traslado de forma completa
                            if (done2)
                            {
                                if (!reqx.Recuperado)
                                    reqx.RecuperarRegistro(reqx.RecuperarFromProcess());
                                reqx.UpdateReqComplete(reqx.id);

                                //set done2 en 0
                                plc1200.Write(sDoneEnvio2, 0);

                                //Desactivar la orden de dosificacion
                                plc1200.Write(sOrdenDosificacion, 0);

                                //Reset de las cantidades
                                plc1200.Write(DataType.DataBlock, 33, 34, 0);
                                plc1200.Write(DataType.DataBlock, 33, 38, 0);
                                plc1200.Write(DataType.DataBlock, 33, 42, 0);

                                //Reset de las rutas
                                plc1200.Write(sTanqueOrigen, 0);//reset tanque de origen
                                plc1200.Write(sTanqueDestino, 0);//reset tanque destino
                                plc1200.Write(sSeleccionDestino, 0);//reset de seleccion destino
                            }
                        }
                        else
                        {
                            if (ReqPendientes())
                            {
                                //trabajar la requisicion pendiente
                                this.IdReqActual = GetSiguiente();
                                RequicisionD req = new RequicisionD();
                                if (req.RecuperarRegistro(IdReqActual))
                                {
                                    //Parar lectura de confirmacion
                                    //TimeConfirmacion.Stop();

                                    //Escribir la transferencia en el plc
                                    //Escribir enteros
                                    object valor_db = plc1200.Read("DB33.DBW24");
                                    

                                    plc1200.Write(DataType.DataBlock, 33, 24, req.id_tanq_o);
                                    plc1200.Write(DataType.DataBlock, 33, 26, req.id_tanq_d);


                                    //Escribir un doble
                                    float var = (float)(req.cant);
                                    float varCero = 0;
                                    //var = 1000;

                                    switch (req.id_tanq_o)//set de la cantidad a transferir al tanque configurado
                                    {
                                        //Escribir la cantidad solicitada
                                        case 88:
                                            //sCantidadSolicitadaTQExt1
                                            plc1200.Write(DataType.DataBlock, 33, 34, var);
                                            plc1200.Write(DataType.DataBlock, 33, 38, varCero);
                                            plc1200.Write(DataType.DataBlock, 33, 42, varCero);
                                            break;
                                        case 90:
                                            //sCantidadSolicitadaTQExt2
                                            plc1200.Write(DataType.DataBlock, 33, 34, varCero);
                                            plc1200.Write(DataType.DataBlock, 33, 38, var);
                                            plc1200.Write(DataType.DataBlock, 33, 42, varCero);
                                            break;
                                        case 91:
                                            //sCantidadSolicitadaTQExt3
                                            plc1200.Write(DataType.DataBlock, 33, 34, varCero);
                                            plc1200.Write(DataType.DataBlock, 33, 38, varCero);
                                            plc1200.Write(DataType.DataBlock, 33, 42, var);
                                           
                                            break;
                                    }

                                    //Definir el tanque destino
                                    plc1200.Write(sSeleccionDestino, req.id_tanq_d);//reset de la solicitud

                                    //Activamos la orden de dosificar
                                    plc1200.Write(sOrdenDosificacion, 1);//activamos la dosificacion


                                    //plc1200.Write(sSeleccionDestino, 43);
                                    //int x = Convert.ToInt32(plc1200.Read(sSeleccionDestino));
                                    ////Leer un doble
                                    //double PesoBatch = ((uint)plc1200.Read(sDosificadoPorBatch)).ConvertToFloat();
                                    ////Leer una cadena
                                    //string a = plc1200.Read(DataType.DataBlock, 33, 64, VarType.String, 20).ToString();


                                    //actualizar el estado a En Proceso de la requisicion
                                    req.UpdateReqInProcess(IdReqActual);

                                    //activar el timer que espera la confirmación
                                    //TimeConfirmacion.Start();

                                    plc1200.Write(sDoneEnvio1, 0);//reset de la solicitud
                                    plc1200.Write(sDoneEnvio2, 0);//reset de la solicitud
                                }
                            }
                        }
                        #endregion
                    }

                }//End If available
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void EscribirCantPorBatch()
        {
            try
            {
                //Buscar la cantidad por batch

            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message);
            }
        }


        public bool GuardarTransferencia(int pRM_id, int pTanqO, int pTanqD, double pCantidadE, double pCantidadS, int idUserS, string sUser, int pFalla, int pIdreq)
        {
            bool r = false;
            string scantidadE = pCantidadE.ToString();
            string scantidadS = pCantidadS.ToString();
            try
            {
                SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);
                conn.Open();
                string sql = @"INSERT INTO [dbo].[oil_transfers]
                                                               ([fecha_hora]
                                                               ,[id_rm]
                                                               ,[id_tanq_o]
                                                               ,[id_tanq_d]
                                                               ,[cant]
                                                               ,[cant_s]
                                                               ,[id_user_solicita]
                                                               ,[user_envia]
                                                               ,[tipo_falla]
                                                               ,[complete]
                                                               ,[id_req_d])
                                                         VALUES
                                                               (GETDATE()," + 
                                                                pRM_id.ToString() + ","+
                                                                pTanqO.ToString() + ","+
                                                                pTanqD.ToString() + ","+
                                                                pCantidadE + ","+
                                                                pCantidadS + ","+
                                                                idUserS.ToString() + ",'"+
                                                                sUser + "',"+
                                                                pFalla.ToString() + ", 1," +
                                                                pIdreq.ToString()+ ")";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                r = true;
            }
            catch (Exception ec)
            {
                //MessageBox.Show(ec.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return r;
        }


        public bool ReqPendientes()
        {
            bool r = false;
            try
            {
                SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);
                conn.Open();
                string sql = @"SELECT COUNT([id])
                               FROM [dbo].[oil_req_d]
                               where complete = 0";
                SqlCommand cmd = new SqlCommand(sql, conn);
                int total = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                if (total > 0)
                    r = true;
            }
            catch (Exception ec)
            {
                //MessageBox.Show(ec.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return r;
        }

        public int GetSiguiente()
        {
            int r = 0;
            try
            {
                SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);
                conn.Open();
                string sql = @" SELECT top 1 id
                                FROM [APMS].[dbo].[oil_req_d] dd
                                where dd.complete = 0
                                order by dd.id asc";
                SqlCommand cmd = new SqlCommand(sql, conn);
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                if (id > 0)
                    r = id;
            }
            catch (Exception ec)
            {
                //MessageBox.Show(ec.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return r;
        }

        public int ReqInProcess()
        {
            int r = 0;
            try
            {
                SqlConnection conn = new SqlConnection(dp.ConnectionStringAPMS);
                conn.Open();
                string sql = @"SELECT [id]
                               FROM [dbo].[oil_req_d]
                               where process = 1 and
                                     complete = 0";
                SqlCommand cmd = new SqlCommand(sql, conn);
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                if (id > 0)
                    r = id;
            }
            catch (Exception ec)
            {
                //MessageBox.Show(ec.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return r;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Disconect_PLC();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Detalle del Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_hide_to_tray_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AppNotify.Visible = true;
            this.Hide();
        }

        private void AppNotify_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            AppNotify.Visible = false;
        }

        private void VarReaderMonitor_Tick(object sender, EventArgs e)
        {
            try
            {
                if (plc1200.IsConnected) 
                {
                    if (!plc1200.IsConnected)
                        Connect_PLC();
   
                }
            }
            catch
            {
                
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Escribir enteros
            plc1200.Write(sProductoTQExt1, 33);
            plc1200.Write(sSeleccionDestino, 43);
            int x = Convert.ToInt32(plc1200.Read(sSeleccionDestino));

            //Escribir un doble
            Double var = 1000.2;
            plc1200.Write(DataType.DataBlock, 33,34, var);
            
            //Leer un doble
            double PesoBatch = ((uint)plc1200.Read(sDosificadoPorBatch)).ConvertToFloat();

            //Leer una cadena
            string a = plc1200.Read(DataType.DataBlock,33,64, VarType.String,20).ToString();
            //MessageBox.Show(a);
        }

        private void TimeConfirmacion_Tick(object sender, EventArgs e)
        {

        }
    }

        #endregion
}
