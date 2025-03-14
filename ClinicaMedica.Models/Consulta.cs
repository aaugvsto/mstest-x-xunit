using ClinicaMedica.Modelos.Base;
using ClinicaMedica.Modelos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedica.Modelos
{
    public class Consulta : Entidade
    {
        public Consulta()
        {
            this.SintomasPaciente = new List<SintomasEnum>();
            this.Status = StatusConsultaEnum.Agendada;
        }

        public DateTime DataHorarioConsulta { get; set; }
        public Guid IdPaciente { get; set; }
        public Guid IdMedico { get; set; }
        public Guid IdAtendente { get; set; }
        public List<SintomasEnum> SintomasPaciente { get; set; }
        public DateTime DataConsultaFinalizada { get; set; }
        public StatusConsultaEnum Status {  get; set; } 
        public string DiagnosticoMedico { get; set; }
    }
}
