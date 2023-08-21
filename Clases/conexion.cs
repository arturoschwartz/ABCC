using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCC.Clases
{
    internal class conexion
    {
        public string conn()
        {
            string miconexion = ("Data Source=DESKTOP-UDV0HQQ;Initial Catalog=abcc;Integrated Security=True");
            return miconexion;
        }
    }
}
