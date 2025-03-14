using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedica.Modelos.Base
{
    public abstract class Funcionario : Pessoa
    {
        public Funcionario()
        {
        }

        public string Matricula { get; set; }
        public string Setor { get; set; }
    }
}
