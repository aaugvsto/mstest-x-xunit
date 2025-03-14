using ClinicaMedica.Modelos;
using ClinicaMedica.Modelos.Enums;
using ClinicaMedica.Resources;
using ClinicaMedica.Servicos;
using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.MSTest.Test
{
    [TestClass]
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

        [TestMethod]
        [DynamicData(nameof(DadosTesteAgedamentoConsulta), DynamicDataSourceType.Method)]
        public void Testa_AgendamentoDaConsulta(DateTime dataHoraConsulta, List<SintomasEnum> sintomas)
        {
            var medico = new Medico();
            var paciente = new Paciente();
            var atendente = new Atendente();

            var consulta = _consultaServico.AgendarConsulta(medico, paciente, atendente, dataHoraConsulta, sintomas);

            Assert.IsTrue(consulta.Id != Guid.Empty);
            Assert.AreEqual(paciente.Id, consulta.IdPaciente);
            Assert.AreEqual(medico.Id, consulta.IdMedico);
            Assert.AreEqual(atendente.Id, consulta.IdAtendente);
            Assert.AreEqual(StatusConsultaEnum.Agendada, consulta.Status);
            Assert.AreEqual(dataHoraConsulta, consulta.DataHorarioConsulta);
        }

        [TestMethod]
        public void Testa_AgendamentoDeConsultaComDataRetrograda()
        {
            var sintomas = new List<SintomasEnum> { SintomasEnum.DorDeCabeca, SintomasEnum.Enjoo };
            var dataHoraConsulta = DateTime.Now.AddDays(-1);
            var ex = Assert.ThrowsException<Exception>(() => _consultaServico.AgendarConsulta(new Medico(), new Paciente(), new Atendente(), dataHoraConsulta, sintomas));

            Assert.AreEqual(Validacoes.AvisoDataRetrograda, ex.Message);
        }

        [TestMethod]
        public void Testa_AgendamentoDeConsultaSemSintomas()
        {
            var sintomas = new List<SintomasEnum>();
            var dataHoraConsulta = DateTime.Now.AddDays(1);

            var ex = Assert.ThrowsException<Exception>(() => _consultaServico.AgendarConsulta(new Medico(), new Paciente(), new Atendente(), dataHoraConsulta, sintomas));
            Assert.AreEqual(Validacoes.AvisoConsultaSemSintomas, ex.Message);
        }

        [TestMethod]
        public void Testa_AgendamentoDeConsultaSemMedico()
        {
            var sintomas = new List<SintomasEnum> { SintomasEnum.DorDeCabeca, SintomasEnum.Enjoo };
            var dataHoraConsulta = DateTime.Now.AddDays(1);
            var ex = Assert.ThrowsException<Exception>(() => _consultaServico.AgendarConsulta(null, new Paciente(), new Atendente(), dataHoraConsulta, sintomas));

            Assert.AreEqual(Validacoes.AvisoMedicoPacienteVazio, ex.Message);
        }

        [TestMethod]
        public void Testa_AgendamentoDeConsultaSemPaciente()
        {
            var sintomas = new List<SintomasEnum> { SintomasEnum.DorDeCabeca, SintomasEnum.Enjoo };
            var dataHoraConsulta = DateTime.Now.AddDays(1);
            var ex = Assert.ThrowsException<Exception>(() => _consultaServico.AgendarConsulta(new Medico(), null, new Atendente(), dataHoraConsulta, sintomas));

            Assert.AreEqual(Validacoes.AvisoMedicoPacienteVazio, ex.Message);
        }

        [TestMethod]
        public void Testa_AgendamentoDeConsultaSemAtendente()
        {
            var sintomas = new List<SintomasEnum> { SintomasEnum.DorDeCabeca, SintomasEnum.Enjoo };
            var dataHoraConsulta = DateTime.Now.AddDays(1);

            var ex = Assert.ThrowsException<Exception>(() => _consultaServico.AgendarConsulta(new Medico(), new Paciente(), null, dataHoraConsulta, sintomas));

            Assert.AreEqual(Validacoes.AvisoAtendenteVazio, ex.Message);
        }

        [TestMethod]
        public void Testa_CancelamentoDaConsulta()
        {
            var consulta = new Consulta();

            _consultaServico.CancelarConsulta(consulta);

            Assert.AreEqual(StatusConsultaEnum.Cancelada, consulta.Status);
        }

        [TestMethod]
        public void Testa_CancelamentoConsultaComStatusDiferenteDeAgendado()
        {
            var consulta = new Consulta();
            consulta.Status = StatusConsultaEnum.EmProgresso;

            var ex = Assert.ThrowsException<Exception>(() => _consultaServico.CancelarConsulta(consulta));

            Assert.AreEqual(Validacoes.AvisoCancelarConsultaComStatusDiferenteDeAgendado, ex.Message);
        }

        [TestMethod]
        public void Testa_InicioDaConsulta()
        {
            var consulta = new Consulta();

            consulta.DataHorarioConsulta = DateTime.Now;

            _consultaServico.IniciarConsulta(consulta);

            Assert.AreEqual(StatusConsultaEnum.EmProgresso, consulta.Status);
        }

        [TestMethod]
        public void Testa_InicioDaConsultaComStatusDiferenteDeAgendada()
        {
            var consulta = new Consulta();
            consulta.Status = StatusConsultaEnum.EmProgresso;

            var ex = Assert.ThrowsException<Exception>(() => _consultaServico.IniciarConsulta(consulta));
            Assert.AreEqual(Validacoes.AvisoIniciarConsultaComStatusDiferenteDeAgendado, ex.Message);
        }

        [TestMethod]
        public void Testa_FinalizacaoDaConsulta()
        {
            var consulta = new Consulta();
            consulta.Status = StatusConsultaEnum.EmProgresso;

            _consultaServico.FinalizarConsulta(consulta);

            Assert.AreEqual(StatusConsultaEnum.Finalizada, consulta.Status);
        }

        [TestMethod]
        public void Testa_FinalizacaoDaConsultaComStatusDiferenteDeEmProgresso()
        {
            var consulta = new Consulta();
            consulta.Status = StatusConsultaEnum.Agendada;

            var ex = Assert.ThrowsException<Exception>(() => _consultaServico.FinalizarConsulta(consulta));

            Assert.AreEqual(Validacoes.AvisoFinalizarConsultaComStatusDiferenteDeEmProgresso, ex.Message);
        }
    }
}
