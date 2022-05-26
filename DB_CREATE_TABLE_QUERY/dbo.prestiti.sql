CREATE TABLE [dbo].[prestiti] (
    [id]                   INT  NOT NULL,
    [id_documento]         INT  NOT NULL,
    [tessera_utente]       INT  NOT NULL,
    [data_inizio_prestito] DATE NOT NULL,
    [durata_in_giorni]     INT  NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [ref_utente_prestito] FOREIGN KEY ([tessera_utente]) REFERENCES [dbo].[utenti] ([id_tessera]),
    CONSTRAINT [ref_libro_prestito] FOREIGN KEY ([id_documento]) REFERENCES [dbo].[documenti] ([codice])
);

