using ClinicaMedica.Modelos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedica.Modelos
{
    public class Paciente : Pessoa
    {
        public Paciente()
        {
        }

        public string PlanoDeSaude { get; set; }
    }
}
