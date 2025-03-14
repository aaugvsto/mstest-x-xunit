using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedica.Modelos.Base
{
    public abstract class Pessoa : Entidade
    {
        public Pessoa()
        {
        }

        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Idade { get; set; }
    }
}
