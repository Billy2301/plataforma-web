
using Entity.ClinicaModels;
using Microsoft.EntityFrameworkCore;

namespace DA.ClinicaContext;

public partial class CPALClinicaContext : DbContext
{
    public CPALClinicaContext(DbContextOptions<CPALClinicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<TriageCotejo> TriageCotejos { get; set; }

    public virtual DbSet<TriageCotejoExt> TriageCotejoExts { get; set; }

    public virtual DbSet<TriageOnline> TriageOnlines { get; set; }

    public virtual DbSet<PagoCitaDetalle> PagoCitaDetalles { get; set; }

    public virtual DbSet<PagoCita> PagoCitas { get; set; }

    public virtual DbSet<UvPagoCitasSearch> UvPagoCitasSearches { get; set; }

    public virtual DbSet<UvPagoTriajeSearch> UvPagoTriajeSearches { get; set; }

    public virtual DbSet<UvPersonalEvaluacionSearch> UvPersonalEvaluacionSearches { get; set; }

    public virtual DbSet<PersonalHorario> PersonalHorarios { get; set; }

    public virtual DbSet<PagosTriaje> PagosTriajes { get; set; }

    public virtual DbSet<UvPacienteTriajeSearch> UvPacienteTriajeSearches { get; set; }

    public virtual DbSet<Triage> Triages { get; set; }

    public virtual DbSet<UvPagoCitaDetalleSearch> UvPagoCitaDetalleSearches { get; set; }

    public virtual DbSet<UvEvaluacionCitaSearch> UvEvaluacionCitaSearches { get; set; }

    public virtual DbSet<EvaluacionCitum> EvaluacionCita { get; set; }

    public virtual DbSet<UvEspecilidadesPublica> UvEspecilidadesPublicas { get; set; }

    public virtual DbSet<UvEvaluacionesPublica> UvEvaluacionesPublicas { get; set; }

    public virtual DbSet<UvCitasPublica> UvCitasPublicas { get; set; }

    public virtual DbSet<UvHistoriaDiagnosticoBusqueda2> UvHistoriaDiagnosticoBusqueda { get; set; }

    public virtual DbSet<DetalleReservaTratamiento> DetalleReservaTratamientos { get; set; }

    public virtual DbSet<ReservaTratamiento> ReservaTratamientos { get; set; }

    public virtual DbSet<UvReservaTratamientoSearch> UvReservaTratamientoSearches { get; set; }

    public virtual DbSet<UvReservaDiagnosticoSearch> UvReservaDiagnosticoSearches { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.NumeroHistoriaClinica).HasName("Pacientes_PK");

            entity.ToTable(tb => tb.HasTrigger("Tr_pacientes_ud"));

            entity.HasIndex(e => e.SedeId, "paciente_idx_sede");

            entity.Property(e => e.NumeroHistoriaClinica)
                .ValueGeneratedNever()
                .HasColumnName("numeroHistoriaClinica");
            entity.Property(e => e.ActualizadoPor)
                .IsRequired()
                .HasMaxLength(101)
                .HasColumnName("actualizadoPor");
            entity.Property(e => e.ApellidoMaterno)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("apellidoMaterno");
            entity.Property(e => e.ApellidoPaterno)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("apellidoPaterno");
            entity.Property(e => e.ApoderadoNombre)
                .HasMaxLength(100)
                .HasColumnName("apoderadoNombre");
            entity.Property(e => e.CelularMadre)
                .HasMaxLength(20)
                .HasColumnName("celularMadre");
            entity.Property(e => e.CelularPadre)
                .HasMaxLength(20)
                .HasColumnName("celularPadre");
            entity.Property(e => e.CentroEducativo)
                .HasMaxLength(100)
                .HasColumnName("centroEducativo");
            entity.Property(e => e.CentroEducativoEmail)
                .HasMaxLength(100)
                .HasColumnName("centroEducativoEmail");
            entity.Property(e => e.CentroEducativoTelefono)
                .HasMaxLength(50)
                .HasColumnName("centroEducativoTelefono");
            entity.Property(e => e.CentroTrabajoMadre)
                .HasMaxLength(100)
                .HasColumnName("centroTrabajoMadre");
            entity.Property(e => e.CentroTrabajoPadre)
                .HasMaxLength(100)
                .HasColumnName("centroTrabajoPadre");
            entity.Property(e => e.CiaSeguro1)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ciaSeguro1");
            entity.Property(e => e.CiaSeguro2)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ciaSeguro2");
            entity.Property(e => e.Clinicoacademico).HasColumnName("clinicoacademico");
            entity.Property(e => e.CodigoColegio).HasColumnName("codigoColegio");
            entity.Property(e => e.CodigoDepartamento).HasColumnName("codigoDepartamento");
            entity.Property(e => e.CodigoDistrito).HasColumnName("codigoDistrito");
            entity.Property(e => e.CodigoPais).HasColumnName("codigoPais");
            entity.Property(e => e.CodigoProvincia).HasColumnName("codigoProvincia");
            entity.Property(e => e.CompDatosEnviadoEndDate).HasColumnType("datetime");
            entity.Property(e => e.CompDatosEnviadoEstado).HasMaxLength(10);
            entity.Property(e => e.CompDatosEnviadoFecha).HasColumnType("datetime");
            entity.Property(e => e.CompDatosEnviadoMailTo).HasMaxLength(150);
            entity.Property(e => e.CompDatosEnviadoPor).HasMaxLength(50);
            entity.Property(e => e.CompDatosRespuestaFecha).HasColumnType("datetime");
            entity.Property(e => e.Convenio)
                .HasMaxLength(100)
                .HasColumnName("convenio");
            entity.Property(e => e.Coordinarcoleg).HasColumnName("coordinarcoleg");
            entity.Property(e => e.DeletedDate)
                .HasColumnType("datetime")
                .HasColumnName("deletedDate");
            entity.Property(e => e.Derivado)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("derivado");
            entity.Property(e => e.DerivadoPor)
                .HasMaxLength(100)
                .HasColumnName("derivadoPor");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .HasColumnName("direccion");
            entity.Property(e => e.Direccion2)
                .HasMaxLength(200)
                .HasColumnName("direccion2");
            entity.Property(e => e.DniTitularSeguro)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dniTitularSeguro");
            entity.Property(e => e.DniTitularSeguro2)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dniTitularSeguro2");
            entity.Property(e => e.EmaiMadre)
                .HasMaxLength(100)
                .HasColumnName("emaiMadre");
            entity.Property(e => e.EmailContacto)
                .HasMaxLength(100)
                .HasColumnName("emailContacto");
            entity.Property(e => e.EmailPadre)
                .HasMaxLength(100)
                .HasColumnName("emailPadre");
            entity.Property(e => e.EnviarCodigoDepartamento).HasColumnName("enviarCodigoDepartamento");
            entity.Property(e => e.EnviarCodigoDistrito).HasColumnName("enviarCodigoDistrito");
            entity.Property(e => e.EnviarCodigoPais).HasColumnName("enviarCodigoPais");
            entity.Property(e => e.EnviarCodigoProvincia).HasColumnName("enviarCodigoProvincia");
            entity.Property(e => e.EnviarDireccion)
                .HasMaxLength(200)
                .HasColumnName("enviarDireccion");
            entity.Property(e => e.EnviarLugar).HasColumnName("enviarLugar");
            entity.Property(e => e.FechaIngresoTratamiento)
                .HasColumnType("datetime")
                .HasColumnName("fechaIngresoTratamiento");
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("smalldatetime")
                .HasColumnName("fechaNacimiento");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("smalldatetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.FechaUltimoReingreso)
                .HasColumnType("datetime")
                .HasColumnName("fechaUltimoReingreso");
            entity.Property(e => e.Filmar).HasColumnName("filmar");
            entity.Property(e => e.FotoPaciente)
                .HasMaxLength(200)
                .HasColumnName("fotoPaciente");
            entity.Property(e => e.FotoPaciente2)
                .HasMaxLength(100)
                .HasColumnName("fotoPaciente2");
            entity.Property(e => e.FotoPaciente3)
                .HasMaxLength(100)
                .HasColumnName("fotoPaciente3");
            entity.Property(e => e.Fotografiar).HasColumnName("fotografiar");
            entity.Property(e => e.GradoEstudios)
                .HasMaxLength(50)
                .HasColumnName("gradoEstudios");
            entity.Property(e => e.HistoriaNotas).HasColumnName("historiaNotas");
            entity.Property(e => e.IndicaCasoEmergencia)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("indicaCasoEmergencia");
            entity.Property(e => e.InformeIntegralDiagnostico)
                .HasMaxLength(100)
                .HasColumnName("informeIntegralDiagnostico");
            entity.Property(e => e.LugarEntreHermanos).HasColumnName("lugarEntreHermanos");
            entity.Property(e => e.MotivoConsulta).HasColumnName("motivoConsulta");
            entity.Property(e => e.NombreMadre)
                .HasMaxLength(101)
                .HasColumnName("nombreMadre");
            entity.Property(e => e.NombrePadre)
                .HasMaxLength(101)
                .HasColumnName("nombrePadre");
            entity.Property(e => e.Nombres)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombres");
            entity.Property(e => e.NumeroHijosMujeres).HasColumnName("numeroHijosMujeres");
            entity.Property(e => e.NumeroHijosVarones).HasColumnName("numeroHijosVarones");
            entity.Property(e => e.Obsclinacad)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("obsclinacad");
            entity.Property(e => e.Obscoordinarcoleg)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("obscoordinarcoleg");
            entity.Property(e => e.Obsfilmar)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("obsfilmar");
            entity.Property(e => e.Obsfotografiar)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("obsfotografiar");
            entity.Property(e => e.OcupacionMadre)
                .HasMaxLength(100)
                .HasColumnName("ocupacionMadre");
            entity.Property(e => e.OcupacionPadre)
                .HasMaxLength(100)
                .HasColumnName("ocupacionPadre");
            entity.Property(e => e.Otros).HasColumnName("otros");
            entity.Property(e => e.PacienteDni)
                .HasMaxLength(50)
                .HasColumnName("pacienteDNI");
            entity.Property(e => e.PacienteViveCon)
                .HasMaxLength(100)
                .HasColumnName("pacienteViveCon");
            entity.Property(e => e.Pamarillas)
                .HasDefaultValue(false)
                .HasColumnName("pamarillas");
            entity.Property(e => e.Prensa)
                .HasDefaultValue(false)
                .HasColumnName("prensa");
            entity.Property(e => e.ProfesorEmail)
                .HasMaxLength(200)
                .HasColumnName("profesorEmail");
            entity.Property(e => e.ProfesorNombre)
                .HasMaxLength(100)
                .HasColumnName("profesorNombre");
            entity.Property(e => e.ProfesorTelefono)
                .HasMaxLength(100)
                .HasColumnName("profesorTelefono");
            entity.Property(e => e.Responsable).HasColumnName("responsable");
            entity.Property(e => e.Revista)
                .HasDefaultValue(false)
                .HasColumnName("revista");
            entity.Property(e => e.SedeId)
                .HasDefaultValue(1)
                .HasColumnName("sedeID");
            entity.Property(e => e.SeguroEssalud).HasColumnName("seguroEssalud");
            entity.Property(e => e.SeguroOtros).HasColumnName("seguroOtros");
            entity.Property(e => e.SeguroParticular).HasColumnName("seguroParticular");
            entity.Property(e => e.SeguroSis).HasColumnName("seguroSis");
            entity.Property(e => e.Seguroescolar).HasColumnName("seguroescolar");
            entity.Property(e => e.Sexo).HasColumnName("sexo");
            entity.Property(e => e.SobreDirigido).HasColumnName("sobreDirigido");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.TelefonoEmergencia)
                .HasMaxLength(20)
                .HasColumnName("telefonoEmergencia");
            entity.Property(e => e.TelefonoMadre)
                .HasMaxLength(20)
                .HasColumnName("telefonoMadre");
            entity.Property(e => e.TelefonoPadre)
                .HasMaxLength(20)
                .HasColumnName("telefonoPadre");
            entity.Property(e => e.Titularseguro).HasColumnName("titularseguro");
            entity.Property(e => e.Titularseguro2).HasColumnName("titularseguro2");
            entity.Property(e => e.TratCostoTotalConDesc)
                .HasColumnType("money")
                .HasColumnName("tratCostoTotalConDesc");
            entity.Property(e => e.TratTotalPagado)
                .HasColumnType("money")
                .HasColumnName("tratTotalPagado");
            entity.Property(e => e.TratTotalPagadoMetodoEst)
                .HasColumnType("money")
                .HasColumnName("tratTotalPagadoMetodoEst");
            entity.Property(e => e.TratamientosAnteriores).HasColumnName("tratamientosAnteriores");
            entity.Property(e => e.TriageNo).HasColumnName("triageNo");
            entity.Property(e => e.UltimaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("ultimaActualizacion");
            entity.Property(e => e.Vigencia1)
                .HasColumnType("smalldatetime")
                .HasColumnName("vigencia1");
            entity.Property(e => e.Vigencia2)
                .HasColumnType("smalldatetime")
                .HasColumnName("vigencia2");
        });

        modelBuilder.Entity<TriageCotejo>(entity =>
        {
            entity.ToTable("TriageCotejo");

            entity.Property(e => e.TriageCotejoId).HasColumnName("triageCotejoID");
            entity.Property(e => e.ActualizadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("actualizadoFecha");
            entity.Property(e => e.ActualizadoPor)
                .HasMaxLength(50)
                .HasColumnName("actualizadoPor");
            entity.Property(e => e.BrindaFuncionObjeto).HasColumnName("brindaFuncionObjeto");
            entity.Property(e => e.BrindaFuncionObjetoObs)
                .HasMaxLength(200)
                .HasColumnName("brindaFuncionObjetoObs");
            entity.Property(e => e.CogeObjetos).HasColumnName("cogeObjetos");
            entity.Property(e => e.CogeObjetosObs)
                .HasMaxLength(200)
                .HasColumnName("cogeObjetosObs");
            entity.Property(e => e.ContactVisual).HasColumnName("contactVisual");
            entity.Property(e => e.ContactVisualObs)
                .HasMaxLength(200)
                .HasColumnName("contactVisualObs");
            entity.Property(e => e.CotejoEstado)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("cotejoEstado");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.DirigeMiradaObjeto).HasColumnName("dirigeMiradaObjeto");
            entity.Property(e => e.DirigeMiradaObjetoObs)
                .HasMaxLength(200)
                .HasColumnName("dirigeMiradaObjetoObs");
            entity.Property(e => e.DisgustaTocar).HasColumnName("disgustaTocar");
            entity.Property(e => e.DisgustaTocarObs)
                .HasMaxLength(200)
                .HasColumnName("disgustaTocarObs");
            entity.Property(e => e.EnviadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("enviadoFecha");
            entity.Property(e => e.EnviadoPor)
                .HasMaxLength(50)
                .HasColumnName("enviadoPor");
            entity.Property(e => e.HablaTerceraPersona).HasColumnName("hablaTerceraPersona");
            entity.Property(e => e.HablaTerceraPersonaObs)
                .HasMaxLength(200)
                .HasColumnName("hablaTerceraPersonaObs");
            entity.Property(e => e.InteresInusual).HasColumnName("interesInusual");
            entity.Property(e => e.InteresInusualObs)
                .HasMaxLength(200)
                .HasColumnName("interesInusualObs");
            entity.Property(e => e.JuegaConPersonas).HasColumnName("juegaConPersonas");
            entity.Property(e => e.JuegaConPersonasObd)
                .HasMaxLength(200)
                .HasColumnName("juegaConPersonasObd");
            entity.Property(e => e.LlamaAtencion).HasColumnName("llamaAtencion");
            entity.Property(e => e.LlamaAtencionObs)
                .HasMaxLength(200)
                .HasColumnName("llamaAtencionObs");
            entity.Property(e => e.LlevaObjetosBoca).HasColumnName("llevaObjetosBoca");
            entity.Property(e => e.LlevaObjetosBocaObs)
                .HasMaxLength(200)
                .HasColumnName("llevaObjetosBocaObs");
            entity.Property(e => e.MolestanRuidos).HasColumnName("molestanRuidos");
            entity.Property(e => e.MolestanRuidosObs)
                .HasMaxLength(200)
                .HasColumnName("molestanRuidosObs");
            entity.Property(e => e.MuestraObjetos).HasColumnName("muestraObjetos");
            entity.Property(e => e.MuestraObjetosObs)
                .HasMaxLength(200)
                .HasColumnName("muestraObjetosObs");
            entity.Property(e => e.PalabrasEmite).HasColumnName("palabrasEmite");
            entity.Property(e => e.PalabrasEmiteObs)
                .HasMaxLength(200)
                .HasColumnName("palabrasEmiteObs");
            entity.Property(e => e.PedirAlgoInteres).HasColumnName("pedirAlgoInteres");
            entity.Property(e => e.PedirAlgoInteresObs)
                .HasMaxLength(200)
                .HasColumnName("pedirAlgoInteresObs");
            entity.Property(e => e.PuedeJuegoDos).HasColumnName("puedeJuegoDos");
            entity.Property(e => e.PuedeJuegoDosObs)
                .HasMaxLength(200)
                .HasColumnName("puedeJuegoDosObs");
            entity.Property(e => e.RealizarMoviRepetido).HasColumnName("realizarMoviRepetido");
            entity.Property(e => e.RealizarMoviRepetidoObs)
                .HasMaxLength(200)
                .HasColumnName("realizarMoviRepetidoObs");
            entity.Property(e => e.RehusaComer).HasColumnName("rehusaComer");
            entity.Property(e => e.RehusaComerObs)
                .HasMaxLength(200)
                .HasColumnName("rehusaComerObs");
            entity.Property(e => e.RepiteDice).HasColumnName("repiteDice");
            entity.Property(e => e.RepiteDiceObs)
                .HasMaxLength(200)
                .HasColumnName("repiteDiceObs");
            entity.Property(e => e.RepiteFrases).HasColumnName("repiteFrases");
            entity.Property(e => e.RepiteFrasesObs)
                .HasMaxLength(200)
                .HasColumnName("repiteFrasesObs");
            entity.Property(e => e.RespondeLlamado).HasColumnName("respondeLlamado");
            entity.Property(e => e.RespondeLlamadoObs)
                .HasMaxLength(200)
                .HasColumnName("respondeLlamadoObs");
            entity.Property(e => e.RespuestaFecha)
                .HasColumnType("datetime")
                .HasColumnName("respuestaFecha");
            entity.Property(e => e.RespuestaPor)
                .HasMaxLength(50)
                .HasColumnName("respuestaPor");
            entity.Property(e => e.SeDejaEntender).HasColumnName("seDejaEntender");
            entity.Property(e => e.SeDejaEntenderObs)
                .HasMaxLength(200)
                .HasColumnName("seDejaEntenderObs");
            entity.Property(e => e.SonrieRespuesta).HasColumnName("sonrieRespuesta");
            entity.Property(e => e.SonrieRespuestaOnb)
                .HasMaxLength(200)
                .HasColumnName("sonrieRespuestaOnb");
            entity.Property(e => e.TriageOnlineId).HasColumnName("triageOnlineID");
            entity.Property(e => e.UtilizaLenguaje).HasColumnName("utilizaLenguaje");
            entity.Property(e => e.UtilizaLenguajeObs)
                .HasMaxLength(200)
                .HasColumnName("utilizaLenguajeObs");

            entity.HasOne(d => d.TriageOnline).WithMany(p => p.TriageCotejos)
                .HasForeignKey(d => d.TriageOnlineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TriageCotejo_TriageOnline");
        });

        modelBuilder.Entity<TriageCotejoExt>(entity =>
        {
            entity.ToTable("TriageCotejoExt");

            entity.Property(e => e.TriageCotejoExtId).HasColumnName("triageCotejoExtID");
            entity.Property(e => e.ActualizadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("actualizadoFecha");
            entity.Property(e => e.ActualizadoPor)
                .HasMaxLength(50)
                .HasColumnName("actualizadoPor");
            entity.Property(e => e.Aprend01).HasColumnName("aprend01");
            entity.Property(e => e.Aprend02).HasColumnName("aprend02");
            entity.Property(e => e.Aprend03).HasColumnName("aprend03");
            entity.Property(e => e.Aprend04).HasColumnName("aprend04");
            entity.Property(e => e.Aprend05).HasColumnName("aprend05");
            entity.Property(e => e.Aprend06).HasColumnName("aprend06");
            entity.Property(e => e.Aprend07).HasColumnName("aprend07");
            entity.Property(e => e.Aprend08).HasColumnName("aprend08");
            entity.Property(e => e.Aprend09).HasColumnName("aprend09");
            entity.Property(e => e.Aprend10).HasColumnName("aprend10");
            entity.Property(e => e.Aprend11).HasColumnName("aprend11");
            entity.Property(e => e.Aprend12).HasColumnName("aprend12");
            entity.Property(e => e.Aprend13).HasColumnName("aprend13");
            entity.Property(e => e.AprendAdul01).HasColumnName("aprendAdul01");
            entity.Property(e => e.AprendAdul02).HasColumnName("aprendAdul02");
            entity.Property(e => e.AprendAdul03).HasColumnName("aprendAdul03");
            entity.Property(e => e.AprendAdul04).HasColumnName("aprendAdul04");
            entity.Property(e => e.AprendAdul05).HasColumnName("aprendAdul05");
            entity.Property(e => e.AprendAdul06).HasColumnName("aprendAdul06");
            entity.Property(e => e.AprendEsc01).HasColumnName("aprendEsc01");
            entity.Property(e => e.AprendEsc02).HasColumnName("aprendEsc02");
            entity.Property(e => e.AprendEsc03).HasColumnName("aprendEsc03");
            entity.Property(e => e.AprendEsc04).HasColumnName("aprendEsc04");
            entity.Property(e => e.AprendEsc05).HasColumnName("aprendEsc05");
            entity.Property(e => e.AprendEsc06).HasColumnName("aprendEsc06");
            entity.Property(e => e.AprendEsc07).HasColumnName("aprendEsc07");
            entity.Property(e => e.AprendEsc08).HasColumnName("aprendEsc08");
            entity.Property(e => e.AprendEsc09).HasColumnName("aprendEsc09");
            entity.Property(e => e.AprendEsc10).HasColumnName("aprendEsc10");
            entity.Property(e => e.AprendEsc11).HasColumnName("aprendEsc11");
            entity.Property(e => e.AprendEsc12).HasColumnName("aprendEsc12");
            entity.Property(e => e.AprendEsc13).HasColumnName("aprendEsc13");
            entity.Property(e => e.AprendEsc14).HasColumnName("aprendEsc14");
            entity.Property(e => e.AprendEsc15).HasColumnName("aprendEsc15");
            entity.Property(e => e.AprendPre01).HasColumnName("aprendPre01");
            entity.Property(e => e.AprendPre02).HasColumnName("aprendPre02");
            entity.Property(e => e.AprendPre03).HasColumnName("aprendPre03");
            entity.Property(e => e.AprendPre04).HasColumnName("aprendPre04");
            entity.Property(e => e.AprendPre05).HasColumnName("aprendPre05");
            entity.Property(e => e.AprendPre06).HasColumnName("aprendPre06");
            entity.Property(e => e.AprendPre07).HasColumnName("aprendPre07");
            entity.Property(e => e.AprendPre08).HasColumnName("aprendPre08");
            entity.Property(e => e.AprendPre09).HasColumnName("aprendPre09");
            entity.Property(e => e.AprendPre10).HasColumnName("aprendPre10");
            entity.Property(e => e.AprendUni01).HasColumnName("aprendUni01");
            entity.Property(e => e.AprendUni02).HasColumnName("aprendUni02");
            entity.Property(e => e.AprendUni03).HasColumnName("aprendUni03");
            entity.Property(e => e.AprendUni04).HasColumnName("aprendUni04");
            entity.Property(e => e.AprendUni05).HasColumnName("aprendUni05");
            entity.Property(e => e.AprendUni06).HasColumnName("aprendUni06");
            entity.Property(e => e.AprendUni07).HasColumnName("aprendUni07");
            entity.Property(e => e.AprendUni08).HasColumnName("aprendUni08");
            entity.Property(e => e.AprendiObs)
                .HasMaxLength(2000)
                .HasColumnName("aprendiObs");
            entity.Property(e => e.AudioObs)
                .HasMaxLength(2000)
                .HasColumnName("audioObs");
            entity.Property(e => e.AudioPerdAdul01).HasColumnName("audioPerdAdul01");
            entity.Property(e => e.AudioPerdAdul02).HasColumnName("audioPerdAdul02");
            entity.Property(e => e.AudioPerdAdul03).HasColumnName("audioPerdAdul03");
            entity.Property(e => e.AudioPerdAdul04).HasColumnName("audioPerdAdul04");
            entity.Property(e => e.AudioPerdAdul05).HasColumnName("audioPerdAdul05");
            entity.Property(e => e.AudioPerdAdul06).HasColumnName("audioPerdAdul06");
            entity.Property(e => e.AudioPerdAdul07).HasColumnName("audioPerdAdul07");
            entity.Property(e => e.AudioPerdAdul08).HasColumnName("audioPerdAdul08");
            entity.Property(e => e.AudioPerdAdul09).HasColumnName("audioPerdAdul09");
            entity.Property(e => e.AudioPerdAdul10).HasColumnName("audioPerdAdul10");
            entity.Property(e => e.AudioPerdBebe01).HasColumnName("audioPerdBebe01");
            entity.Property(e => e.AudioPerdBebe02).HasColumnName("audioPerdBebe02");
            entity.Property(e => e.AudioPerdBebe03).HasColumnName("audioPerdBebe03");
            entity.Property(e => e.AudioPerdBebe04).HasColumnName("audioPerdBebe04");
            entity.Property(e => e.AudioPerdBebe05).HasColumnName("audioPerdBebe05");
            entity.Property(e => e.AudioPerdBebe06).HasColumnName("audioPerdBebe06");
            entity.Property(e => e.AudioPerdBebe07).HasColumnName("audioPerdBebe07");
            entity.Property(e => e.AudioPerdBebe08).HasColumnName("audioPerdBebe08");
            entity.Property(e => e.AudioPerdBebe09).HasColumnName("audioPerdBebe09");
            entity.Property(e => e.AudioPerdBebe10).HasColumnName("audioPerdBebe10");
            entity.Property(e => e.AudioProcAud01).HasColumnName("audioProcAud01");
            entity.Property(e => e.AudioProcAud02).HasColumnName("audioProcAud02");
            entity.Property(e => e.AudioProcAud03).HasColumnName("audioProcAud03");
            entity.Property(e => e.AudioProcAud04).HasColumnName("audioProcAud04");
            entity.Property(e => e.AudioProcAud05).HasColumnName("audioProcAud05");
            entity.Property(e => e.AudioProcAud06).HasColumnName("audioProcAud06");
            entity.Property(e => e.AudioProcAud07).HasColumnName("audioProcAud07");
            entity.Property(e => e.AudioProcAud08).HasColumnName("audioProcAud08");
            entity.Property(e => e.AudioProcAud09).HasColumnName("audioProcAud09");
            entity.Property(e => e.AudioProcAud10).HasColumnName("audioProcAud10");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Cuerpo01).HasColumnName("cuerpo01");
            entity.Property(e => e.Cuerpo02).HasColumnName("cuerpo02");
            entity.Property(e => e.Cuerpo03).HasColumnName("cuerpo03");
            entity.Property(e => e.Cuerpo04).HasColumnName("cuerpo04");
            entity.Property(e => e.Cuerpo05).HasColumnName("cuerpo05");
            entity.Property(e => e.Cuerpo06).HasColumnName("cuerpo06");
            entity.Property(e => e.Cuerpo07).HasColumnName("cuerpo07");
            entity.Property(e => e.Cuerpo08).HasColumnName("cuerpo08");
            entity.Property(e => e.Cuerpo09).HasColumnName("cuerpo09");
            entity.Property(e => e.Cuerpo10).HasColumnName("cuerpo10");
            entity.Property(e => e.Cuerpo11).HasColumnName("cuerpo11");
            entity.Property(e => e.Cuerpo12).HasColumnName("cuerpo12");
            entity.Property(e => e.Cuerpo13).HasColumnName("cuerpo13");
            entity.Property(e => e.Cuerpo14).HasColumnName("cuerpo14");
            entity.Property(e => e.CuerpoObs)
                .HasMaxLength(2000)
                .HasColumnName("cuerpoObs");
            entity.Property(e => e.Habla01).HasColumnName("habla01");
            entity.Property(e => e.Habla02).HasColumnName("habla02");
            entity.Property(e => e.Habla03).HasColumnName("habla03");
            entity.Property(e => e.Habla04).HasColumnName("habla04");
            entity.Property(e => e.Habla05).HasColumnName("habla05");
            entity.Property(e => e.Habla06).HasColumnName("habla06");
            entity.Property(e => e.Habla07).HasColumnName("habla07");
            entity.Property(e => e.Habla08).HasColumnName("habla08");
            entity.Property(e => e.Habla09).HasColumnName("habla09");
            entity.Property(e => e.Habla10).HasColumnName("habla10");
            entity.Property(e => e.Habla11).HasColumnName("habla11");
            entity.Property(e => e.Habla12).HasColumnName("habla12");
            entity.Property(e => e.Habla13).HasColumnName("habla13");
            entity.Property(e => e.Habla14).HasColumnName("habla14");
            entity.Property(e => e.Habla15).HasColumnName("habla15");
            entity.Property(e => e.Habla16).HasColumnName("habla16");
            entity.Property(e => e.Habla17).HasColumnName("habla17");
            entity.Property(e => e.Habla18).HasColumnName("habla18");
            entity.Property(e => e.HablaObs)
                .HasMaxLength(2000)
                .HasColumnName("hablaObs");
            entity.Property(e => e.Lenguaje01).HasColumnName("lenguaje01");
            entity.Property(e => e.Lenguaje02).HasColumnName("lenguaje02");
            entity.Property(e => e.Lenguaje03).HasColumnName("lenguaje03");
            entity.Property(e => e.Lenguaje04).HasColumnName("lenguaje04");
            entity.Property(e => e.Lenguaje05).HasColumnName("lenguaje05");
            entity.Property(e => e.Lenguaje06).HasColumnName("lenguaje06");
            entity.Property(e => e.Lenguaje07).HasColumnName("lenguaje07");
            entity.Property(e => e.Lenguaje08).HasColumnName("lenguaje08");
            entity.Property(e => e.Lenguaje09).HasColumnName("lenguaje09");
            entity.Property(e => e.Lenguaje10).HasColumnName("lenguaje10");
            entity.Property(e => e.Lenguaje11).HasColumnName("lenguaje11");
            entity.Property(e => e.Lenguaje12).HasColumnName("lenguaje12");
            entity.Property(e => e.Lenguaje13).HasColumnName("lenguaje13");
            entity.Property(e => e.LenguajeObs)
                .HasMaxLength(2000)
                .HasColumnName("lenguajeObs");
            entity.Property(e => e.PsicoObs)
                .HasMaxLength(2000)
                .HasColumnName("psicoObs");
            entity.Property(e => e.Psicologia01)
                .HasMaxLength(50)
                .HasColumnName("psicologia01");
            entity.Property(e => e.TriageOnlineId).HasColumnName("triageOnlineID");
            entity.Property(e => e.UdadConduc01).HasColumnName("udadConduc01");
            entity.Property(e => e.UdadConduc02).HasColumnName("udadConduc02");
            entity.Property(e => e.UdadConduc03).HasColumnName("udadConduc03");
            entity.Property(e => e.UdadConduc04).HasColumnName("udadConduc04");
            entity.Property(e => e.UdadEstereo01).HasColumnName("udadEstereo01");
            entity.Property(e => e.UdadEstereo02).HasColumnName("udadEstereo02");
            entity.Property(e => e.UdadFsenso01).HasColumnName("udadFSenso01");
            entity.Property(e => e.UdadFsenso02).HasColumnName("udadFSenso02");
            entity.Property(e => e.UdadJuego01).HasColumnName("udadJuego01");
            entity.Property(e => e.UdadJuego02).HasColumnName("udadJuego02");
            entity.Property(e => e.UdadLf01).HasColumnName("udadLF01");
            entity.Property(e => e.UdadLf02).HasColumnName("udadLF02");
            entity.Property(e => e.UdadLf03).HasColumnName("udadLF03");
            entity.Property(e => e.UdadLf04).HasColumnName("udadLF04");
            entity.Property(e => e.UdadLf05).HasColumnName("udadLF05");
            entity.Property(e => e.UdadLf06).HasColumnName("udadLF06");
            entity.Property(e => e.UdadObs)
                .HasMaxLength(2000)
                .HasColumnName("udadObs");
            entity.Property(e => e.UdadPl01).HasColumnName("udadPL01");
            entity.Property(e => e.UdadPl02).HasColumnName("udadPL02");
            entity.Property(e => e.UdadPl03).HasColumnName("udadPL03");
            entity.Property(e => e.UdadPl04).HasColumnName("udadPL04");
            entity.Property(e => e.UdadPl05).HasColumnName("udadPL05");
            entity.Property(e => e.UdadPl06).HasColumnName("udadPL06");
            entity.Property(e => e.UdadPl07).HasColumnName("udadPL07");
            entity.Property(e => e.UdadSensorial01).HasColumnName("udadSensorial01");
            entity.Property(e => e.UdadSensorial02).HasColumnName("udadSensorial02");
            entity.Property(e => e.UdadSensorial03).HasColumnName("udadSensorial03");
            entity.Property(e => e.UdadSensorial04).HasColumnName("udadSensorial04");

            entity.HasOne(d => d.TriageOnline).WithMany(p => p.TriageCotejoExts)
                .HasForeignKey(d => d.TriageOnlineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TriageCotejoExt_TriageOnline");
        });

        modelBuilder.Entity<TriageOnline>(entity =>
        {
            entity.ToTable("TriageOnline");

            entity.Property(e => e.TriageOnlineId).HasColumnName("triageOnlineID");
            entity.Property(e => e.ActualizadoPor)
                .HasMaxLength(50)
                .HasColumnName("actualizadoPor");
            entity.Property(e => e.ApellidoMaternoPaciente)
                .HasMaxLength(50)
                .HasColumnName("apellidoMaternoPaciente");
            entity.Property(e => e.ApellidoPaternoPaciente)
                .HasMaxLength(50)
                .HasColumnName("apellidoPaternoPaciente");
            entity.Property(e => e.ApoderadoEmail)
                .HasMaxLength(100)
                .HasColumnName("apoderadoEmail");
            entity.Property(e => e.ApoderadoNombre)
                .HasMaxLength(100)
                .HasColumnName("apoderadoNombre");
            entity.Property(e => e.ApoderadoRelacion)
                .HasMaxLength(100)
                .HasColumnName("apoderadoRelacion");
            entity.Property(e => e.ApoderadoTelCelular)
                .HasMaxLength(50)
                .HasColumnName("apoderadoTelCelular");
            entity.Property(e => e.ApoderadoTelFijo)
                .HasMaxLength(50)
                .HasColumnName("apoderadoTelFijo");
            entity.Property(e => e.Area).HasColumnName("area");
            entity.Property(e => e.CelularMadre)
                .HasMaxLength(20)
                .HasColumnName("celularMadre");
            entity.Property(e => e.CelularPadre)
                .HasMaxLength(20)
                .HasColumnName("celularPadre");
            entity.Property(e => e.CentroTrabajoMadre)
                .HasMaxLength(100)
                .HasColumnName("centroTrabajoMadre");
            entity.Property(e => e.CentroTrabajoPadre)
                .HasMaxLength(100)
                .HasColumnName("centroTrabajoPadre");
            entity.Property(e => e.CodigoColegio).HasColumnName("codigoColegio");
            entity.Property(e => e.Colegio)
                .HasMaxLength(100)
                .HasColumnName("colegio");
            entity.Property(e => e.Comollegocpal).HasColumnName("comollegocpal");
            entity.Property(e => e.ConsultoPor)
                .HasMaxLength(100)
                .HasColumnName("consultoPor");
            entity.Property(e => e.ContactoFechaNac)
                .HasColumnType("datetime")
                .HasColumnName("contactoFechaNac");
            entity.Property(e => e.ContactoNroDoc)
                .HasMaxLength(50)
                .HasColumnName("contactoNroDoc");
            entity.Property(e => e.ContactoSexo).HasColumnName("contactoSexo");
            entity.Property(e => e.ContactoTipoDoc)
                .HasMaxLength(50)
                .HasColumnName("contactoTipoDoc");
            entity.Property(e => e.Coordinarcoleg).HasColumnName("coordinarcoleg");
            entity.Property(e => e.CorreoNombreDe)
                .HasMaxLength(100)
                .HasColumnName("correoNombreDe");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.DepartamenteEnvio).HasColumnName("departamenteEnvio");
            entity.Property(e => e.Dependiente).HasColumnName("dependiente");
            entity.Property(e => e.Descomollego)
                .HasMaxLength(1000)
                .HasColumnName("descomollego");
            entity.Property(e => e.DetalleOrientacion)
                .HasMaxLength(2000)
                .HasColumnName("detalleOrientacion");
            entity.Property(e => e.DireccionEnvio)
                .HasMaxLength(200)
                .HasColumnName("direccionEnvio");
            entity.Property(e => e.DistritoEnvio).HasColumnName("distritoEnvio");
            entity.Property(e => e.EliminadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("eliminadoFecha");
            entity.Property(e => e.EmaiMadre)
                .HasMaxLength(100)
                .HasColumnName("emaiMadre");
            entity.Property(e => e.EmailBanco1)
                .HasMaxLength(100)
                .HasColumnName("emailBanco1");
            entity.Property(e => e.EmailBanco2)
                .HasMaxLength(100)
                .HasColumnName("emailBanco2");
            entity.Property(e => e.EmailBanco3)
                .HasMaxLength(100)
                .HasColumnName("emailBanco3");
            entity.Property(e => e.EmailBanco4)
                .HasMaxLength(100)
                .HasColumnName("emailBanco4");
            entity.Property(e => e.EmailBancoCuenta1)
                .HasMaxLength(100)
                .HasColumnName("emailBancoCuenta1");
            entity.Property(e => e.EmailBancoCuenta2)
                .HasMaxLength(100)
                .HasColumnName("emailBancoCuenta2");
            entity.Property(e => e.EmailBancoCuenta3)
                .HasMaxLength(100)
                .HasColumnName("emailBancoCuenta3");
            entity.Property(e => e.EmailBancoCuenta4)
                .HasMaxLength(100)
                .HasColumnName("emailBancoCuenta4");
            entity.Property(e => e.EmailBancoCuentaCci1)
                .HasMaxLength(100)
                .HasColumnName("emailBancoCuentaCCI1");
            entity.Property(e => e.EmailBancoCuentaCci2)
                .HasMaxLength(100)
                .HasColumnName("emailBancoCuentaCCI2");
            entity.Property(e => e.EmailBancoCuentaCci3)
                .HasMaxLength(100)
                .HasColumnName("emailBancoCuentaCCI3");
            entity.Property(e => e.EmailBancoCuentaCci4)
                .HasMaxLength(100)
                .HasColumnName("emailBancoCuentaCCI4");
            entity.Property(e => e.EmailEvaluaciones)
                .HasMaxLength(4000)
                .HasColumnName("emailEvaluaciones");
            entity.Property(e => e.EmailPadre)
                .HasMaxLength(100)
                .HasColumnName("emailPadre");
            entity.Property(e => e.EmailProcedimiento1)
                .HasMaxLength(3000)
                .HasColumnName("emailProcedimiento1");
            entity.Property(e => e.EmailProcedimiento2)
                .HasMaxLength(3000)
                .HasColumnName("emailProcedimiento2");
            entity.Property(e => e.EmailProcedimiento3)
                .HasMaxLength(3000)
                .HasColumnName("emailProcedimiento3");
            entity.Property(e => e.EmailRegistro)
                .HasMaxLength(100)
                .HasColumnName("emailRegistro");
            entity.Property(e => e.EsOrientado).HasColumnName("esOrientado");
            entity.Property(e => e.EvaluaAprendizaje).HasColumnName("evaluaAprendizaje");
            entity.Property(e => e.EvaluaDisfluencia).HasColumnName("evaluaDisfluencia");
            entity.Property(e => e.EvaluaMotividad).HasColumnName("evaluaMotividad");
            entity.Property(e => e.EvaluaTrastComunicacion).HasColumnName("evaluaTrastComunicacion");
            entity.Property(e => e.EvaluaVoz).HasColumnName("evaluaVoz");
            entity.Property(e => e.EvaluarAudiologia).HasColumnName("evaluarAudiologia");
            entity.Property(e => e.EvaluarHabla).HasColumnName("evaluarHabla");
            entity.Property(e => e.EvaluarLenguajeAprendizaje).HasColumnName("evaluarLenguajeAprendizaje");
            entity.Property(e => e.EvaluarNeurologia).HasColumnName("evaluarNeurologia");
            entity.Property(e => e.EvaluarOrientacionVocacional).HasColumnName("evaluarOrientacionVocacional");
            entity.Property(e => e.EvaluarPsicologia).HasColumnName("evaluarPsicologia");
            entity.Property(e => e.EvaluarPsicomotriz).HasColumnName("evaluarPsicomotriz");
            entity.Property(e => e.Evaluaudad).HasColumnName("evaluaudad");
            entity.Property(e => e.FechaEvaluacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaEvaluacion");
            entity.Property(e => e.FechaNacimientoPaciente)
                .HasColumnType("datetime")
                .HasColumnName("fechaNacimientoPaciente");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.GuidCheck).HasColumnName("guidCheck");
            entity.Property(e => e.GuidCode)
                .HasMaxLength(50)
                .HasColumnName("guidCode");
            entity.Property(e => e.GuidEndDate)
                .HasColumnType("datetime")
                .HasColumnName("guidEndDate");
            entity.Property(e => e.IdColegio).HasColumnName("id_colegio");
            entity.Property(e => e.LinkPago)
                .HasMaxLength(2000)
                .HasColumnName("linkPago");
            entity.Property(e => e.LugarEntreHermanos).HasColumnName("lugarEntreHermanos");
            entity.Property(e => e.MotivoConsulta)
                .HasMaxLength(2000)
                .HasColumnName("motivoConsulta");
            entity.Property(e => e.NombreMadre)
                .HasMaxLength(100)
                .HasColumnName("nombreMadre");
            entity.Property(e => e.NombrePadre)
                .HasMaxLength(100)
                .HasColumnName("nombrePadre");
            entity.Property(e => e.NombresPaciente)
                .HasMaxLength(50)
                .HasColumnName("nombresPaciente");
            entity.Property(e => e.NumeroHijosMujeres).HasColumnName("numeroHijosMujeres");
            entity.Property(e => e.NumeroHijosVarones).HasColumnName("numeroHijosVarones");
            entity.Property(e => e.Observaciones).HasMaxLength(2000);
            entity.Property(e => e.OcupacionMadre)
                .HasMaxLength(100)
                .HasColumnName("ocupacionMadre");
            entity.Property(e => e.OcupacionPadre)
                .HasMaxLength(50)
                .HasColumnName("ocupacionPadre");
            entity.Property(e => e.PacienteCentroEducativo)
                .HasMaxLength(100)
                .HasColumnName("pacienteCentroEducativo");
            entity.Property(e => e.PacienteCodigoDepartamento).HasColumnName("pacienteCodigoDepartamento");
            entity.Property(e => e.PacienteCodigoDistrito).HasColumnName("pacienteCodigoDistrito");
            entity.Property(e => e.PacienteCodigoPais).HasColumnName("pacienteCodigoPais");
            entity.Property(e => e.PacienteCodigoProvincia).HasColumnName("pacienteCodigoProvincia");
            entity.Property(e => e.PacienteDerivadoPor)
                .HasMaxLength(100)
                .HasColumnName("pacienteDerivadoPor");
            entity.Property(e => e.PacienteDireccion)
                .HasMaxLength(100)
                .HasColumnName("pacienteDireccion");
            entity.Property(e => e.PacienteDireccion2)
                .HasMaxLength(100)
                .HasColumnName("pacienteDireccion2");
            entity.Property(e => e.PacienteDni)
                .HasMaxLength(50)
                .HasColumnName("pacienteDNI");
            entity.Property(e => e.PacienteDomicilioTel)
                .HasMaxLength(50)
                .HasColumnName("pacienteDomicilioTel");
            entity.Property(e => e.PacienteDomicilioTelEmer)
                .HasMaxLength(50)
                .HasColumnName("pacienteDomicilioTelEmer");
            entity.Property(e => e.PacienteEmail)
                .HasMaxLength(100)
                .HasColumnName("pacienteEmail");
            entity.Property(e => e.PacienteGrado)
                .HasMaxLength(50)
                .HasColumnName("pacienteGrado");
            entity.Property(e => e.PacienteObservaciones)
                .HasMaxLength(4000)
                .HasColumnName("pacienteObservaciones");
            entity.Property(e => e.PacienteProfEmail)
                .HasMaxLength(100)
                .HasColumnName("pacienteProfEmail");
            entity.Property(e => e.PacienteProfNombre)
                .HasMaxLength(100)
                .HasColumnName("pacienteProfNombre");
            entity.Property(e => e.PacienteProfTel)
                .HasMaxLength(100)
                .HasColumnName("pacienteProfTel");
            entity.Property(e => e.PacienteTel)
                .HasMaxLength(50)
                .HasColumnName("pacienteTel");
            entity.Property(e => e.PacienteTratAnteriores)
                .HasMaxLength(4000)
                .HasColumnName("pacienteTratAnteriores");
            entity.Property(e => e.PacienteViveCon)
                .HasMaxLength(100)
                .HasColumnName("pacienteViveCon");
            entity.Property(e => e.PaisEnvio).HasColumnName("paisEnvio");
            entity.Property(e => e.ParentescoPaciente)
                .HasMaxLength(100)
                .HasColumnName("parentescoPaciente");
            entity.Property(e => e.PersonalId).HasColumnName("personalID");
            entity.Property(e => e.Presuncion).HasColumnName("presuncion");
            entity.Property(e => e.Procedencia)
                .HasMaxLength(50)
                .HasColumnName("procedencia");
            entity.Property(e => e.ProvinciaEnvio).HasColumnName("provinciaEnvio");
            entity.Property(e => e.RefNroHistoriaClinica).HasColumnName("refNroHistoriaClinica");
            entity.Property(e => e.RefUsuarioId)
                .HasMaxLength(450)
                .HasColumnName("refUsuarioId");
            entity.Property(e => e.Responsable).HasColumnName("responsable");
            entity.Property(e => e.SedeId)
                .HasDefaultValue(1)
                .HasColumnName("sedeID");
            entity.Property(e => e.SexoPaciente).HasColumnName("sexoPaciente");
            entity.Property(e => e.TelefonoMadre)
                .HasMaxLength(20)
                .HasColumnName("telefonoMadre");
            entity.Property(e => e.TelefonoPaciente)
                .HasMaxLength(20)
                .HasColumnName("telefonoPaciente");
            entity.Property(e => e.TelefonoPadre)
                .HasMaxLength(20)
                .HasColumnName("telefonoPadre");
            entity.Property(e => e.Tipo)
                .HasDefaultValue(0)
                .HasColumnName("tipo");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .HasColumnName("tipoDocumento");
            entity.Property(e => e.TriageGenerado).HasColumnName("triageGenerado");
            entity.Property(e => e.TriageNoRef).HasColumnName("triageNoRef");
            entity.Property(e => e.TriageOnlineEstadoId).HasColumnName("triageOnlineEstadoID");
            entity.Property(e => e.TriageTipo).HasColumnName("triageTipo");
            entity.Property(e => e.UltimaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("ultimaActualizacion");
        });

        modelBuilder.Entity<UvPagoCitaDetalleSearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvPagoCitaDetalleSearch");

            entity.Property(e => e.Duracion).HasColumnName("duracion");
            entity.Property(e => e.CitaPara).HasColumnName("citaPara");
            entity.Property(e => e.EspecialistaId).HasColumnName("especialistaId");
            entity.Property(e => e.EvaluacionAreaId).HasColumnName("evaluacionAreaId");
            entity.Property(e => e.EvaluacionAreaNombre)
                .HasMaxLength(100)
                .HasColumnName("evaluacionAreaNombre");
            entity.Property(e => e.EvaluacionCitaId).HasColumnName("evaluacionCitaId");
            entity.Property(e => e.EvaluacionId).HasColumnName("evaluacionId");
            entity.Property(e => e.EvaluacionNombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("evaluacionNombre");
            entity.Property(e => e.FechaCita)
                .HasColumnType("datetime")
                .HasColumnName("fechaCita");
            entity.Property(e => e.HoraCitaDesde).HasColumnName("horaCitaDesde");
            entity.Property(e => e.HoraCitaHasta).HasColumnName("horaCitaHasta");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("imagenUrl");
            entity.Property(e => e.IsProgramado).HasColumnName("isProgramado");
            entity.Property(e => e.NombreCita)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreCita");
            entity.Property(e => e.NumeroCita).HasColumnName("numeroCita");
            entity.Property(e => e.PagoCitaDetalleId).HasColumnName("pagoCitaDetalleId");
            entity.Property(e => e.PagoCitaId).HasColumnName("pagoCitaId");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.SedeId).HasColumnName("sedeId");
            entity.Property(e => e.SedeNombre)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("sedeNombre");
            entity.Property(e => e.TipoCitaId).HasColumnName("tipoCitaId");
            entity.Property(e => e.TipoCitaNombre)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipoCitaNombre");
        });

        modelBuilder.Entity<PagoCitaDetalle>(entity =>
        {
            entity.HasKey(e => e.PagoCitaDetalleId).HasName("PK__PagoCita__F1A174CFCA74EFB4");

            entity.ToTable("PagoCitaDetalle");

            entity.Property(e => e.PagoCitaDetalleId).HasColumnName("pagoCitaDetalleId");
            entity.Property(e => e.EspecialistaId).HasColumnName("especialistaId");
            entity.Property(e => e.EvaluacionAreaId).HasColumnName("evaluacionAreaId");
            entity.Property(e => e.EvaluacionCitaId).HasColumnName("evaluacionCitaId");
            entity.Property(e => e.EvaluacionCitaNombre)
                .HasMaxLength(50)
                .HasColumnName("evaluacionCitaNombre");
            entity.Property(e => e.EvaluacionId).HasColumnName("evaluacionId");
            entity.Property(e => e.EvaluacionNombre)
                .HasMaxLength(255)
                .HasColumnName("evaluacionNombre");
            entity.Property(e => e.FechaCita)
                .HasColumnType("datetime")
                .HasColumnName("fechaCita");
            entity.Property(e => e.HoraCitaDesde).HasColumnName("horaCitaDesde");
            entity.Property(e => e.HoraCitaHasta).HasColumnName("horaCitaHasta");
            entity.Property(e => e.IsPagado)
                .HasDefaultValue(false)
                .HasColumnName("isPagado");
            entity.Property(e => e.IsProgramado)
                .HasDefaultValue(false)
                .HasColumnName("isProgramado");
            entity.Property(e => e.NumeroCita).HasColumnName("numeroCita");
            entity.Property(e => e.PagoCitaId).HasColumnName("pagoCitaId");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.SedeId).HasColumnName("sedeId");
            entity.Property(e => e.TipoCitaId).HasColumnName("tipoCitaId");

            entity.HasOne(d => d.PagoCita).WithMany(p => p.PagoCitaDetalles)
                .HasForeignKey(d => d.PagoCitaId)
                .HasConstraintName("FK__PagoCitaD__pagoC__619DE24D");
        });

        modelBuilder.Entity<PagoCita>(entity =>
        {
            entity.HasKey(e => e.PagoCitaId).HasName("PK__PagoCita__44DA2164F0E313EC");
            entity.ToTable("PagoCita");
            entity.Property(e => e.PagoCitaId).HasColumnName("pagoCitaId");
            entity.Property(e => e.CreadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("creadoFecha");
            entity.Property(e => e.CreadoPor)
                .HasMaxLength(50)
                .HasColumnName("creadoPor");
            entity.Property(e => e.EliminadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("eliminadoFecha");
            entity.Property(e => e.EstadoPago)
                .HasMaxLength(10)
                .HasColumnName("estadoPago");
            entity.Property(e => e.HistoriaClinica).HasColumnName("historiaClinica");
            entity.Property(e => e.PagoTriajeIdRef).HasColumnName("pagoTriajeIdRef");
            entity.Property(e => e.TiempoDeExpiracion)
                .HasColumnType("datetime")
                .HasColumnName("tiempoDeExpiracion");
            entity.Property(e => e.TriajeOnlineId).HasColumnName("triajeOnlineId");
            entity.Property(e => e.UsuarioId)
                .HasMaxLength(450)
                .HasColumnName("usuarioId");

            entity.HasOne(d => d.TriajeOnline).WithMany(p => p.PagoCitas)
                .HasForeignKey(d => d.TriajeOnlineId)
                .HasConstraintName("FK_PagoCita_TriageOnline");
        });

        modelBuilder.Entity<UvEvaluacionCitaSearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvEvaluacionCitaSearch");

            entity.Property(e => e.Area)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Especialidad).HasMaxLength(100);
            entity.Property(e => e.EspecialidadId).HasColumnName("EspecialidadID");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Evaluacion)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.EvaluacionId).HasColumnName("evaluacionID");
            entity.Property(e => e.Sesiones).HasMaxLength(90);
        });

        modelBuilder.Entity<UvPagoCitasSearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvPagoCitasSearch");

            entity.Property(e => e.CitaNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("citaNombre");
            entity.Property(e => e.CreadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("creadoFecha");
            entity.Property(e => e.CreadoPor)
                .HasMaxLength(50)
                .HasColumnName("creadoPor");
            entity.Property(e => e.EliminadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("eliminadoFecha");
            entity.Property(e => e.EspecialistaId).HasColumnName("especialistaId");
            entity.Property(e => e.EstadoPago)
                .HasMaxLength(10)
                .HasColumnName("estadoPago");
            entity.Property(e => e.EvaluacionAreaId).HasColumnName("evaluacionAreaId");
            entity.Property(e => e.EvaluacionCitaId).HasColumnName("evaluacionCitaId");
            entity.Property(e => e.EvaluacionId).HasColumnName("evaluacionId");
            entity.Property(e => e.FechaCita)
                .HasColumnType("datetime")
                .HasColumnName("fechaCita");
            entity.Property(e => e.FechaNacimientoPaciente)
                .HasColumnType("datetime")
                .HasColumnName("fechaNacimientoPaciente");
            entity.Property(e => e.HistoriaClinica).HasColumnName("historiaClinica");
            entity.Property(e => e.HoraCitaDesde).HasColumnName("horaCitaDesde");
            entity.Property(e => e.HoraCitaHasta).HasColumnName("horaCitaHasta");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("imagenUrl");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.NumeroCita).HasColumnName("numeroCita");
            entity.Property(e => e.PagoCitaDetalleId).HasColumnName("pagoCitaDetalleId");
            entity.Property(e => e.PagoCitaId).HasColumnName("pagoCitaId");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.Sede)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("sede");
            entity.Property(e => e.SedeId).HasColumnName("sedeId");
            entity.Property(e => e.TipoCitaId).HasColumnName("tipoCitaId");
            entity.Property(e => e.TriajeOnlineId).HasColumnName("triajeOnlineId");
            entity.Property(e => e.UsuarioId)
                .HasMaxLength(450)
                .HasColumnName("usuarioId");
        });

        modelBuilder.Entity<UvPagoTriajeSearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvPagoTriajeSearch");

            entity.Property(e => e.ApellidoMaternoPaciente)
                .HasMaxLength(50)
                .HasColumnName("apellidoMaternoPaciente");
            entity.Property(e => e.ApellidoPaternoPaciente)
                .HasMaxLength(50)
                .HasColumnName("apellidoPaternoPaciente");
            entity.Property(e => e.CreadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("creadoFecha");
            entity.Property(e => e.CreadoPor)
                .HasMaxLength(50)
                .HasColumnName("creadoPor");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.EliminadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("eliminadoFecha");
            entity.Property(e => e.EstadoPago)
                .HasMaxLength(10)
                .HasColumnName("estadoPago");
            entity.Property(e => e.FechaPago).HasColumnType("datetime");
            entity.Property(e => e.FechaTriajeOnline).HasColumnType("datetime");
            entity.Property(e => e.HistoriaClinica).HasColumnName("historiaClinica");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.NombreCompleto).HasMaxLength(104);
            entity.Property(e => e.NombresPaciente)
                .HasMaxLength(50)
                .HasColumnName("nombresPaciente");
            entity.Property(e => e.PagoCitaId).HasColumnName("pagoCitaId");
            entity.Property(e => e.TriajeOnlineId).HasColumnName("triajeOnlineId");
            entity.Property(e => e.UsuarioId)
                .HasMaxLength(450)
                .HasColumnName("usuarioId");
        });

        modelBuilder.Entity<UvPersonalEvaluacionSearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvPersonalEvaluacionSearch");

            entity.Property(e => e.Apellidos)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("apellidos");
            entity.Property(e => e.EvaluacionId).HasColumnName("evaluacionID");
            entity.Property(e => e.Monto).HasColumnName("monto");
            entity.Property(e => e.Nombres)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombres");
            entity.Property(e => e.PersonalId).HasColumnName("personalID");
            entity.Property(e => e.PersonalPorcentageId).HasColumnName("personalPorcentageID");
            entity.Property(e => e.Porcentage).HasColumnName("porcentage");
            entity.Property(e => e.PorcentajeTipo).HasColumnName("porcentajeTipo");
            entity.Property(e => e.RangoEdadAtencionId).HasColumnName("rangoEdadAtencionID");
        });

        modelBuilder.Entity<PersonalHorario>(entity =>
        {
            entity.HasKey(e => e.PersonalHorarioId).HasName("PersonalHorario_PK");

            entity.ToTable("PersonalHorario");

            entity.HasIndex(e => e.PersonalId, "personal_FK1");

            entity.Property(e => e.PersonalHorarioId).HasColumnName("personalHorarioID");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Admin")
                .HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.DomingoFin)
                .HasColumnType("datetime")
                .HasColumnName("domingoFin");
            entity.Property(e => e.DomingoFinTarde)
                .HasColumnType("datetime")
                .HasColumnName("domingoFinTarde");
            entity.Property(e => e.DomingoFinTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("domingoFinTardeTra");
            entity.Property(e => e.DomingoFinTra)
                .HasColumnType("datetime")
                .HasColumnName("domingoFinTra");
            entity.Property(e => e.DomingoFinVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("domingoFinVirDiag");
            entity.Property(e => e.DomingoFinVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("domingoFinVirDiagTarde");
            entity.Property(e => e.DomingoFinVirTra)
                .HasColumnType("datetime")
                .HasColumnName("domingoFinVirTra");
            entity.Property(e => e.DomingoFinVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("domingoFinVirTraTarde");
            entity.Property(e => e.DomingoInicio)
                .HasColumnType("datetime")
                .HasColumnName("domingoInicio");
            entity.Property(e => e.DomingoInicioTarde)
                .HasColumnType("datetime")
                .HasColumnName("domingoInicioTarde");
            entity.Property(e => e.DomingoInicioTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("domingoInicioTardeTra");
            entity.Property(e => e.DomingoInicioTra)
                .HasColumnType("datetime")
                .HasColumnName("domingoInicioTra");
            entity.Property(e => e.DomingoInicioVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("domingoInicioVirDiag");
            entity.Property(e => e.DomingoInicioVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("domingoInicioVirDiagTarde");
            entity.Property(e => e.DomingoInicioVirTra)
                .HasColumnType("datetime")
                .HasColumnName("domingoInicioVirTra");
            entity.Property(e => e.DomingoInicioVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("domingoInicioVirTraTarde");
            entity.Property(e => e.JuevesFin)
                .HasColumnType("datetime")
                .HasColumnName("juevesFin");
            entity.Property(e => e.JuevesFinTarde)
                .HasColumnType("datetime")
                .HasColumnName("juevesFinTarde");
            entity.Property(e => e.JuevesFinTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("juevesFinTardeTra");
            entity.Property(e => e.JuevesFinTra)
                .HasColumnType("datetime")
                .HasColumnName("juevesFinTra");
            entity.Property(e => e.JuevesFinVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("juevesFinVirDiag");
            entity.Property(e => e.JuevesFinVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("juevesFinVirDiagTarde");
            entity.Property(e => e.JuevesFinVirTra)
                .HasColumnType("datetime")
                .HasColumnName("juevesFinVirTra");
            entity.Property(e => e.JuevesFinVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("juevesFinVirTraTarde");
            entity.Property(e => e.JuevesInicio)
                .HasColumnType("datetime")
                .HasColumnName("juevesInicio");
            entity.Property(e => e.JuevesInicioTarde)
                .HasColumnType("datetime")
                .HasColumnName("juevesInicioTarde");
            entity.Property(e => e.JuevesInicioTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("juevesInicioTardeTra");
            entity.Property(e => e.JuevesInicioTra)
                .HasColumnType("datetime")
                .HasColumnName("juevesInicioTra");
            entity.Property(e => e.JuevesInicioVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("juevesInicioVirDiag");
            entity.Property(e => e.JuevesInicioVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("juevesInicioVirDiagTarde");
            entity.Property(e => e.JuevesInicioVirTra)
                .HasColumnType("datetime")
                .HasColumnName("juevesInicioVirTra");
            entity.Property(e => e.JuevesInicioVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("juevesInicioVirTraTarde");
            entity.Property(e => e.LastUpdated)
                .HasColumnType("datetime")
                .HasColumnName("lastUpdated");
            entity.Property(e => e.LastUpdatedBy)
                .HasMaxLength(50)
                .HasColumnName("lastUpdatedBy");
            entity.Property(e => e.LunesFin)
                .HasColumnType("datetime")
                .HasColumnName("lunesFin");
            entity.Property(e => e.LunesFinTarde)
                .HasColumnType("datetime")
                .HasColumnName("lunesFinTarde");
            entity.Property(e => e.LunesFinTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("lunesFinTardeTra");
            entity.Property(e => e.LunesFinTra)
                .HasColumnType("datetime")
                .HasColumnName("lunesFinTra");
            entity.Property(e => e.LunesFinVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("lunesFinVirDiag");
            entity.Property(e => e.LunesFinVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("lunesFinVirDiagTarde");
            entity.Property(e => e.LunesFinVirTra)
                .HasColumnType("datetime")
                .HasColumnName("lunesFinVirTra");
            entity.Property(e => e.LunesFinVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("lunesFinVirTraTarde");
            entity.Property(e => e.LunesInicio)
                .HasColumnType("datetime")
                .HasColumnName("lunesInicio");
            entity.Property(e => e.LunesInicioTarde)
                .HasColumnType("datetime")
                .HasColumnName("lunesInicioTarde");
            entity.Property(e => e.LunesInicioTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("lunesInicioTardeTra");
            entity.Property(e => e.LunesInicioTra)
                .HasColumnType("datetime")
                .HasColumnName("lunesInicioTra");
            entity.Property(e => e.LunesInicioVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("lunesInicioVirDiag");
            entity.Property(e => e.LunesInicioVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("lunesInicioVirDiagTarde");
            entity.Property(e => e.LunesInicioVirTra)
                .HasColumnType("datetime")
                .HasColumnName("lunesInicioVirTra");
            entity.Property(e => e.LunesInicioVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("lunesInicioVirTraTarde");
            entity.Property(e => e.MartesFin)
                .HasColumnType("datetime")
                .HasColumnName("martesFin");
            entity.Property(e => e.MartesFinTarde)
                .HasColumnType("datetime")
                .HasColumnName("martesFinTarde");
            entity.Property(e => e.MartesFinTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("martesFinTardeTra");
            entity.Property(e => e.MartesFinTra)
                .HasColumnType("datetime")
                .HasColumnName("martesFinTra");
            entity.Property(e => e.MartesFinVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("martesFinVirDiag");
            entity.Property(e => e.MartesFinVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("martesFinVirDiagTarde");
            entity.Property(e => e.MartesFinVirTra)
                .HasColumnType("datetime")
                .HasColumnName("martesFinVirTra");
            entity.Property(e => e.MartesFinVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("martesFinVirTraTarde");
            entity.Property(e => e.MartesInicio)
                .HasColumnType("datetime")
                .HasColumnName("martesInicio");
            entity.Property(e => e.MartesInicioTarde)
                .HasColumnType("datetime")
                .HasColumnName("martesInicioTarde");
            entity.Property(e => e.MartesInicioTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("martesInicioTardeTra");
            entity.Property(e => e.MartesInicioTra)
                .HasColumnType("datetime")
                .HasColumnName("martesInicioTra");
            entity.Property(e => e.MartesInicioVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("martesInicioVirDiag");
            entity.Property(e => e.MartesInicioVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("martesInicioVirDiagTarde");
            entity.Property(e => e.MartesInicioVirTra)
                .HasColumnType("datetime")
                .HasColumnName("martesInicioVirTra");
            entity.Property(e => e.MartesInicioVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("martesInicioVirTraTarde");
            entity.Property(e => e.MiercolesFin)
                .HasColumnType("datetime")
                .HasColumnName("miercolesFin");
            entity.Property(e => e.MiercolesFinTarde)
                .HasColumnType("datetime")
                .HasColumnName("miercolesFinTarde");
            entity.Property(e => e.MiercolesFinTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("miercolesFinTardeTra");
            entity.Property(e => e.MiercolesFinTra)
                .HasColumnType("datetime")
                .HasColumnName("miercolesFinTra");
            entity.Property(e => e.MiercolesFinVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("miercolesFinVirDiag");
            entity.Property(e => e.MiercolesFinVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("miercolesFinVirDiagTarde");
            entity.Property(e => e.MiercolesFinVirTra)
                .HasColumnType("datetime")
                .HasColumnName("miercolesFinVirTra");
            entity.Property(e => e.MiercolesFinVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("miercolesFinVirTraTarde");
            entity.Property(e => e.MiercolesInicio)
                .HasColumnType("datetime")
                .HasColumnName("miercolesInicio");
            entity.Property(e => e.MiercolesInicioTarde)
                .HasColumnType("datetime")
                .HasColumnName("miercolesInicioTarde");
            entity.Property(e => e.MiercolesInicioTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("miercolesInicioTardeTra");
            entity.Property(e => e.MiercolesInicioTra)
                .HasColumnType("datetime")
                .HasColumnName("miercolesInicioTra");
            entity.Property(e => e.MiercolesInicioVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("miercolesInicioVirDiag");
            entity.Property(e => e.MiercolesInicioVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("miercolesInicioVirDiagTarde");
            entity.Property(e => e.MiercolesInicioVirTra)
                .HasColumnType("datetime")
                .HasColumnName("miercolesInicioVirTra");
            entity.Property(e => e.MiercolesInicioVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("miercolesInicioVirTraTarde");
            entity.Property(e => e.PeriodoFin)
                .HasColumnType("datetime")
                .HasColumnName("periodoFin");
            entity.Property(e => e.PeriodoInicio)
                .HasColumnType("datetime")
                .HasColumnName("periodoInicio");
            entity.Property(e => e.PersonalId).HasColumnName("personalID");
            entity.Property(e => e.SabadoFin)
                .HasColumnType("datetime")
                .HasColumnName("sabadoFin");
            entity.Property(e => e.SabadoFinTarde)
                .HasColumnType("datetime")
                .HasColumnName("sabadoFinTarde");
            entity.Property(e => e.SabadoFinTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("sabadoFinTardeTra");
            entity.Property(e => e.SabadoFinTra)
                .HasColumnType("datetime")
                .HasColumnName("sabadoFinTra");
            entity.Property(e => e.SabadoFinVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("sabadoFinVirDiag");
            entity.Property(e => e.SabadoFinVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("sabadoFinVirDiagTarde");
            entity.Property(e => e.SabadoFinVirTra)
                .HasColumnType("datetime")
                .HasColumnName("sabadoFinVirTra");
            entity.Property(e => e.SabadoFinVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("sabadoFinVirTraTarde");
            entity.Property(e => e.SabadoInicio)
                .HasColumnType("datetime")
                .HasColumnName("sabadoInicio");
            entity.Property(e => e.SabadoInicioTarde)
                .HasColumnType("datetime")
                .HasColumnName("sabadoInicioTarde");
            entity.Property(e => e.SabadoInicioTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("sabadoInicioTardeTra");
            entity.Property(e => e.SabadoInicioTra)
                .HasColumnType("datetime")
                .HasColumnName("sabadoInicioTra");
            entity.Property(e => e.SabadoInicioVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("sabadoInicioVirDiag");
            entity.Property(e => e.SabadoInicioVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("sabadoInicioVirDiagTarde");
            entity.Property(e => e.SabadoInicioVirTra)
                .HasColumnType("datetime")
                .HasColumnName("sabadoInicioVirTra");
            entity.Property(e => e.SabadoInicioVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("sabadoInicioVirTraTarde");
            entity.Property(e => e.SedeId)
                .HasDefaultValue(1)
                .HasColumnName("sedeID");
            entity.Property(e => e.ViernesFin)
                .HasColumnType("datetime")
                .HasColumnName("viernesFin");
            entity.Property(e => e.ViernesFinTarde)
                .HasColumnType("datetime")
                .HasColumnName("viernesFinTarde");
            entity.Property(e => e.ViernesFinTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("viernesFinTardeTra");
            entity.Property(e => e.ViernesFinTra)
                .HasColumnType("datetime")
                .HasColumnName("viernesFinTra");
            entity.Property(e => e.ViernesFinVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("viernesFinVirDiag");
            entity.Property(e => e.ViernesFinVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("viernesFinVirDiagTarde");
            entity.Property(e => e.ViernesFinVirTra)
                .HasColumnType("datetime")
                .HasColumnName("viernesFinVirTra");
            entity.Property(e => e.ViernesFinVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("viernesFinVirTraTarde");
            entity.Property(e => e.ViernesInicio)
                .HasColumnType("datetime")
                .HasColumnName("viernesInicio");
            entity.Property(e => e.ViernesInicioTarde)
                .HasColumnType("datetime")
                .HasColumnName("viernesInicioTarde");
            entity.Property(e => e.ViernesInicioTardeTra)
                .HasColumnType("datetime")
                .HasColumnName("viernesInicioTardeTra");
            entity.Property(e => e.ViernesInicioTra)
                .HasColumnType("datetime")
                .HasColumnName("viernesInicioTra");
            entity.Property(e => e.ViernesInicioVirDiag)
                .HasColumnType("datetime")
                .HasColumnName("viernesInicioVirDiag");
            entity.Property(e => e.ViernesInicioVirDiagTarde)
                .HasColumnType("datetime")
                .HasColumnName("viernesInicioVirDiagTarde");
            entity.Property(e => e.ViernesInicioVirTra)
                .HasColumnType("datetime")
                .HasColumnName("viernesInicioVirTra");
            entity.Property(e => e.ViernesInicioVirTraTarde)
                .HasColumnType("datetime")
                .HasColumnName("viernesInicioVirTraTarde");
        });

        modelBuilder.Entity<PagosTriaje>(entity =>
        {
            entity.HasKey(e => e.PagoId);

            entity.ToTable("PagosTriaje");

            entity.Property(e => e.PagoId).HasColumnName("pagoID");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.ActualizadoPor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("actualizadoPor");
            entity.Property(e => e.ApellidoCliente)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("apellidoCliente");
            entity.Property(e => e.Atencion).HasColumnName("atencion");
            entity.Property(e => e.Bin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bin");
            entity.Property(e => e.Captura).HasColumnName("captura");
            entity.Property(e => e.CartHash)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("cart_hash");
            entity.Property(e => e.CategoriaTarjeta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("categoria_tarjeta");
            entity.Property(e => e.CitasDetalle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("citasDetalle");
            entity.Property(e => e.CiudadCliente)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ciudadCliente");
            entity.Property(e => e.ClaveOrden)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("clave_orden");
            entity.Property(e => e.CodigoAutorizacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codigo_autorizacion");
            entity.Property(e => e.CodigoMoneda)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("codigo_moneda");
            entity.Property(e => e.CodigoPaisCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codigo_paisCliente");
            entity.Property(e => e.CodigoPaisEmisor)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("codigo_paisEmisor");
            entity.Property(e => e.CodigoPaisIpCliente)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codigo_pais_ipCliente");
            entity.Property(e => e.CodigoReferencia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codigo_referencia");
            entity.Property(e => e.CodigoRespuesta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codigoRespuesta");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("correo_electronico");
            entity.Property(e => e.CorreoElectronicoToken)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("correo_electronicoToken");
            entity.Property(e => e.Cuotas).HasColumnName("cuotas");
            entity.Property(e => e.CuotasPermitidas)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cuotas_permitidas");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.DescriptorEstado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descriptor_estado");
            entity.Property(e => e.DireccionCliente)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("direccionCliente");
            entity.Property(e => e.DireccionIpCliente)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("direccion_ipCliente");
            entity.Property(e => e.Disputa).HasColumnName("disputa");
            entity.Property(e => e.DniCliente)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dni_cliente");
            entity.Property(e => e.Duplicado).HasColumnName("duplicado");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.EstadoPedido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado_pedido");
            entity.Property(e => e.FechaCaptura)
                .HasColumnType("datetime")
                .HasColumnName("fecha_captura");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaCreacionPedido)
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion_pedido");
            entity.Property(e => e.FechaCreacionToken)
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacionToken");
            entity.Property(e => e.FechaPagoPedido)
                .HasColumnType("datetime")
                .HasColumnName("fecha_pago_pedido");
            entity.Property(e => e.HuellaDispositivoCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("huella_dispositivoCliente");
            entity.Property(e => e.IdCargo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("idCargo");
            entity.Property(e => e.IdOrden)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("id_orden");
            entity.Property(e => e.IdToken)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idToken");
            entity.Property(e => e.IdTransferencia)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("id_transferencia");
            entity.Property(e => e.MarcaTarjeta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("marca_tarjeta");
            entity.Property(e => e.MensajeComercio)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("mensaje_comercio");
            entity.Property(e => e.MensajeUsuario)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("mensaje_usuario");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.MontoActual)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto_actual");
            entity.Property(e => e.MontoCuotas)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto_cuotas");
            entity.Property(e => e.MontoReembolsado)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto_reembolsado");
            entity.Property(e => e.MontoTransferido)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto_transferido");
            entity.Property(e => e.NavegadorCliente)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("navegadorCliente");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombreCliente");
            entity.Property(e => e.NombreClienteFacturacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_cliente_facturacion");
            entity.Property(e => e.NombreEmisor)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombreEmisor");
            entity.Property(e => e.NroHc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nroHC");
            entity.Property(e => e.NroTriaje)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nroTriaje");
            entity.Property(e => e.NumeroTarjeta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numero_tarjeta");
            entity.Property(e => e.NumeroTelefonoEmisor)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numero_telefonoEmisor");
            entity.Property(e => e.Pagado).HasColumnName("pagado");
            entity.Property(e => e.PagoCitaIdRef).HasColumnName("pagoCitaIdRef");
            entity.Property(e => e.PaisEmisor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("paisEmisor");
            entity.Property(e => e.PaisIpCliente)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pais_ipCliente");
            entity.Property(e => e.Productos)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("productos");
            entity.Property(e => e.PublicarCliente).HasColumnName("publicar_cliente");
            entity.Property(e => e.PuntajeFraude).HasColumnName("puntaje_fraude");
            entity.Property(e => e.RucClienteFacturacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ruc_cliente_facturacion");
            entity.Property(e => e.Sede)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sede");
            entity.Property(e => e.SimboloMonedaPedido)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("simbolo_moneda_pedido");
            entity.Property(e => e.SitioWebEmisor)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("sitio_webEmisor");
            entity.Property(e => e.Sponsor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sponsor");
            entity.Property(e => e.TelefonoCliente)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("telefonoCliente");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo");
            entity.Property(e => e.TipoBoletaFactura)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo_boleta_factura");
            entity.Property(e => e.TipoDispositivoCliente)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo_dispositivoCliente");
            entity.Property(e => e.TipoDocCliente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipoDoc_cliente");
            entity.Property(e => e.TipoRespuesta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoRespuesta");
            entity.Property(e => e.TipoTarjeta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo_tarjeta");
            entity.Property(e => e.TotalComision)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_comision");
            entity.Property(e => e.TotalImpuestosComision)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_impuestos_comision");
            entity.Property(e => e.UltimaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("ultimaActualizacion");
            entity.Property(e => e.UltimosCuatro)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ultimos_cuatro");
            entity.Property(e => e.ViaCreacionPedido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("via_creacion_pedido");
        });

        modelBuilder.Entity<UvPacienteTriajeSearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvPacienteTriajeSearch");

            entity.Property(e => e.ApellidoMaternoPaciente)
                .HasMaxLength(50)
                .HasColumnName("apellidoMaternoPaciente");
            entity.Property(e => e.ApellidoPaternoPaciente)
                .HasMaxLength(50)
                .HasColumnName("apellidoPaternoPaciente");
            entity.Property(e => e.ApoderadoEmail)
                .HasMaxLength(100)
                .HasColumnName("apoderadoEmail");
            entity.Property(e => e.ApoderadoNombre)
                .HasMaxLength(100)
                .HasColumnName("apoderadoNombre");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.EliminadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("eliminadoFecha");
            entity.Property(e => e.EmailBancoCuenta4)
                .HasMaxLength(100)
                .HasColumnName("emailBancoCuenta4");
            entity.Property(e => e.EmailRegistro)
                .HasMaxLength(100)
                .HasColumnName("emailRegistro");
            entity.Property(e => e.FechaNacimientoPaciente)
                .HasColumnType("datetime")
                .HasColumnName("fechaNacimientoPaciente");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.MotivoConsulta)
                .HasMaxLength(2000)
                .HasColumnName("motivoConsulta");
            entity.Property(e => e.NombresPaciente)
                .HasMaxLength(50)
                .HasColumnName("nombresPaciente");
            entity.Property(e => e.PacienteDni)
                .HasMaxLength(50)
                .HasColumnName("pacienteDNI");
            entity.Property(e => e.PacienteGrado)
                .HasMaxLength(50)
                .HasColumnName("pacienteGrado");
            entity.Property(e => e.ParentescoPaciente)
                .HasMaxLength(100)
                .HasColumnName("parentescoPaciente");
            entity.Property(e => e.Procedencia)
                .HasMaxLength(50)
                .HasColumnName("procedencia");
            entity.Property(e => e.RefNroHistoriaClinica).HasColumnName("refNroHistoriaClinica");
            entity.Property(e => e.RefUsuarioId)
                .HasMaxLength(450)
                .HasColumnName("refUsuarioId");
            entity.Property(e => e.SedeId).HasColumnName("sedeID");
            entity.Property(e => e.SexoPaciente).HasColumnName("sexoPaciente");
            entity.Property(e => e.TelefonoPaciente)
                .HasMaxLength(20)
                .HasColumnName("telefonoPaciente");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .HasColumnName("tipoDocumento");
            entity.Property(e => e.TriageEstadoNombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("triageEstadoNombre");
            entity.Property(e => e.TriageNoRef).HasColumnName("triageNoRef");
            entity.Property(e => e.TriageOnlineEstadoId).HasColumnName("triageOnlineEstadoID");
            entity.Property(e => e.TriageOnlineId).HasColumnName("triageOnlineID");
        });

        modelBuilder.Entity<Triage>(entity =>
        {
            entity.HasKey(e => e.TriageNo).HasName("Triage_PK");

            entity.ToTable("Triage");

            entity.HasIndex(e => e.PersonalId, "personal_FK1");

            entity.Property(e => e.TriageNo)
                .ValueGeneratedNever()
                .HasColumnName("triageNo");
            entity.Property(e => e.ActualizadoPor)
                .IsRequired()
                .HasMaxLength(101)
                .HasColumnName("actualizadoPor");
            entity.Property(e => e.ApellidoMaternoPaciente)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("apellidoMaternoPaciente");
            entity.Property(e => e.ApellidoPaternoPaciente)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("apellidoPaternoPaciente");
            entity.Property(e => e.ApoderadoNombre)
                .HasMaxLength(100)
                .HasColumnName("apoderadoNombre");
            entity.Property(e => e.ApoderadoRelacion)
                .HasMaxLength(100)
                .HasColumnName("apoderadoRelacion");
            entity.Property(e => e.ApoderadoTelCelular)
                .HasMaxLength(50)
                .HasColumnName("apoderadoTelCelular");
            entity.Property(e => e.CelularMadre)
                .HasMaxLength(20)
                .HasColumnName("celularMadre");
            entity.Property(e => e.CelularPadre)
                .HasMaxLength(20)
                .HasColumnName("celularPadre");
            entity.Property(e => e.CentroTrabajoMadre)
                .HasMaxLength(100)
                .HasColumnName("centroTrabajoMadre");
            entity.Property(e => e.CentroTrabajoPadre)
                .HasMaxLength(100)
                .HasColumnName("centroTrabajoPadre");
            entity.Property(e => e.CodigoColegio).HasColumnName("codigoColegio");
            entity.Property(e => e.Colegio)
                .HasMaxLength(100)
                .HasColumnName("colegio");
            entity.Property(e => e.Comollegocpal).HasColumnName("comollegocpal");
            entity.Property(e => e.ConsultoPor)
                .HasMaxLength(100)
                .HasColumnName("consultoPor");
            entity.Property(e => e.ContactoFechaNac)
                .HasColumnType("datetime")
                .HasColumnName("contactoFechaNac");
            entity.Property(e => e.ContactoNroDoc)
                .HasMaxLength(50)
                .HasColumnName("contactoNroDoc");
            entity.Property(e => e.ContactoSexo).HasColumnName("contactoSexo");
            entity.Property(e => e.ContactoTipoDoc)
                .HasMaxLength(50)
                .HasColumnName("contactoTipoDoc");
            entity.Property(e => e.Coordinarcoleg).HasColumnName("coordinarcoleg");
            entity.Property(e => e.CreadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("creadoFecha");
            entity.Property(e => e.CreadoPor)
                .HasMaxLength(50)
                .HasColumnName("creadoPor");
            entity.Property(e => e.DeletedDate)
                .HasColumnType("datetime")
                .HasColumnName("deletedDate");
            entity.Property(e => e.Dependiente).HasColumnName("dependiente");
            entity.Property(e => e.Descomollego)
                .HasMaxLength(1000)
                .HasColumnName("descomollego");
            entity.Property(e => e.EmaiMadre)
                .HasMaxLength(100)
                .HasColumnName("emaiMadre");
            entity.Property(e => e.EmailDeContacto)
                .HasMaxLength(100)
                .HasColumnName("emailDeContacto");
            entity.Property(e => e.EmailPadre)
                .HasMaxLength(100)
                .HasColumnName("emailPadre");
            entity.Property(e => e.EvaluaAprendizaje).HasColumnName("evaluaAprendizaje");
            entity.Property(e => e.EvaluaDisfluencia).HasColumnName("evaluaDisfluencia");
            entity.Property(e => e.EvaluaMotividad).HasColumnName("evaluaMotividad");
            entity.Property(e => e.EvaluaTrastComunicacion).HasColumnName("evaluaTrastComunicacion");
            entity.Property(e => e.EvaluaVoz).HasColumnName("evaluaVoz");
            entity.Property(e => e.EvaluarAudiologia).HasColumnName("evaluarAudiologia");
            entity.Property(e => e.EvaluarHabla).HasColumnName("evaluarHabla");
            entity.Property(e => e.EvaluarLenguajeAprendizaje).HasColumnName("evaluarLenguajeAprendizaje");
            entity.Property(e => e.EvaluarNeurologia).HasColumnName("evaluarNeurologia");
            entity.Property(e => e.EvaluarOrientacionVocacional).HasColumnName("evaluarOrientacionVocacional");
            entity.Property(e => e.EvaluarPsicologia).HasColumnName("evaluarPsicologia");
            entity.Property(e => e.EvaluarPsicomotriz).HasColumnName("evaluarPsicomotriz");
            entity.Property(e => e.Evaluaudad).HasColumnName("evaluaudad");
            entity.Property(e => e.Fecha)
                .HasColumnType("smalldatetime")
                .HasColumnName("fecha");
            entity.Property(e => e.FechaEvaluacion)
                .HasColumnType("smalldatetime")
                .HasColumnName("fechaEvaluacion");
            entity.Property(e => e.FechaNacimientoPaciente)
                .HasColumnType("smalldatetime")
                .HasColumnName("fechaNacimientoPaciente");
            entity.Property(e => e.IdColegio).HasColumnName("id_colegio");
            entity.Property(e => e.LugarEntreHermanos).HasColumnName("lugarEntreHermanos");
            entity.Property(e => e.MotivoConsulta)
                .HasMaxLength(2000)
                .HasColumnName("motivoConsulta");
            entity.Property(e => e.NombreMadre)
                .HasMaxLength(100)
                .HasColumnName("nombreMadre");
            entity.Property(e => e.NombrePadre)
                .HasMaxLength(101)
                .HasColumnName("nombrePadre");
            entity.Property(e => e.NombresPaciente)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombresPaciente");
            entity.Property(e => e.NumeroHijosMujeres).HasColumnName("numeroHijosMujeres");
            entity.Property(e => e.NumeroHijosVarones).HasColumnName("numeroHijosVarones");
            entity.Property(e => e.OcupacionMadre)
                .HasMaxLength(100)
                .HasColumnName("ocupacionMadre");
            entity.Property(e => e.OcupacionPadre)
                .HasMaxLength(50)
                .HasColumnName("ocupacionPadre");
            entity.Property(e => e.PacienteCentroEducativo)
                .HasMaxLength(100)
                .HasColumnName("pacienteCentroEducativo");
            entity.Property(e => e.PacienteCodigoDepartamento).HasColumnName("pacienteCodigoDepartamento");
            entity.Property(e => e.PacienteCodigoDistrito).HasColumnName("pacienteCodigoDistrito");
            entity.Property(e => e.PacienteCodigoPais).HasColumnName("pacienteCodigoPais");
            entity.Property(e => e.PacienteCodigoProvincia).HasColumnName("pacienteCodigoProvincia");
            entity.Property(e => e.PacienteDerivadoPor)
                .HasMaxLength(100)
                .HasColumnName("pacienteDerivadoPor");
            entity.Property(e => e.PacienteDireccion)
                .HasMaxLength(100)
                .HasColumnName("pacienteDireccion");
            entity.Property(e => e.PacienteDireccion2)
                .HasMaxLength(100)
                .HasColumnName("pacienteDireccion2");
            entity.Property(e => e.PacienteDni)
                .HasMaxLength(50)
                .HasColumnName("pacienteDNI");
            entity.Property(e => e.PacienteDomicilioTel)
                .HasMaxLength(50)
                .HasColumnName("pacienteDomicilioTel");
            entity.Property(e => e.PacienteDomicilioTelEmer)
                .HasMaxLength(50)
                .HasColumnName("pacienteDomicilioTelEmer");
            entity.Property(e => e.PacienteEmail)
                .HasMaxLength(100)
                .HasColumnName("pacienteEmail");
            entity.Property(e => e.PacienteGrado)
                .HasMaxLength(50)
                .HasColumnName("pacienteGrado");
            entity.Property(e => e.PacienteObservaciones)
                .HasMaxLength(4000)
                .HasColumnName("pacienteObservaciones");
            entity.Property(e => e.PacienteProfEmail)
                .HasMaxLength(100)
                .HasColumnName("pacienteProfEmail");
            entity.Property(e => e.PacienteProfNombre)
                .HasMaxLength(100)
                .HasColumnName("pacienteProfNombre");
            entity.Property(e => e.PacienteProfTel)
                .HasMaxLength(500)
                .HasColumnName("pacienteProfTel");
            entity.Property(e => e.PacienteTel)
                .HasMaxLength(50)
                .HasColumnName("pacienteTel");
            entity.Property(e => e.PacienteTratAnteriores)
                .HasMaxLength(4000)
                .HasColumnName("pacienteTratAnteriores");
            entity.Property(e => e.PacienteViveCon)
                .HasMaxLength(100)
                .HasColumnName("pacienteViveCon");
            entity.Property(e => e.PersonalId).HasColumnName("personalID");
            entity.Property(e => e.Presuncion).HasColumnName("presuncion");
            entity.Property(e => e.Responsable).HasColumnName("responsable");
            entity.Property(e => e.SedeId)
                .HasDefaultValue(1)
                .HasColumnName("sedeID");
            entity.Property(e => e.SexoPaciente).HasColumnName("sexoPaciente");
            entity.Property(e => e.TelefonoMadre)
                .HasMaxLength(20)
                .HasColumnName("telefonoMadre");
            entity.Property(e => e.TelefonoPaciente)
                .HasMaxLength(20)
                .HasColumnName("telefonoPaciente");
            entity.Property(e => e.TelefonoPadre)
                .HasMaxLength(20)
                .HasColumnName("telefonoPadre");
            entity.Property(e => e.Tipo)
                .HasDefaultValue(0)
                .HasColumnName("tipo");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .HasColumnName("tipoDocumento");
            entity.Property(e => e.TriageTipo).HasColumnName("triageTipo");
            entity.Property(e => e.UltimaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("ultimaActualizacion");
            entity.Property(e => e.Usopublicidad).HasColumnName("usopublicidad");
        });

        modelBuilder.Entity<EvaluacionCitum>(entity =>
        {
            entity.HasKey(e => e.EvaluacionCitaId);

            entity.Property(e => e.EvaluacionCitaId).HasColumnName("evaluacionCitaID");
            entity.Property(e => e.ActualizadoPor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("actualizadoPor");
            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("categoria");
            entity.Property(e => e.CategoriaNro).HasColumnName("categoriaNro");
            entity.Property(e => e.CitaPara).HasColumnName("citaPara");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Duracion).HasColumnName("duracion");
            entity.Property(e => e.EliminadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("eliminadoFecha");
            entity.Property(e => e.EvaluacionAreaId).HasColumnName("evaluacionAreaID");
            entity.Property(e => e.EvaluacionId).HasColumnName("evaluacionID");
            entity.Property(e => e.EvaluacionPublicaId).HasColumnName("evaluacionPublicaID");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("imagenUrl");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.TipoCitaId).HasColumnName("tipoCitaID");
            entity.Property(e => e.UltimaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("ultimaActualizacion");
        });

        modelBuilder.Entity<UvEspecilidadesPublica>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvEspecilidadesPublicas");

            entity.Property(e => e.Area)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.Especialidad).HasMaxLength(100);
            entity.Property(e => e.EspecialidadId).HasColumnName("EspecialidadID");
            entity.Property(e => e.EspecialidadPublica).HasMaxLength(100);
            entity.Property(e => e.EspecialidadVariable).HasMaxLength(20);
        });

        modelBuilder.Entity<UvEvaluacionesPublica>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvEvaluacionesPublicas");

            entity.Property(e => e.Area)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.DescripcionEspecialidad).HasColumnName("descripcionEspecialidad");
            entity.Property(e => e.DescripcionEvaluacion).HasColumnName("descripcionEvaluacion");
            entity.Property(e => e.Especialidad).HasMaxLength(100);
            entity.Property(e => e.EspecialidadId).HasColumnName("EspecialidadID");
            entity.Property(e => e.EspecialidadPublica).HasMaxLength(100);
            entity.Property(e => e.Evaluacion).HasMaxLength(50);
            entity.Property(e => e.EvaluacionId).HasColumnName("evaluacionID");
            entity.Property(e => e.EvaluacionPublica)
                .HasMaxLength(100)
                .HasColumnName("evaluacionPublica");
            entity.Property(e => e.EvaluacionPublicaId).HasColumnName("evaluacionPublicaID");
            entity.Property(e => e.EvaluacionPublicaIdpadre).HasColumnName("evaluacionPublicaIDPadre");
            entity.Property(e => e.EvaluacionPublicaNroOrden).HasColumnName("evaluacionPublicaNroOrden");
            entity.Property(e => e.Variable).HasMaxLength(20);
        });

        modelBuilder.Entity<UvCitasPublica>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvCitasPublicas");

            entity.Property(e => e.CitaPara).HasColumnName("citaPara");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Duracion).HasColumnName("duracion");
            entity.Property(e => e.EvaluacionCitaId).HasColumnName("evaluacionCitaID");
            entity.Property(e => e.EvaluacionId).HasColumnName("evaluacionID");
            entity.Property(e => e.EvaluacionPublicaId).HasColumnName("evaluacionPublicaID");
            entity.Property(e => e.EvaluacionPublicaNroOrden).HasColumnName("evaluacionPublicaNroOrden");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("imagenUrl");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.TipoCitaId).HasColumnName("tipoCitaID");
        });

        modelBuilder.Entity<UvEspecilidadesPublica>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvEspecilidadesPublicas");

            entity.Property(e => e.Area)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.Especialidad).HasMaxLength(100);
            entity.Property(e => e.EspecialidadId).HasColumnName("EspecialidadID");
            entity.Property(e => e.EspecialidadPublica).HasMaxLength(100);
            entity.Property(e => e.EspecialidadVariable).HasMaxLength(20);
        });

        modelBuilder.Entity<UvHistoriaDiagnosticoBusqueda2>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("UvHistoriaDiagnosticoBusqueda2");

            entity.Property(e => e.Especialista)
                .IsRequired()
                .HasMaxLength(101);
            entity.Property(e => e.Evaluacion)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("evaluacion");
            entity.Property(e => e.FechaFin)
                .HasColumnType("smalldatetime")
                .HasColumnName("fechaFin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("smalldatetime")
                .HasColumnName("fechaInicio");
            entity.Property(e => e.FinalizarNombre)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("finalizarNombre");
            entity.Property(e => e.FinalizarTipoId).HasColumnName("finalizarTipoID");
            entity.Property(e => e.HistorialEstadoId).HasColumnName("historialEstadoID");
            entity.Property(e => e.HistorialPacienteId).HasColumnName("historialPacienteID");
            entity.Property(e => e.InfBorradorCorregir1)
                .HasMaxLength(100)
                .HasColumnName("infBorradorCorregir1");
            entity.Property(e => e.InfBorradorCorregir1Entrega)
                .HasMaxLength(100)
                .HasColumnName("infBorradorCorregir1Entrega");
            entity.Property(e => e.InfBorradorCorregir1EntregaFecha)
                .HasColumnType("datetime")
                .HasColumnName("infBorradorCorregir1EntregaFecha");
            entity.Property(e => e.InfBorradorCorregir1Fecha)
                .HasColumnType("datetime")
                .HasColumnName("infBorradorCorregir1Fecha");
            entity.Property(e => e.InfBorradorCorregir2)
                .HasMaxLength(100)
                .HasColumnName("infBorradorCorregir2");
            entity.Property(e => e.InfBorradorCorregir2Entrega)
                .HasMaxLength(100)
                .HasColumnName("infBorradorCorregir2Entrega");
            entity.Property(e => e.InfBorradorCorregir2EntregaFecha)
                .HasColumnType("datetime")
                .HasColumnName("infBorradorCorregir2EntregaFecha");
            entity.Property(e => e.InfBorradorCorregir2Fecha)
                .HasColumnType("datetime")
                .HasColumnName("infBorradorCorregir2Fecha");
            entity.Property(e => e.InfRevisadoEntregaRecepcion)
                .HasColumnType("datetime")
                .HasColumnName("infRevisadoEntregaRecepcion");
            entity.Property(e => e.InformeBorrador)
                .HasMaxLength(100)
                .HasColumnName("informeBorrador");
            entity.Property(e => e.InformeBorradorEntrega)
                .HasColumnType("datetime")
                .HasColumnName("informeBorradorEntrega");
            entity.Property(e => e.InformeBorradorProgramada)
                .HasColumnType("datetime")
                .HasColumnName("informeBorradorProgramada");
            entity.Property(e => e.InformeIndividual)
                .HasMaxLength(100)
                .HasColumnName("informeIndividual");
            entity.Property(e => e.InformeRevisado)
                .HasMaxLength(100)
                .HasColumnName("informeRevisado");
            entity.Property(e => e.InformeRevisadoEntrega)
                .HasColumnType("datetime")
                .HasColumnName("informeRevisadoEntrega");
            entity.Property(e => e.InformeRevisadoProgramada)
                .HasColumnType("datetime")
                .HasColumnName("informeRevisadoProgramada");
            entity.Property(e => e.NombreEstado)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombreEstado");
            entity.Property(e => e.NumeroHistoriaClinica).HasColumnName("numeroHistoriaClinica");
            entity.Property(e => e.NumeroRegistro).HasColumnName("numeroRegistro");
            entity.Property(e => e.PersonalId).HasColumnName("personalID");
            entity.Property(e => e.Revaluar)
                .HasColumnType("datetime")
                .HasColumnName("revaluar");
            entity.Property(e => e.TerapiaAsociada)
                .HasMaxLength(50)
                .HasColumnName("terapiaAsociada");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
        });

        modelBuilder.Entity<DetalleReservaTratamiento>(entity =>
        {
            entity.HasKey(e => e.DetalleReservaTratamientoId).HasName("PK__DetalleR__A556F1DAE0404E53");

            entity.ToTable("DetalleReservaTratamiento");

            entity.Property(e => e.DetalleReservaTratamientoId).HasColumnName("DetalleReservaTratamientoID");
            entity.Property(e => e.DeleteBy)
                .HasMaxLength(50)
                .HasColumnName("deleteBy");
            entity.Property(e => e.DeleteDate)
                .HasColumnType("datetime")
                .HasColumnName("deleteDate");
            entity.Property(e => e.ReservaTratamientoId).HasColumnName("reservaTratamientoID");

            entity.HasOne(d => d.ReservaTratamiento).WithMany(p => p.DetalleReservaTratamientos)
                .HasForeignKey(d => d.ReservaTratamientoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleReservaTratamiento_Reserva");
        });

        modelBuilder.Entity<ReservaTratamiento>(entity =>
        {
            entity.HasKey(e => e.ReservaTratamientoId).HasName("PK__ReservaT__CE1FF6EE22B59077");

            entity.ToTable("ReservaTratamiento");

            entity.Property(e => e.ReservaTratamientoId).HasColumnName("reservaTratamientoID");
            entity.Property(e => e.ActualizadoPor)
                .HasMaxLength(50)
                .HasColumnName("actualizadoPor");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.DeleteBy)
                .HasMaxLength(50)
                .HasColumnName("deleteBy");
            entity.Property(e => e.DeleteDate)
                .HasColumnType("datetime")
                .HasColumnName("deleteDate");
            entity.Property(e => e.EmailRegistro)
                .HasMaxLength(100)
                .HasColumnName("emailRegistro");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .HasColumnName("observaciones");
            entity.Property(e => e.OrientacionMedio)
                .HasMaxLength(100)
                .HasColumnName("orientacionMedio");
            entity.Property(e => e.PersonalId).HasColumnName("personalID");
            entity.Property(e => e.RefNroHistoriaClinica).HasColumnName("refNroHistoriaClinica");
            entity.Property(e => e.RefUsuarioId)
                .HasMaxLength(450)
                .HasColumnName("refUsuarioId");
            entity.Property(e => e.ReservaTratamientoEstadoId).HasColumnName("reservaTratamientoEstadoID");
            entity.Property(e => e.SedeId).HasColumnName("sedeID");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
            entity.Property(e => e.UltimaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("ultimaActualizacion");
        });

        modelBuilder.Entity<UvReservaTratamientoSearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvReservaTratamientoSearch");

            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .HasColumnName("apellidoMaterno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .HasColumnName("apellidoPaterno");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.DeleteDate)
                .HasColumnType("datetime")
                .HasColumnName("deleteDate");
            entity.Property(e => e.EmailRegistro)
                .HasMaxLength(100)
                .HasColumnName("emailRegistro");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasColumnName("estado");
            entity.Property(e => e.HistoriaClinica).HasColumnName("historiaClinica");
            entity.Property(e => e.Nombres)
                .HasMaxLength(50)
                .HasColumnName("nombres");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .HasColumnName("observaciones");
            entity.Property(e => e.OrientacionMedio)
                .HasMaxLength(100)
                .HasColumnName("orientacionMedio");
            entity.Property(e => e.PersonalId).HasColumnName("personalID");
            entity.Property(e => e.ReservaTratamientoEstadoId).HasColumnName("reservaTratamientoEstadoID");
            entity.Property(e => e.ReservaTratamientoId).HasColumnName("reservaTratamientoID");
            entity.Property(e => e.Sede)
                .HasMaxLength(9)
                .IsUnicode(false);
            entity.Property(e => e.Terapia)
                .HasMaxLength(4000)
                .HasColumnName("terapia");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
            entity.Property(e => e.UsuarioId)
                .HasMaxLength(450)
                .HasColumnName("usuarioId");
        });

        modelBuilder.Entity<UvReservaDiagnosticoSearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvReservaDiagnosticoSearch");

            entity.Property(e => e.ApellidoMaternoPaciente)
                .HasMaxLength(50)
                .HasColumnName("apellidoMaternoPaciente");
            entity.Property(e => e.ApellidoPaternoPaciente)
                .HasMaxLength(50)
                .HasColumnName("apellidoPaternoPaciente");
            entity.Property(e => e.ApoderadoEmail)
                .HasMaxLength(100)
                .HasColumnName("apoderadoEmail");
            entity.Property(e => e.ApoderadoNombre)
                .HasMaxLength(100)
                .HasColumnName("apoderadoNombre");
            entity.Property(e => e.CreadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("creadoFecha");
            entity.Property(e => e.CreadoPor)
                .HasMaxLength(50)
                .HasColumnName("creadoPor");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Documento)
                .HasMaxLength(50)
                .HasColumnName("documento");
            entity.Property(e => e.EliminadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("eliminadoFecha");
            entity.Property(e => e.EmailRegistro)
                .HasMaxLength(100)
                .HasColumnName("emailRegistro");
            entity.Property(e => e.EstadoPago)
                .HasMaxLength(10)
                .HasColumnName("estadoPago");
            entity.Property(e => e.FechaPago).HasColumnType("datetime");
            entity.Property(e => e.FechaTriajeOnline).HasColumnType("datetime");
            entity.Property(e => e.HistoriaClinica).HasColumnName("historiaClinica");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.NombreCompleto).HasMaxLength(104);
            entity.Property(e => e.NombresPaciente)
                .HasMaxLength(50)
                .HasColumnName("nombresPaciente");
            entity.Property(e => e.PagoCitaId).HasColumnName("pagoCitaId");
            entity.Property(e => e.SedeId).HasColumnName("sedeID");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .HasColumnName("tipoDocumento");
            entity.Property(e => e.TriageEstadoNombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("triageEstadoNombre");
            entity.Property(e => e.TriageNoRef).HasColumnName("triageNoRef");
            entity.Property(e => e.TriageOnlineEstadoId).HasColumnName("triageOnlineEstadoID");
            entity.Property(e => e.TriajeOnlineId).HasColumnName("triajeOnlineId");
            entity.Property(e => e.UsuarioId)
                .HasMaxLength(450)
                .HasColumnName("usuarioId");
        });


        OnModelCreatingGeneratedProcedures(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}