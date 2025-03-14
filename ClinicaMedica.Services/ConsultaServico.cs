using ClinicaMedica.Modelos;
using ClinicaMedica.Modelos.Enums;
using ClinicaMedica.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedica.Servicos
{
   public class ConsultaServico
   {
        public ConsultaServico()
        {
        }

        public Consulta AgendarConsulta(Medico medico, Paciente paciente, Atendente atendente, DateTime dataHorarioConsulta, List<SintomasEnum> sintomasPaciente)
        {
            if(medico == null || paciente == null)
                throw new Exception(Validacoes.AvisoMedicoPacienteVazio);

            if (dataHorarioConsulta.Date < DateTime.Now.Date)
                throw new Exception(Validacoes.AvisoDataRetrograda);

            if(sintomasPaciente == null || !sintomasPaciente.Any())
                throw new Exception(Validacoes.AvisoConsultaSemSintomas);

            if(atendente == null)
                throw new Exception(Validacoes.AvisoAtendenteVazio);

            var consulta = new Consulta();

            consulta.Id = Guid.NewGuid();
            consulta.DataHorarioConsulta = dataHorarioConsulta;
            consulta.Status = StatusConsultaEnum.Agendada;
            consulta.IdPaciente = paciente.Id;
            consulta.IdMedico = medico.Id;
            consulta.IdAtendente = atendente.Id;
            consulta.SintomasPaciente = sintomasPaciente;

            return consulta;
        }

        public void CancelarConsulta(Consulta consulta)
        {
            if (consulta.Status != StatusConsultaEnum.Agendada)
                throw new Exception(Validacoes.AvisoCancelarConsultaComStatusDiferenteDeAgendado);

            consulta.Status = StatusConsultaEnum.Cancelada;
        }

        public void IniciarConsulta(Consulta consulta)
        {
            if(consulta.DataHorarioConsulta.Date != DateTime.Now.Date)
            {
                throw new Exception(Validacoes.AvisoIniciarConsultaComStatusDiferenteDeAgendado);
            }

            consulta.Status = StatusConsultaEnum.EmProgresso;
        }

        public void FinalizarConsulta(Consulta consulta)
        {
            if(consulta.Status != StatusConsultaEnum.EmProgresso)
            {
                throw new Exception(Validacoes.AvisoFinalizarConsultaComStatusDiferenteDeEmProgresso);
            }

            consulta.Status = StatusConsultaEnum.Finalizada;
            consulta.DataConsultaFinalizada = DateTime.Now;
        }


    }
}
