CREATE TABLE [dbo].[autori] (
    [id_autore] INT           IDENTITY (1000, 1) NOT NULL,
    [nome]      VARCHAR (100) NOT NULL,
    [cognome]   VARCHAR (100) NOT NULL,
    [mail]      VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([id_autore] ASC),
    CONSTRAINT [secondary_key_author] UNIQUE NONCLUSTERED ([nome] ASC, [cognome] ASC, [mail] ASC)
);

