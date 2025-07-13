
using Entity.CentralModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DA.CentralContext;

public partial class CPALCentralContext : IdentityDbContext
{
    public CPALCentralContext(DbContextOptions<CPALCentralContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<Distrito> Distritos { get; set; }

    public virtual DbSet<Pais> Paises { get; set; }

    public virtual DbSet<Provincia> Provincia { get; set; }

    public virtual DbSet<PerfilUsuario> PerfilUsuarios { get; set; }

    public virtual DbSet<ConfiguracionSistema> ConfiguracionSistemas { get; set; }

    public virtual DbSet<EmailQueue> EmailQueues { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<PagoExternoVoucher> PagoExternoVouchers { get; set; }

    public virtual DbSet<PagosDetalle> PagosDetalles { get; set; }

    public virtual DbSet<PagosExterno> PagosExternos { get; set; }

    public virtual DbSet<UvConfiguracionVariable> UvConfiguracionVariables { get; set; }

    public virtual DbSet<UvAspnetUserRole> UvAspnetUserRoles { get; set; }

    public virtual DbSet<PortalUsuarioLog> PortalUsuarioLogs { get; set; }

    public virtual DbSet<UsuariosV2> UsuariosV2s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Habilita el registro de datos sensibles
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<PerfilUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PerfilUs__3214EC07C076E77F");

            entity.ToTable("PerfilUsuario");

            entity.Property(e => e.ActualizadoPor)
                .HasMaxLength(50)
                .HasColumnName("actualizadoPor");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoPaterno");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoMaterno");
            entity.Property(e => e.Sexo)
                .HasColumnName("sexo");
            entity.Property(e => e.CreadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("creadoFecha");
            entity.Property(e => e.Departamento).HasColumnName("departamento");
            entity.Property(e => e.Dirección)
                .HasMaxLength(300)
                .HasColumnName("dirección");
            entity.Property(e => e.Dni)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("dni");
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("datetime")
                .HasColumnName("fechaNacimiento");
            entity.Property(e => e.ImagenExtension)
                .HasMaxLength(100)
                .HasColumnName("imagenExtension");
            entity.Property(e => e.ImagenExternal)
                .HasMaxLength(100)
                .HasColumnName("imagenExternal");
            entity.Property(e => e.ImagenInternal)
                .HasMaxLength(100)
                .HasColumnName("imagenInternal");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombres");
            entity.Property(e => e.Pais).HasColumnName("pais");
            entity.Property(e => e.RefAspNetUser)
                .HasMaxLength(450)
                .HasColumnName("refAspNetUser");
            entity.Property(e => e.Region).HasColumnName("region");
            entity.Property(e => e.Telefono)
                .HasMaxLength(12)
                .HasColumnName("telefono");
            entity.Property(e => e.TipoDoc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoDoc");
            entity.Property(e => e.UrlImagen).HasColumnName("urlImagen");
        });

        modelBuilder.Entity<ConfiguracionSistema>(entity =>
        {
            entity.HasKey(e => e.ConfiguracionSistemaId).HasName("ConfiguracionSistema_PK");

            entity.ToTable("ConfiguracionSistema");

            entity.Property(e => e.ConfiguracionSistemaId).HasColumnName("configuracionSistemaID");
            entity.Property(e => e.BoletaNumero).HasColumnName("boletaNumero");
            entity.Property(e => e.BoletaNumeroAntares).HasColumnName("boletaNumeroAntares");
            entity.Property(e => e.BoletaNumeroIniSede2).HasColumnName("boletaNumeroIniSede2");
            entity.Property(e => e.BoletaSerie).HasColumnName("boletaSerie");
            entity.Property(e => e.BoletaSerieAntares).HasColumnName("boletaSerieAntares");
            entity.Property(e => e.BoletaSerieSede2).HasColumnName("boletaSerieSede2");
            entity.Property(e => e.BoletaSerieWiese).HasColumnName("boletaSerieWiese");
            entity.Property(e => e.BoletaUltimoNumero).HasDefaultValue(1);
            entity.Property(e => e.BoletaUltimoNumeroSede2).HasColumnName("boletaUltimoNumeroSede2");
            entity.Property(e => e.DefaultPrinter).HasMaxLength(50);
            entity.Property(e => e.DefaultPrinterAntares).HasMaxLength(50);
            entity.Property(e => e.EmailAdministrador)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("emailAdministrador");
            entity.Property(e => e.FacturaNumero).HasColumnName("facturaNumero");
            entity.Property(e => e.FacturaNumeroAntares).HasColumnName("facturaNumeroAntares");
            entity.Property(e => e.FacturaNumeroIniSede2).HasColumnName("facturaNumeroIniSede2");
            entity.Property(e => e.FacturaSerie).HasColumnName("facturaSerie");
            entity.Property(e => e.FacturaSerieAntares).HasColumnName("facturaSerieAntares");
            entity.Property(e => e.FacturaSerieSede2).HasColumnName("facturaSerieSede2");
            entity.Property(e => e.FacturaSerieWiese).HasColumnName("facturaSerieWiese");
            entity.Property(e => e.FacturaUltimoNumero)
                .HasDefaultValue(1)
                .HasColumnName("facturaUltimoNumero");
            entity.Property(e => e.FacturaUltimoNumeroAntares).HasColumnName("facturaUltimoNumeroAntares");
            entity.Property(e => e.FacturaUltimoNumeroSede2).HasColumnName("facturaUltimoNumeroSede2");
            entity.Property(e => e.FacturaUltimoNumeroWiese).HasColumnName("facturaUltimoNumeroWiese");
            entity.Property(e => e.HistoriaClinicaNumero).HasColumnName("historiaClinicaNumero");
            entity.Property(e => e.HistoriaClinicaUltimoNumero).HasColumnName("historiaClinicaUltimoNumero");
            entity.Property(e => e.Igv).HasColumnName("igv");
            entity.Property(e => e.IgvSede2).HasColumnName("igvSede2");
            entity.Property(e => e.NcBoletaSerieSede2).HasColumnName("ncBoletaSerieSede2");
            entity.Property(e => e.NcBoletaUltimoNumeroSede2).HasColumnName("ncBoletaUltimoNumeroSede2");
            entity.Property(e => e.NcFacturaSerieSede2).HasColumnName("ncFacturaSerieSede2");
            entity.Property(e => e.NcFacturaUltimoNumeroSede2).HasColumnName("ncFacturaUltimoNumeroSede2");
            entity.Property(e => e.TriageNumero).HasColumnName("triageNumero");
        });

        modelBuilder.Entity<EmailQueue>(entity =>
        {
            entity.HasKey(e => e.EmailQueueId).HasName("EmailQueue_PK");

            entity.ToTable("EmailQueue");

            entity.Property(e => e.EmailQueueId).HasColumnName("emailQueueID");
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.EmailBcc)
                .HasMaxLength(500)
                .HasColumnName("emailBcc");
            entity.Property(e => e.EmailBody).HasColumnName("emailBody");
            entity.Property(e => e.EmailCc)
                .HasMaxLength(500)
                .HasColumnName("emailCc");
            entity.Property(e => e.EmailFiles)
                .HasMaxLength(500)
                .HasColumnName("emailFiles");
            entity.Property(e => e.EmailFrom)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("emailFrom");
            entity.Property(e => e.EmailFromDisplay)
                .HasMaxLength(50)
                .HasColumnName("emailFromDisplay");
            entity.Property(e => e.EmailSubject)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("emailSubject");
            entity.Property(e => e.EmailTo)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("emailTo");
            entity.Property(e => e.QueueDesc)
                .HasMaxLength(500)
                .HasColumnName("queueDesc");
            entity.Property(e => e.QueueSendDate)
                .HasColumnType("datetime")
                .HasColumnName("queueSendDate");
            entity.Property(e => e.QueueStatus)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("queueStatus");
            entity.Property(e => e.ReplyTo)
                .HasMaxLength(500)
                .HasColumnName("replyTo");
            entity.Property(e => e.SmtpPort)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("smtpPort");
            entity.Property(e => e.SmtpPwd)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("smtpPwd");
            entity.Property(e => e.SmtpServer)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("smtpServer");
            entity.Property(e => e.SmtpSsl).HasColumnName("smtpSsl");
            entity.Property(e => e.SmtpUser)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("smtpUser");
            entity.Property(e => e.SystemId).HasColumnName("systemID");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("Pagos_PK");

            entity.HasIndex(e => e.NumeroHistoriaClinica, "PAGOS_INDX_HISTORIA");

            entity.Property(e => e.PagoId).HasColumnName("pagoID");
            entity.Property(e => e.ActualizadoPor)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("actualizadoPor");
            entity.Property(e => e.AreaId).HasColumnName("areaID");
            entity.Property(e => e.EvaluacionId).HasColumnName("evaluacionID");
            entity.Property(e => e.FacturaIdref).HasColumnName("facturaIDRef");
            entity.Property(e => e.FechaPago).HasColumnType("datetime");
            entity.Property(e => e.HistorialPacienteId).HasColumnName("historialPacienteID");
            entity.Property(e => e.NumeroHistoriaClinica).HasColumnName("numeroHistoriaClinica");
            entity.Property(e => e.OtrosPagos)
                .HasColumnType("money")
                .HasColumnName("otrosPagos");
            entity.Property(e => e.PersonalId).HasColumnName("personalID");
            entity.Property(e => e.Referencia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("referencia");
            entity.Property(e => e.SedeId)
                .HasDefaultValue(1)
                .HasColumnName("sedeID");
            entity.Property(e => e.UltimaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("ultimaActualizacion");
        });

        modelBuilder.Entity<PagoExternoVoucher>(entity =>
        {
            entity.ToTable("PagoExternoVoucher");

            entity.Property(e => e.PagoExternoVoucherId).HasColumnName("pagoExternoVoucherID");
            entity.Property(e => e.ActualizadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("actualizadoFecha");
            entity.Property(e => e.ActualizadoPor)
                .HasMaxLength(50)
                .HasColumnName("actualizadoPor");
            entity.Property(e => e.Agencia)
                .HasMaxLength(100)
                .HasColumnName("agencia");
            entity.Property(e => e.AreaNombre)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("areaNombre");
            entity.Property(e => e.CreadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("creadoFecha");
            entity.Property(e => e.CreadoPor)
                .HasMaxLength(50)
                .HasColumnName("creadoPor");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FechaPago)
                .HasColumnType("datetime")
                .HasColumnName("fechaPago");
            entity.Property(e => e.FileExtension)
                .IsRequired()
                .HasMaxLength(5)
                .HasColumnName("fileExtension");
            entity.Property(e => e.FileExternal)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("fileExternal");
            entity.Property(e => e.FileInternal)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("fileInternal");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(100)
                .HasColumnName("nombreCompleto");
            entity.Property(e => e.NroOperacion)
                .HasMaxLength(50)
                .HasColumnName("nroOperacion");
            entity.Property(e => e.PagoBanco)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("pagoBanco");
            entity.Property(e => e.PagoDescripcion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("pagoDescripcion");
            entity.Property(e => e.PagoDireccion)
                .HasMaxLength(200)
                .HasColumnName("pagoDireccion");
            entity.Property(e => e.PagoDocNumero)
                .HasMaxLength(50)
                .HasColumnName("pagoDocNumero");
            entity.Property(e => e.PagoHc).HasColumnName("pagoHC");
            entity.Property(e => e.PagoMoneda)
                .HasMaxLength(3)
                .HasColumnName("pagoMoneda");
            entity.Property(e => e.PagoMonedaAbbrev)
                .HasMaxLength(3)
                .HasColumnName("pagoMonedaAbbrev");
            entity.Property(e => e.PagoMonto)
                .HasColumnType("money")
                .HasColumnName("pagoMonto");
            entity.Property(e => e.PagoRazonSocial)
                .HasMaxLength(200)
                .HasColumnName("pagoRazonSocial");
            entity.Property(e => e.PagoTipo)
                .HasMaxLength(10)
                .HasColumnName("pagoTipo");
            entity.Property(e => e.PagoTipoDoc)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("pagoTipoDoc");
            entity.Property(e => e.PagoVoucherEstado)
                .HasMaxLength(10)
                .HasColumnName("pagoVoucherEstado");
            entity.Property(e => e.RefFacturaId).HasColumnName("refFacturaID");
            entity.Property(e => e.RefPagoId).HasColumnName("refPagoID");
            entity.Property(e => e.SedeId).HasColumnName("sedeID");
        });

        modelBuilder.Entity<PagosDetalle>(entity =>
        {
            entity.HasKey(e => e.PagoDetalleId).HasName("PagosDetalle_PK");

            entity.ToTable("PagosDetalle");

            entity.HasIndex(e => e.CitasPacienteId, "PagosDetalle_citasPacienteID_INDX");

            entity.HasIndex(e => e.PagoId, "PagosDetalle_pagoID_INDX");

            entity.Property(e => e.PagoDetalleId).HasColumnName("pagoDetalleID");
            entity.Property(e => e.CitasPacienteId).HasColumnName("citasPacienteID");
            entity.Property(e => e.DescuentoId)
                .HasDefaultValue(1)
                .HasColumnName("descuentoID");
            entity.Property(e => e.PagoCantidad).HasColumnName("pagoCantidad");
            entity.Property(e => e.PagoDescripcion)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("pagoDescripcion");
            entity.Property(e => e.PagoDescuento).HasColumnName("pagoDescuento");
            entity.Property(e => e.PagoId).HasColumnName("pagoID");
            entity.Property(e => e.PagoPrecio)
                .HasColumnType("money")
                .HasColumnName("pagoPrecio");
            entity.Property(e => e.PagoPrecioOtros)
                .HasColumnType("money")
                .HasColumnName("pagoPrecioOtros");
            entity.Property(e => e.ReferenciaDet)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("referenciaDet");

            entity.HasOne(d => d.Pago).WithMany(p => p.PagosDetalles)
                .HasForeignKey(d => d.PagoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Pagos_PagosDetalle_FK1");
        });

        modelBuilder.Entity<PagosExterno>(entity =>
        {
            entity.HasKey(e => e.PagoExternoId);

            entity.Property(e => e.PagoExternoId).HasColumnName("pagoExternoID");
            entity.Property(e => e.ActualizadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("actualizadoFecha");
            entity.Property(e => e.ActualizadoPor)
                .HasMaxLength(50)
                .HasColumnName("actualizadoPor");
            entity.Property(e => e.AreaNombre)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("areaNombre");
            entity.Property(e => e.CreadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("creadoFecha");
            entity.Property(e => e.CreadoPor)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("creadoPor");
            entity.Property(e => e.DocNumero)
                .HasMaxLength(50)
                .HasColumnName("docNumero");
            entity.Property(e => e.DocuDireccion)
                .HasMaxLength(200)
                .HasColumnName("docuDireccion");
            entity.Property(e => e.DocuRazonSocial)
                .HasMaxLength(100)
                .HasColumnName("docuRazonSocial");
            entity.Property(e => e.DocuRuc)
                .HasMaxLength(50)
                .HasColumnName("docuRUC");
            entity.Property(e => e.DocuTipo)
                .HasMaxLength(10)
                .HasColumnName("docuTipo");
            entity.Property(e => e.MonedaAbrev)
                .HasMaxLength(5)
                .HasColumnName("monedaAbrev");
            entity.Property(e => e.MonedaCodigo)
                .HasMaxLength(5)
                .HasColumnName("monedaCodigo");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(100)
                .HasColumnName("nombreCompleto");
            entity.Property(e => e.PagoExternoBeneficiario).HasColumnName("pagoExternoBeneficiario");
            entity.Property(e => e.PagoExternoBeneficiarioDet).HasColumnName("pagoExternoBeneficiarioDet");
            entity.Property(e => e.PagoExternoEstado)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("pagoExternoEstado");
            entity.Property(e => e.PagoExternoMonto)
                .HasColumnType("money")
                .HasColumnName("pagoExternoMonto");
            entity.Property(e => e.RefFacturaId).HasColumnName("refFacturaID");
            entity.Property(e => e.RefPagoId).HasColumnName("refPagoID");
            entity.Property(e => e.RefPagoTipo)
                .HasMaxLength(5)
                .HasColumnName("refPagoTipo");
            entity.Property(e => e.ReferenceCode)
                .HasMaxLength(50)
                .HasColumnName("reference_code");
            entity.Property(e => e.SedeId).HasColumnName("sedeID");
            entity.Property(e => e.TokenCardBrand)
                .HasMaxLength(100)
                .HasColumnName("token_card_brand");
            entity.Property(e => e.TokenCardNumber)
                .HasMaxLength(100)
                .HasColumnName("token_card_number");
            entity.Property(e => e.TokenCardType)
                .HasMaxLength(100)
                .HasColumnName("token_card_type");
            entity.Property(e => e.TokenClientBrowser)
                .HasMaxLength(200)
                .HasColumnName("token_client_browser");
            entity.Property(e => e.TokenClientDeviceType)
                .HasMaxLength(100)
                .HasColumnName("token_client_deviceType");
            entity.Property(e => e.TokenClientIp)
                .HasMaxLength(100)
                .HasColumnName("token_client_ip");
            entity.Property(e => e.TokenEmail)
                .HasMaxLength(100)
                .HasColumnName("token_email");
            entity.Property(e => e.TokenId)
                .HasMaxLength(100)
                .HasColumnName("token_id");
            entity.Property(e => e.TokenInstallments)
                .HasMaxLength(100)
                .HasColumnName("token_installments");
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(100)
                .HasColumnName("transactionCode");
            entity.Property(e => e.TransactionDesc)
                .HasMaxLength(200)
                .HasColumnName("transactionDesc");
            entity.Property(e => e.TransactionResponse)
                .HasMaxLength(4000)
                .HasColumnName("transactionResponse");
        });

        modelBuilder.Entity<UvConfiguracionVariable>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvConfiguracionVariables");

            entity.Property(e => e.ConfigData)
                .IsRequired()
                .HasMaxLength(2000)
                .HasColumnName("configData");
            entity.Property(e => e.ConfigNombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("configNombre");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.CodigoDepartamento).HasName("Departamento_PK");

            entity.ToTable("Departamento");

            entity.HasIndex(e => e.CodigoPais, "pais_FK1");

            entity.Property(e => e.CodigoDepartamento)
                .ValueGeneratedNever()
                .HasColumnName("codigoDepartamento");
            entity.Property(e => e.CodigoPais).HasColumnName("codigoPais");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombre");

            entity.HasOne(d => d.CodigoPaisNavigation).WithMany(p => p.Departamentos)
                .HasForeignKey(d => d.CodigoPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Paises_Departamento_FK1");
        });

        modelBuilder.Entity<Distrito>(entity =>
        {
            entity.HasKey(e => e.CodigoDistrito).HasName("Distrito_PK");

            entity.ToTable("Distrito");

            entity.HasIndex(e => e.CodigoProvincia, "provincia_FK1");

            entity.Property(e => e.CodigoDistrito)
                .ValueGeneratedNever()
                .HasColumnName("codigoDistrito");
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(50)
                .HasColumnName("codigoPostal");
            entity.Property(e => e.CodigoProvincia).HasColumnName("codigoProvincia");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombre");

            entity.HasOne(d => d.CodigoProvinciaNavigation).WithMany(p => p.Distritos)
                .HasForeignKey(d => d.CodigoProvincia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Provincia_Distrito_FK1");
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.HasKey(e => e.CodigoPais).HasName("paises_PK");

            entity.Property(e => e.CodigoPais)
                .ValueGeneratedNever()
                .HasColumnName("codigoPais");
            entity.Property(e => e.Abreviacion)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("abreviacion");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => e.CodigoProvincia).HasName("Provincia_PK");

            entity.HasIndex(e => e.CodigoDepartamento, "departamento_FK1");

            entity.Property(e => e.CodigoProvincia)
                .ValueGeneratedNever()
                .HasColumnName("codigoProvincia");
            entity.Property(e => e.CodigoDepartamento).HasColumnName("codigoDepartamento");
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombre");

            entity.HasOne(d => d.CodigoDepartamentoNavigation).WithMany(p => p.Provincia)
                .HasForeignKey(d => d.CodigoDepartamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Departamento_Provincia_FK1");
        });

        modelBuilder.Entity<UvAspnetUserRole>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("uvASPnetUserRole");

            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellidoPaterno");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellidoMaterno");
            entity.Property(e => e.Sexo)
                .HasColumnName("sexo");
            entity.Property(e => e.Dni)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("dni");
            entity.Property(e => e.TipoDoc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoDoc");
            entity.Property(e => e.CreadoFecha)
                .HasColumnType("datetime")
                .HasColumnName("creadoFecha");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("datetime")
                .HasColumnName("fechaNacimiento");
            entity.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(450);
            entity.Property(e => e.IdRol)
                .IsRequired()
                .HasMaxLength(450)
                .HasColumnName("idRol");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombres");
            entity.Property(e => e.Pais).HasColumnName("pais");
            entity.Property(e => e.Rol).HasMaxLength(256);
            entity.Property(e => e.Telefono)
                .HasMaxLength(12)
                .HasColumnName("telefono");
            entity.Property(e => e.UrlImagen).HasColumnName("urlImagen");
        });

        modelBuilder.Entity<PortalUsuarioLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PortalUs__3213E83FB3210F63");

            entity.ToTable("PortalUsuarioLog");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Browser)
                .HasMaxLength(255)
                .HasColumnName("browser");
            entity.Property(e => e.Clase)
                .HasMaxLength(255)
                .HasColumnName("clase");
            entity.Property(e => e.FechaHora)
                .HasColumnType("datetime")
                .HasColumnName("fechaHora");
            entity.Property(e => e.InformacionAdicional).HasColumnName("informacionAdicional");
            entity.Property(e => e.Mensaje).HasColumnName("mensaje");
            entity.Property(e => e.Metodo)
                .HasMaxLength(255)
                .HasColumnName("metodo");
            entity.Property(e => e.Modulo).HasColumnName("modulo");
            entity.Property(e => e.Nivel)
                .HasMaxLength(50)
                .HasColumnName("nivel");
            entity.Property(e => e.UsuarioId)
                .HasMaxLength(255)
                .HasColumnName("usuarioId");
            entity.Property(e => e.UsuarioIp)
                .HasMaxLength(50)
                .HasColumnName("usuarioIP");
            entity.Property(e => e.UsuarioNombre)
                .HasMaxLength(255)
                .HasColumnName("usuarioNombre");
        });

        modelBuilder.Entity<UsuariosV2>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("usuariosV2_PK");

            entity.ToTable("UsuariosV2");

            entity.Property(e => e.UsuarioId).HasColumnName("usuarioID");
            entity.Property(e => e.ActualizadoPor)
                .IsRequired()
                .HasMaxLength(101)
                .HasColumnName("actualizadoPor");
            entity.Property(e => e.AgendaClave)
                .HasMaxLength(50)
                .HasColumnName("agendaClave");
            entity.Property(e => e.CentroCosto2Id).HasColumnName("centroCosto2ID");
            entity.Property(e => e.Contrasena)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("contrasena");
            entity.Property(e => e.Deshabilitado).HasColumnName("deshabilitado");
            entity.Property(e => e.EmpleadoId).HasColumnName("empleadoID");
            entity.Property(e => e.EsSistema).HasColumnName("esSistema");
            entity.Property(e => e.GrupoId).HasColumnName("grupoID");
            entity.Property(e => e.PuedeBorrarActa).HasColumnName("puedeBorrarActa");
            entity.Property(e => e.PuedeBorrarConclu).HasColumnName("puedeBorrarConclu");
            entity.Property(e => e.PuedeBorrarResumen).HasColumnName("puedeBorrarResumen");
            entity.Property(e => e.PuedeRevertirEval).HasColumnName("puedeRevertirEval");
            entity.Property(e => e.RefAlumnoId).HasColumnName("refAlumnoID");
            entity.Property(e => e.RefHistoriaClinica).HasColumnName("refHistoriaClinica");
            entity.Property(e => e.RefPersonalId)
                .HasDefaultValue(1)
                .HasColumnName("refPersonalID");
            entity.Property(e => e.SistemaId).HasColumnName("sistemaID");
            entity.Property(e => e.UltimaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("ultimaActualizacion");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(100)
                .HasColumnName("userEmail");
            entity.Property(e => e.UsuarioNombre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("usuarioNombre");
        });


        OnModelCreatingGeneratedProcedures(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    public void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder
    .HasAnnotation("ProductVersion", "8.0.3")
    .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
        {
            b.Property<string>("Id")
                .HasColumnType("nvarchar(450)");

            b.Property<int>("AccessFailedCount")
                .HasColumnType("int");

            b.Property<string>("ConcurrencyStamp")
                .IsConcurrencyToken()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("Discriminator")
                       .HasMaxLength(13)
                       .HasColumnType("nvarchar(13)");

            b.Property<string>("Email")
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            b.Property<bool>("EmailConfirmed")
                .HasColumnType("bit");

            b.Property<bool>("LockoutEnabled")
                .HasColumnType("bit");

            b.Property<DateTimeOffset?>("LockoutEnd")
                .HasColumnType("datetimeoffset");

            b.Property<string>("NormalizedEmail")
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            b.Property<string>("NormalizedUserName")
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            b.Property<string>("PasswordHash")
                .HasColumnType("nvarchar(max)");

            b.Property<string>("PhoneNumber")
                .HasColumnType("nvarchar(max)");

            b.Property<bool>("PhoneNumberConfirmed")
                .HasColumnType("bit");

            b.Property<string>("SecurityStamp")
                .HasColumnType("nvarchar(max)");

            b.Property<bool>("TwoFactorEnabled")
                .HasColumnType("bit");

            b.Property<string>("UserName")
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            b.HasKey("Id");

            b.HasIndex("NormalizedEmail")
                .HasDatabaseName("EmailIndex");

            b.HasIndex("NormalizedUserName")
                .IsUnique()
                .HasDatabaseName("UserNameIndex")
                .HasFilter("[NormalizedUserName] IS NOT NULL");

            b.ToTable("AspNetUsers", (string)null);
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
        {
            b.Property<string>("Id")
                .HasColumnType("nvarchar(450)");

            b.Property<string>("ConcurrencyStamp")
                .IsConcurrencyToken()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("Name")
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            b.Property<string>("NormalizedName")
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            b.HasKey("Id");

            b.HasIndex("NormalizedName")
                .IsUnique()
                .HasDatabaseName("RoleNameIndex")
                .HasFilter("[NormalizedName] IS NOT NULL");

            b.ToTable("AspNetRoles", (string)null);
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<string>("ClaimType")
                .HasColumnType("nvarchar(max)");

            b.Property<string>("ClaimValue")
                .HasColumnType("nvarchar(max)");

            b.Property<string>("RoleId")
                .IsRequired()
                .HasColumnType("nvarchar(450)");

            b.HasKey("Id");

            b.HasIndex("RoleId");

            b.ToTable("AspNetRoleClaims", (string)null);
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<string>("ClaimType")
                .HasColumnType("nvarchar(max)");

            b.Property<string>("ClaimValue")
                .HasColumnType("nvarchar(max)");

            b.Property<string>("UserId")
                .IsRequired()
                .HasColumnType("nvarchar(450)");

            b.HasKey("Id");

            b.HasIndex("UserId");

            b.ToTable("AspNetUserClaims", (string)null);
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
        {
            b.Property<string>("LoginProvider")
                .HasColumnType("nvarchar(450)");

            b.Property<string>("ProviderKey")
                .HasColumnType("nvarchar(450)");

            b.Property<string>("ProviderDisplayName")
                .HasColumnType("nvarchar(max)");

            b.Property<string>("UserId")
                .IsRequired()
                .HasColumnType("nvarchar(450)");

            b.HasKey("LoginProvider", "ProviderKey");

            b.HasIndex("UserId");

            b.ToTable("AspNetUserLogins", (string)null);
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
        {
            b.Property<string>("UserId")
                .HasColumnType("nvarchar(450)");

            b.Property<string>("RoleId")
                .HasColumnType("nvarchar(450)");

            b.HasKey("UserId", "RoleId");

            b.HasIndex("RoleId");

            b.ToTable("AspNetUserRoles", (string)null);
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
        {
            b.Property<string>("UserId")
                .HasColumnType("nvarchar(450)");

            b.Property<string>("LoginProvider")
                .HasColumnType("nvarchar(450)");

            b.Property<string>("Name")
                .HasColumnType("nvarchar(450)");

            b.Property<string>("Value")
                .HasColumnType("nvarchar(max)");

            b.HasKey("UserId", "LoginProvider", "Name");

            b.ToTable("AspNetUserTokens", (string)null);
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
        {
            b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                .WithMany()
                .HasForeignKey("RoleId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
        {
            b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
        {
            b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
        {
            b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                .WithMany()
                .HasForeignKey("RoleId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
        {
            b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });
    }

}