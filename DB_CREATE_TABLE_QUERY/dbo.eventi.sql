CREATE TABLE [dbo].[eventi] (
    [id]                  INT      IDENTITY (1, 1) NOT NULL,
    [codice_libro]        BIGINT   NOT NULL,
    [data_event]          DATETIME NOT NULL,
    [svolto]              BIT      NOT NULL,
    [numero_partecipanti] INT      DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

