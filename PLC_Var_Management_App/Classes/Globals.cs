using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_Var_Management_App.Classes
{
    class Globals
    {
        #region Credenciales Producción
        //WINCC
        public static string CMS_ServerPellet = "10.50.11.24";
        public static string CMS_ServerExtruder = "10.50.11.23";
        public static string CMS_DB_User = "bkadmin";
        public static string CMS_DB_Pass = "AquaF33dHN2014";
        public static string CMS_ActiveDB = "process_data";

        //ACS (Costos)
        public static string CTS_ServerAddress = "AQFSVR003";
        public static string CTS_ServerName = "Servidor Productivo";
        public static string CTS_ActiveDB = "ACS";
        public static string CTS_DB_User = "sa";
        public static string CTS_DB_Pass = "AquaF33dHN2014";

        //APMS (Aquafeed Pocess Management System)
        public static string APMS_Server = @"AQFSVR008\AQFSVR008";
        public static string APMS_DB_User = "mrt";//para liberar el usuario sa
        public static string APMS_DB_Pass = "AquaF33dHN2017";
        public static string APMS_ActiveDB = "APMS";

        //ODOO
        public static string odoo_ServerAddress = "AQFSVR003";
        public static string odoo_ServerName = "Servidor Productivo";
        public static string odoo_ActiveDB = "aquafeed";

        public static string odoo_DB_User = "aquafeed";
        public static string odoo_DB_Pass = "Aqua3820";

        ////Promix -- TEST ---
        public static string prinin_ServerAddress = "9DR5P32";
        public static string prinin_ServerName = "Development Server";
        public static string prinin_ActiveDB = "PRININ";

        public static string prinin_DB_User = "sa";
        public static string prinin_DB_Pass = "Promix1620";
        #endregion

        #region Credenciales Desarrollo
        ////WINCC 
        //public static string CMS_ServerPellet = @"JFTDF12\SQLEXPRESS";
        //public static string CMS_ServerExtruder = @"JFTDF12\SQLEXPRESS";
        //public static string CMS_DB_User = "sa";
        //public static string CMS_DB_Pass = "AquaFeed2016";
        //public static string CMS_ActiveDB = "process_data";

        ////APMS (Aquafeed Pocess Management System)
        //public static string APMS_Server = @"localhost";
        //public static string APMS_DB_User = "sa";
        //public static string APMS_DB_Pass = "AquaFeed2016";
        //public static string APMS_ActiveDB = "APMS";

        ////ACS (Aquafeed Costing System)
        //// public static string CTS_ServerAddress = @"JFTDF12\SQLEXPRESS";       //PC - DR
        ////public static string CTS_ServerAddress  = @"HR57FB2-PC\SQLEXPRESS";    //PC - JV
        //public static string CTS_ServerAddress = @"localhost";                  //PC LocalHost
        //public static string CTS_ServerName = "Servidor de Desarrollo";
        //public static string CTS_ActiveDB = "ACS";
        //public static string CTS_DB_User = "sa";
        //public static string CTS_DB_Pass = "AquaFeed2016";

        ////Odoo
        ////public static string odoo_ServerAddress = "odoo-pruebas-pc"; 
        //public static string odoo_ServerAddress = "10.50.11.126";               //PC-Virtual (JV)
        ////public static string odoo_ServerAddress = "192.168.0.14";               //PC-Virtual (Laptop)
        //public static string odoo_ServerName = "Servidor de Desarrollo";
        //public static string odoo_ActiveDB = "pruebas";
        //public static string odoo_DB_User = "aquafeed"; //"aquafeed";
        //public static string odoo_DB_Pass = "Aqua3820";
        #endregion

        #region Credenciales Carlos_Cordova
        ////WINCC
        //public static string CMS_ServerPellet = @"JFTDF12\SQLEXPRESS";
        //public static string CMS_ServerExtruder = @"JFTDF12\SQLEXPRESS";
        //public static string CMS_DB_User = "sa";
        //public static string CMS_DB_Pass = "AquaFeed2016";
        //public static string CMS_ActiveDB = "process_data";

        ////APMS (Aquafeed Pocess Management System)
        ////public static string APMS_Server      = @"192.168.0.125";
        ////public static string APMS_Server = @"CV-PC\SQLEXPRESS";
        //public static string APMS_Server = @"CARLOS-PC\SQLEXPRESS";
        //public static string APMS_DB_User = "sa";
        //public static string APMS_DB_Pass = "ingenia";
        //public static string APMS_ActiveDB = "APMS";

        ////ACS (Aquafeed Costing System)
        //public static string CTS_ServerAddress = @"localhost";                  //PC LocalHost
        //public static string CTS_ServerName = "Servidor de Desarrollo";
        //public static string CTS_ActiveDB = "***";
        //public static string CTS_DB_User = "***";
        //public static string CTS_DB_Pass = "***";

        ////Odoo
        //public static string odoo_ServerAddress = "10.50.11.126";               //PC-Virtual (JV)
        //public static string odoo_ServerName = "Servidor de Desarrollo";
        //public static string odoo_ActiveDB = "***";
        //public static string odoo_DB_User = "***"; //"aquafeed";
        //public static string odoo_DB_Pass = "***";
        #endregion
    }
}
