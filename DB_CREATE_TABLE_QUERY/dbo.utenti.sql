CREATE TABLE [dbo].[utenti] (
    [id_tessera] INT           IDENTITY (1000, 1) NOT NULL,
    [nome]       VARCHAR (255) NOT NULL,
    [cognome]    VARCHAR (255) NOT NULL,
    [mail]       VARCHAR (255) NOT NULL,
    [telefono]   VARCHAR (16)  NULL,
    PRIMARY KEY CLUSTERED ([id_tessera] ASC),
    CONSTRAINT [SECONDARY_KEY] UNIQUE NONCLUSTERED ([nome] ASC, [cognome] ASC, [mail] ASC)
);

