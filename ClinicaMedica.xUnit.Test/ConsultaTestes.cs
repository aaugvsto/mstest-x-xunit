using ClinicaMedica.Modelos;
using ClinicaMedica.Modelos.Enums;
using ClinicaMedica.Resources;
using ClinicaMedica.Servicos;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ClinicaMedica.XUnit.Test
{
    public sealed class ConsultaTestes
    {
        private readonly ConsultaServico _consultaServico;
        public ConsultaTestes()
        {
            _consultaServico = new ConsultaServico();
        }

        public static IEnumerable<object[]> DadosTesteAgedamentoConsulta()
        {
            yield return new object[]
            {
                    DateTime.Now.AddDays(1),
                    new List<SintomasEnum> { SintomasEnum.DorDeCabeca, SintomasEnum.Enjoo }
            };

            yield return new object[]
            {
                    DateTime.Now,
                    new List<SintomasEnum> { SintomasEnum.DorDeCabeca, SintomasEnum.Enjoo, SintomasEnum.Febre }
            };

            yield return new object[]
            {
                    DateTime.Now.AddDays(2),
                    new List<SintomasEnum> { SintomasEnum.DorDeCabeca, SintomasEnum.Enjoo, SintomasEnum.Febre }
            };
        }

        [Theory]
        [MemberData(nameof(DadosTesteAgedamentoConsulta))]
        public void Testa_AgendamentoDaConsulta(DateTime dataHoraConsulta, List<SintomasEnum> sintomas)
        {
            var medico = new Medico();
            var paciente = new Paciente();
            var atendente = new Atendente();

            var consulta = _consultaServico.AgendarConsulta(medico, paciente, atendente, dataHoraConsulta, sintomas);

            Assert.True(consulta.Id != Guid.Empty);
            Assert.Equal(consulta.IdPaciente, paciente.Id);
            Assert.Equal(consulta.IdMedico, medico.Id);
            Assert.Equal(consulta.IdAtendente, atendente.Id);
            Assert.Equal(StatusConsultaEnum.Agendada, consulta.Status);
            Assert.Equal(consulta.DataHorarioConsulta, dataHoraConsulta);
        }

        [Fact]
        public void Testa_AgendamentoDeConsultaComDataRetrograda()
        {
            var sintomas = new List<SintomasEnum> { SintomasEnum.DorDeCabeca, SintomasEnum.Enjoo };
            var dataHoraConsulta = DateTime.Now.AddDays(-1);
            var ex = Assert.Throws<Exception>(() => _consultaServico.AgendarConsulta(new Medico(), new Paciente(), new Atendente(), dataHoraConsulta, sintomas));

            Assert.Equal(ex.Message, Validacoes.AvisoDataRetrograda);
        }

        [Fact]
        public void Testa_AgendamentoDeConsultaSemSintomas()
        {
            var sintomas = new List<SintomasEnum>();
            var dataHoraConsulta = DateTime.Now.AddDays(1);

            var ex = Assert.Throws<Exception>(() => _consultaServico.AgendarConsulta(new Medico(), new Paciente(), new Atendente(), dataHoraConsulta, sintomas));
            Assert.Equal(ex.Message, Validacoes.AvisoConsultaSemSintomas);
        }

        [Fact]
        public void Testa_AgendamentoDeConsultaSemMedico()
        {
            var sintomas = new List<SintomasEnum> { SintomasEnum.DorDeCabeca, SintomasEnum.Enjoo };
            var dataHoraConsulta = DateTime.Now.AddDays(1);
            var ex = Assert.Throws<Exception>(() => _consultaServico.AgendarConsulta(null, new Paciente(), new Atendente(), dataHoraConsulta, sintomas));

            Assert.Equal(ex.Message, Validacoes.AvisoMedicoPacienteVazio);
        }

        [Fact]
        public void Testa_AgendamentoDeConsultaSemPaciente()
        {
            var sintomas = new List<SintomasEnum> { SintomasEnum.DorDeCabeca, SintomasEnum.Enjoo };
            var dataHoraConsulta = DateTime.Now.AddDays(1);
            var ex = Assert.Throws<Exception>(() => _consultaServico.AgendarConsulta(new Medico(), null, new Atendente(), dataHoraConsulta, sintomas));

            Assert.Equal(ex.Message, Validacoes.AvisoMedicoPacienteVazio);
        }

        [Fact]
        public void Testa_AgendamentoDeConsultaSemAtendente()
        {
            var sintomas = new List<SintomasEnum> { SintomasEnum.DorDeCabeca, SintomasEnum.Enjoo };
            var dataHoraConsulta = DateTime.Now.AddDays(1);

            var ex = Assert.Throws<Exception>(() => _consultaServico.AgendarConsulta(new Medico(), new Paciente(), null, dataHoraConsulta, sintomas));
            
            Assert.Equal(ex.Message, Validacoes.AvisoAtendenteVazio);
        }

        [Fact]
        public void Testa_CancelamentoDaConsulta()
        {
            var consulta = new Consulta();

            _consultaServico.CancelarConsulta(consulta);

            Assert.Equal(StatusConsultaEnum.Cancelada, consulta.Status);
        }

        [Fact]
        public void Testa_CancelamentoConsultaComStatusDiferenteDeAgendado()
        {
            var consulta = new Consulta();
            consulta.Status = StatusConsultaEnum.EmProgresso;

            var ex = Assert.Throws<Exception>(() => _consultaServico.CancelarConsulta(consulta));

            Assert.Equal(ex.Message, Validacoes.AvisoCancelarConsultaComStatusDiferenteDeAgendado);
        }

        [Fact]
        public void Testa_InicioDaConsulta()
        {
            var consulta = new Consulta();

            consulta.DataHorarioConsulta = DateTime.Now;

            _consultaServico.IniciarConsulta(consulta);

            Assert.Equal(StatusConsultaEnum.EmProgresso, consulta.Status);
        }

        [Fact]
        public void Testa_InicioDaConsultaComStatusDiferenteDeAgendada()
        {
            var consulta = new Consulta();
            consulta.Status = StatusConsultaEnum.EmProgresso;

            var ex = Assert.Throws<Exception>(() => _consultaServico.IniciarConsulta(consulta));
            Assert.Equal(ex.Message, Validacoes.AvisoIniciarConsultaComStatusDiferenteDeAgendado);
        }

        [Fact]
        public void Testa_FinalizacaoDaConsulta()
        {
            var consulta = new Consulta();
            consulta.Status = StatusConsultaEnum.EmProgresso;

            _consultaServico.FinalizarConsulta(consulta);

            Assert.Equal(StatusConsultaEnum.Finalizada, consulta.Status);
        }

        [Fact]
        public void Testa_FinalizacaoDaConsultaComStatusDiferenteDeEmProgresso()
        {
            var consulta = new Consulta();
            consulta.Status = StatusConsultaEnum.Agendada;

            var ex = Assert.Throws<Exception>(() => _consultaServico.FinalizarConsulta(consulta));

            Assert.Equal(ex.Message, Validacoes.AvisoFinalizarConsultaComStatusDiferenteDeEmProgresso);
        }
    }
}