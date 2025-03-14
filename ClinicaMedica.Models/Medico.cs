using ClinicaMedica.Modelos.Base;

namespace ClinicaMedica.Modelos
{
    public class Medico : Funcionario
    {
        public Medico() 
        { 
        }

        public string CRM { get; set; }
        public string Especialidade { get; set; }

    }
}
